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
        public int id;
        public string name;

        public DateTime creationDate;
        public DateTime lastEditDate;

        public int cardCount = 0;

        public Box[] boxes;
    }
}
