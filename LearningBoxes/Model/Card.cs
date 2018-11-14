using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningBoxes.Model {
    class Card {
        private DateTime creationDate;
        private DateTime lastEditDate;
        private DateTime lastTestedDate;

        private string name;
        private int    id;
        private string frontInk;
        private string backInk;

        private int blockid;

        public Card(string cardName = "",
                    string cardQuestion = "",
                    string cardAwnser = "") {
            //TODO generate new id
            this.id = 0;

            this.name = cardName;
            this.frontInk = cardQuestion;
            this.backInk = cardAwnser;
            DateTime current = new DateTime();
            this.creationDate = current;
            this.lastEditDate = current;
        }

        public void setCardName(string value) {
            this.name = value;
            this.lastEditDate = new DateTime();
        }
        public string getCardName() {
            return this.name;
        }
    }
}
