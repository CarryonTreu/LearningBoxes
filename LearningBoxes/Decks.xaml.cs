using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.UI;
using LearningBoxes.Helper;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LearningBoxes {
    public sealed partial class Decks : UserControl {
        public Decks() {
            this.InitializeComponent();
            CreateDeck.Visibility = Visibility.Collapsed;
            /**
            if (true) {
                Button myButton = new Button();
                myButton.Content = "Click Me!";

                this.rootGrid.Children.Add(myButton);
            }**/
        }

        private void AddDeck_Button(object sender, RoutedEventArgs e) {
            CreateDeck.Visibility = Visibility.Visible;
        }

        private void CreateDeck_Button(object sender, RoutedEventArgs e) {
            if (this.DeckName.Text == "") {
                this.SaveInfoText.Foreground = new SolidColorBrush(Colors.IndianRed);
                this.SaveInfoText.Text = "Please enter a Deckname!";
                return;
            }

            string deckName = this.DeckName.Text;
            // TODO add color
            DeckHelper.createDeck(deckName);
            this.DeckName.Text = "";
            this.SaveInfoText.Foreground = new SolidColorBrush(Colors.ForestGreen);
            this.SaveInfoText.Text = deckName + " was created successfully!";
        }
    }
}
