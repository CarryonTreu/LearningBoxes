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
        protected DateTime creationDate;
        protected DateTime lastEditDate;
        protected DateTime lastTestedDate;

        protected string name;
        protected int id;
        protected string frontInk;
        protected string backInk;

        protected int blockId;
    }
}
