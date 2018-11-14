using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LearningBoxes.Model {
    class Box {

        private DateTime creationDate;
        private DateTime lastEditDate;

        private string boxName;
        private int cardCount;

        private Card cards = new Card();


        public Box() {

        }

    }
}
