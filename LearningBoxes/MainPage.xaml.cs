using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Input.Inking;
using Windows.Storage.Streams;
using LearningBoxes.Model;

namespace LearningBoxes {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        public MainPage() {
            this.InitializeComponent();
            // Set supported inking device types.
            inkCanvas.InkPresenter.InputDeviceTypes =
                Windows.UI.Core.CoreInputDeviceTypes.Mouse |
                Windows.UI.Core.CoreInputDeviceTypes.Pen;
        }

        private void OnNavigationViewItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args) {
            if (args.IsSettingsInvoked) {
                Debug.WriteLine("Settings invoked");
            } else {
                var invokedItem = args.InvokedItem;

                Debug.WriteLine("Invoked:" + invokedItem.ToString());
            }
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
        /// <param name="sender">Source of the click event</param>
        /// <param name="e">Event args for the button click routed event</param>
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
        /// <param name="sender">Source of the click event</param>
        /// <param name="e">Event args for the button click routed event</param>
        private async void btnSave_Click(object sender, RoutedEventArgs e) {
            // Get all strokes on the InkCanvas.
            IReadOnlyList<InkStroke> currentStrokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();

            // Strokes present on ink canvas.
            if (currentStrokes.Count > 0) {
                Windows.Storage.Pickers.FileSavePicker savePicker = new Windows.Storage.Pickers.FileSavePicker();
                savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add("GIF with embedded ISF", new List<string>() { ".gif" });
                savePicker.DefaultFileExtension = ".gif";
                savePicker.SuggestedFileName = "InkSample";

                Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null) {
                    Windows.Storage.CachedFileManager.DeferUpdates(file);
                    IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

                    using (IOutputStream outputStream = stream.GetOutputStreamAt(0)) {
                        await inkCanvas.InkPresenter.StrokeContainer.SaveAsync(outputStream);
                        await outputStream.FlushAsync();
                    }
                    stream.Dispose();

                    Windows.Storage.Provider.FileUpdateStatus status =
                        await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);

                    if (status == Windows.Storage.Provider.FileUpdateStatus.Complete) {
                        // File saved.
                    } else {
                        // File couldn't be saved.
                        // TODO give warning
                    }
                } else {
                    // Operation cancelled.
                }
            }

        }
    }
}
