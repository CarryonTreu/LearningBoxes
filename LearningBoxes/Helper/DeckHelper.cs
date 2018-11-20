using LearningBoxes.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
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
            int currentDeckId = (int)localSettings.Values[Constants.currentDeckId];
            currentDeckId++;
            localSettings.Values[Constants.currentDeckId] = currentDeckId;
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
            ModelHelper.SaveFile(deckName, newDeck);

            //change active deck to the new
            localSettings.Values[Constants.activeDeck] = deckName;
        }
        

        public static Deck loadDeckObjectFromXML(string filepath) {
            //Deck tmpDeck = new Deck();
            //XmlSerializer serializer = new XmlSerializer(typeof(Deck));
            //using (Stream reader = new FileStream(filepath, FileMode.Open)) {
            //    tmpDeck = (Deck)serializer.Deserialize(reader);
            //}
            //return tmpDeck;

            // Now we can read the serialized book ...  
            XmlSerializer reader = new XmlSerializer(typeof(Deck));
            StreamReader file = new StreamReader(filepath);
            Deck tmpDeck = (Deck)reader.Deserialize(file);
            file.Close();

            return tmpDeck;
        }
    }
}
