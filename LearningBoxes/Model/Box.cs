﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LearningBoxes.Model {
    class Box {

        private DateTime creationDate;
        private DateTime lastEditDate;

        private int id;
        private int daysBetweenTest;
        private int cardCount;

        private List<Card> cardList;


        public Box() {

        }

    }
}
