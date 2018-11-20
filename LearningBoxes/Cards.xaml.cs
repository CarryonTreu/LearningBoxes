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

namespace LearningBoxes {
    public sealed partial class Cards : UserControl {

        string activeDeckName = (string)ApplicationData.Current.LocalSettings.Values[Constants.activeDeck];

        public Cards() {
            this.InitializeComponent();
            if (activeDeckName == null || activeDeckName == "") {
                //TODO handle no active deck
                this.rootGrid.Visibility = Visibility.Collapsed;
            }
            // Set supported inking device types
            inkCanvas.InkPresenter.InputDeviceTypes =
                Windows.UI.Core.CoreInputDeviceTypes.Mouse |
                Windows.UI.Core.CoreInputDeviceTypes.Pen;
        }

        /// <summary>
        /// Clear ink canvas of all ink strokes.
        /// </summary>
        /// <param name="sender">Source of the click event</param>
        /// <param name="e">Event args for the button click routed event</param>
        private void btnClear_Click(object sender, RoutedEventArgs e) {
            inkCanvas.InkPresenter.StrokeContainer.Clear();
        }
        /// <summary>
        /// Load ink data from a file, deserialize it, and add it to ink canvas.
        /// </summary>
        private async void btnLoad_Click(object sender, RoutedEventArgs e) {
            // Let users choose their ink file using a file picker.
            // Initialize the picker.
            Windows.Storage.Pickers.FileOpenPicker openPicker =
                new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".gif");
            // Show the file picker.
            Windows.Storage.StorageFile file = await openPicker.PickSingleFileAsync();
            // User selects a file and picker returns a reference to the selected file.
            if (file != null) {
                // Open a file stream for reading.
                IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
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
            // Get all strokes on the InkCanvas
            IReadOnlyList<InkStroke> currentStrokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();

            // Strokes present on ink canvas
            if (currentStrokes.Count > 0) {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await localFolder.CreateFileAsync($@"{DateTime.Now.Ticks}.gif");
                Debug.WriteLine(file.Path);
                if (file != null) {
                    CachedFileManager.DeferUpdates(file);
                    IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite);

                    using (IOutputStream outputStream = stream.GetOutputStreamAt(0)) {
                        await inkCanvas.InkPresenter.StrokeContainer.SaveAsync(outputStream);
                        await outputStream.FlushAsync();
                    }
                    stream.Dispose();
                    FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);

                    if (status == FileUpdateStatus.Complete) {
                        // gif saved -> add card to deck
                        string filePath = localFolder.Path + "\\" + activeDeckName + ".xml";
                        string gifPath = file.Path;
                        Deck activeDeck = DeckHelper.loadDeckObjectFromXML(filePath);
                        Card newCard = CardHelper.CreateCard(++activeDeck.latestCardId, gifPath);

                        activeDeck.boxes[0].cards.Add(newCard);
                        ModelHelper.SaveFile(activeDeckName,activeDeck);

                    } else {
                        // File couldn't be saved.
                        // TODO give warning
                    }
                } else {
                    // Operation cancelled.
                }
            }

        }

        public void AddNewCardToDeck() {

        }
    }
}
