using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LearningBoxes.Model {
    public class Box {
        public DateTime creationDate { get; set; }
        public DateTime lastEditDate { get; set; }

        public int id { get; set; }
        public int daysBetweenTest { get; set; }

        public int cardCount { get; set; }
        public List<Card> cards { get; set; }
    }
}
