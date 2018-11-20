using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Windows.Storage;

namespace LearningBoxes.Model {
    [XmlRootAttribute("Deck", Namespace = "LearningBoxes.Model", IsNullable = false)]
    public class Deck {
        public int id { get; set; }
        public string name { get; set; }

        public DateTime creationDate { get; set; }
        public DateTime lastEditDate { get; set; }

        public int latestCardId { get; set; }
        public int cardCount { get; set; }

        public Box[] boxes{ get; set; }
    }
}
