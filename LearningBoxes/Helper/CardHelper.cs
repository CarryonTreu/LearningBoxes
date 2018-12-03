using LearningBoxes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LearningBoxes.Helper {
    class CardHelper {

        public static Card CreateCard(int cardId, string cardName = "") {
            Card tmpCard = new Card();

            tmpCard.id = cardId;
            tmpCard.name = cardName;

            DateTime now = DateTime.Now;
            tmpCard.creationDate = now;
            tmpCard.lastEditDate = now;

            //TODO handle question and awnser
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            tmpCard.frontInk = (string)localSettings.Values[Constants.tmpInkFrontFileName];
            tmpCard.backInk = (string)localSettings.Values[Constants.tmpInkBackFileName];
            return tmpCard;
        }
    }
}
