using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LearningBoxes.Model {
    public class Box {
        protected DateTime creationDate;
        protected DateTime lastEditDate;

        protected int id;
        protected int daysBetweenTest;

        protected int cardCount;
        protected Card[] cards;
    }
}
