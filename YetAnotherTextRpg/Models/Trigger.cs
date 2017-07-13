using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace YetAnotherTextRpg.Models
{
    public class Trigger
    {
        [XmlAttribute]
        public string Phrase { get; set; }
        [XmlAttribute]
        public string Action { get; set; }
        [XmlAttribute]
        public string Conditional { get; set; }
    }
}
