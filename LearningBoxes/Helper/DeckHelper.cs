using LearningBoxes.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Diagnostics;
using Windows.Storage.Streams;

namespace LearningBoxes.Helper {
    public class DeckHelper {

        public static void CreateDeck(string inputName = "") {
            Deck newDeck = new Deck();

            //Id
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            int currentDeckId = (int)localSettings.Values["currentDeckId"];
            currentDeckId++;
            localSettings.Values["currentDeckId"] = currentDeckId;
            newDeck.id = currentDeckId;

            //Name
            string deckName;
            if (inputName.Equals("")) {
                deckName = "deck_" + newDeck.id.ToString();
            } else {
                deckName = inputName;
            }
            newDeck.name = deckName;

            //Date
            DateTime now = DateTime.Now;
            newDeck.creationDate = now;
            newDeck.lastEditDate = now;

            //Boxes
            newDeck.boxes = BoxHelper.CreateBoxes();

            //create File
            ModelHelper.CreateFile(deckName, newDeck);
        }

    }
}
