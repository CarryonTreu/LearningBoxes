using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Input.Inking;
using Windows.Storage.Streams;

namespace LearningBoxes {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        public MainPage() {
            this.InitializeComponent();
           
        }
        
        private void OnNavigationViewItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args) {
            if (args.IsSettingsInvoked) {
                Debug.WriteLine("Settings invoked");
            } else {
                var invokedItem = args.InvokedItem;
                //Debug.WriteLine("Invoked:" + invokedItem.ToString());
                switch (invokedItem) {
                    case "Decks":
                        this.contentFrame.Content = new Decks();
                        break;
                    case "Cards":
                        this.contentFrame.Content = new Cards();
                        break;
                    default:
                        //TODO
                        break;
                }
                  
            }
        }
    }
}
