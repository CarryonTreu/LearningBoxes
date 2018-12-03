using LearningBoxes.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LearningBoxes.Model;
using Windows.Storage.Pickers;
using System.Threading.Tasks;

namespace LearningBoxes {
    public sealed partial class Cards : UserControl {

        //Binds
        string toggleButtonText = Constants.goToCardBack;
        string activeDeckNameText = "Active Deck: "+(string)ApplicationData.Current.LocalSettings.Values[Constants.activeDeck];
        string activeDeckName = (string)ApplicationData.Current.LocalSettings.Values[Constants.activeDeck];
        string inkFilePath;
        string inkFileName;
        FileUpdateStatus fileUploadStatus = FileUpdateStatus.Failed;

        public Cards() {
            this.InitializeComponent();
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[Constants.tmpInkFrontFileName] = null;
            localSettings.Values[Constants.tmpInkBackFileName] = null;
            if (activeDeckName == null || activeDeckName == "") {
                //TODO handle no active deck
                this.rootGrid.Visibility = Visibility.Collapsed;
            }
            this.SaveButton.Visibility = Visibility.Collapsed;
            // Set supported inking device types
            inkCanvas.InkPresenter.InputDeviceTypes =
                Windows.UI.Core.CoreInputDeviceTypes.Mouse |
                Windows.UI.Core.CoreInputDeviceTypes.Pen;
        }

        private async void ToggleBtn_Click(object sender, RoutedEventArgs e) {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            await SaveInkToGif();
            if (this.fileUploadStatus != FileUpdateStatus.Complete) {
                //TODO add logging and maybe handle in SafeToInk method
                return;
            }
            if (this.inkFilePath == null || this.inkFilePath == "") {
                //TODO add logging
                return;
            }
            //TODO refractor below
            //toggle button text and save filepath
            if (this.toggleButtonText == Constants.goToCardBack) {
                //Front -> Back
                localSettings.Values[Constants.tmpInkFrontFileName] = this.inkFileName;
                this.toggleButtonText = Constants.goToCardFront;
                this.SaveButton.Visibility = Visibility.Visible;
                //if back empty
                if (localSettings.Values[Constants.tmpInkBackFileName] == null || (string)localSettings.Values[Constants.tmpInkBackFileName] == "") {
                    inkCanvas.InkPresenter.StrokeContainer.Clear();
                    await SaveInkToGif();
                    localSettings.Values[Constants.tmpInkBackFileName] = this.inkFileName;
                } else {
                    await LoadInkToCanvas((string)localSettings.Values[Constants.tmpInkBackFileName]);
                }
            } else {
                //Back -> Front
                localSettings.Values[Constants.tmpInkBackFileName] = this.inkFileName;
                this.toggleButtonText = Constants.goToCardBack;
                await LoadInkToCanvas((string)localSettings.Values[Constants.tmpInkFrontFileName]);
            }
        }

        private async Task<bool> SaveInkToGif() {
            //init
            this.fileUploadStatus = FileUpdateStatus.Failed;
            this.inkFilePath = null;

            // get strokes
            IReadOnlyList<InkStroke> currentStrokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();

            //save to gif
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            string tmpFileName = $@"{DateTime.Now.Ticks}.gif";
            StorageFile file = await localFolder.CreateFileAsync(tmpFileName, CreationCollisionOption.GenerateUniqueName);
            if (file != null) {
                this.inkFilePath = file.Path;
                this.inkFileName = tmpFileName;
                CachedFileManager.DeferUpdates(file);
                IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite);

                using (IOutputStream outputStream = stream.GetOutputStreamAt(0)) {
                    await inkCanvas.InkPresenter.StrokeContainer.SaveAsync(outputStream);
                    await outputStream.FlushAsync();
                }
                stream.Dispose();
                this.fileUploadStatus = await CachedFileManager.CompleteUpdatesAsync(file);
            } else {
                // Operation cancelled.
            }

            return true;
        }

        /// <summary>
        /// Clear ink canvas of all ink strokes.
        /// </summary>
        /// <param name="sender">Source of the click event</param>
        /// <param name="e">Event args for the button click routed event</param>
        private void btnClear_Click(object sender, RoutedEventArgs e) {
            inkCanvas.InkPresenter.StrokeContainer.Clear();
        }

        private async Task<bool> LoadInkToCanvas(string inkFileName) {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile inkFile = await storageFolder.GetFileAsync(inkFileName);
            if (inkFile != null) {
                // Open a file stream for reading.
                IRandomAccessStream stream = await inkFile.OpenAsync(FileAccessMode.Read);
                // Read from file.
                using (var inputStream = stream.GetInputStreamAt(0)) {
                    await inkCanvas.InkPresenter.StrokeContainer.LoadAsync(stream);
                }
                stream.Dispose();
            } else {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Load ink data from a file, deserialize it, and add it to ink canvas.
        /// </summary>
        private async void btnLoad_Click(object sender, RoutedEventArgs e) {
            // Let users choose their ink file using a file picker.
            // Initialize the picker.
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".gif");
            // Show the file picker.
            StorageFile file = await openPicker.PickSingleFileAsync();
            // User selects a file and picker returns a reference to the selected file.
            if (file != null) {
                // Open a file stream for reading.
                IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
                // Read from file.
                using (var inputStream = stream.GetInputStreamAt(0)) {
                    await inkCanvas.InkPresenter.StrokeContainer.LoadAsync(stream);
                }
                stream.Dispose();
            }
            // User selects Cancel and picker returns null.
            else {
                // Operation cancelled.
            }
        }

        /// <summary>
        /// Get ink data from ink canvas, serialize it, and save it to a file.
        /// </summary>
        private async void btnSave_Click(object sender, RoutedEventArgs e) {
            await SaveInkToGif();
            if (this.fileUploadStatus != FileUpdateStatus.Complete) {
                //TODO add logging
                return;
            }
            if (this.inkFilePath == null || this.inkFilePath == "") {
                //TODO add logging
                return;
            }

            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            string filePath = localFolder.Path + @"\" + activeDeckName + ".xml";
            string gifPath = this.inkFilePath;
            Deck activeDeck = DeckHelper.loadDeckObjectFromXML(filePath);
            string cardName = this.CardName.Text;
            Card newCard = new Card();
            if (cardName == "Card Name (optional)" || cardName == "") {
                newCard = CardHelper.CreateCard(++activeDeck.latestCardId);
            } else {
                newCard = CardHelper.CreateCard(++activeDeck.latestCardId, cardName);
            }

            activeDeck.boxes[0].cards.Add(newCard);
            ModelHelper.SaveFile(activeDeckName, activeDeck);

            //RESET
            this.inkFilePath = null;
            this.toggleButtonText = Constants.goToCardBack;
            //TODO instead of collapse disable and tooltip to add back
            this.SaveButton.Visibility = Visibility.Collapsed;
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[Constants.tmpInkFrontFileName] = null;
            localSettings.Values[Constants.tmpInkBackFileName] = null;
            inkCanvas.InkPresenter.StrokeContainer.Clear();
        }
    }
}
