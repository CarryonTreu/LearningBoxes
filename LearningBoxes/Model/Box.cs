using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LearningBoxes.Model {
    public class Box {
        public DateTime creationDate;
        public DateTime lastEditDate;

        public int id;
        public int daysBetweenTest;

        public int cardCount = 0;
        public Card[] cards = null;
    }
}
