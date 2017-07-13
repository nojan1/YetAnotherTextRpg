using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace YetAnotherTextRpg.Models
{
    public class Pickup
    {
        [XmlAttribute]
        public string Phrase { get; set; }
        [XmlAttribute]
        public string ItemId { get; set; }
        [XmlAttribute]
        public string Conditional { get; set; }

        public Item Item { get; set; }
    }
}
