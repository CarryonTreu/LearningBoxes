using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace LearningBoxes.Model {
    public class Card {
        public DateTime creationDate { get; set; }
        public DateTime lastEditDate { get; set; }
        public DateTime lastTestedDate { get; set; }

        public string name { get; set; }
        public int id { get; set; }
        public string frontInk { get; set; }
        public string backInk { get; set; }

        public int blockId { get; set; }
    }
}
