using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningBoxes.Model {
    class Card {

        private DateTime creationDate;
        private DateTime lastEditDate;

        private string cardName;
        private int    cardId;
        private string cardQuestion;
        private string cardAwnser;

        public Card(string cardName = "",
                    string cardQuestion = "",
                    string cardAwnser = "") {
            //TODO generate new id
            this.cardId = 0;

            this.cardName = cardName;
            this.cardQuestion = cardQuestion;
            this.cardAwnser = cardAwnser;
            DateTime current = new DateTime();
            this.creationDate = current;
            this.lastEditDate = current;
        }

        public void setCardName(string value) {
            this.cardName = value;
            this.lastEditDate = new DateTime();
        }
        public string getCardName() {
            return this.cardName;
        }
    }
}
