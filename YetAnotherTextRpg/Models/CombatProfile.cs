using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace YetAnotherTextRpg.Models
{
    public class CombatProfile
    {
        [XmlAttribute]
        public int Strength { get; set; }
        [XmlAttribute]
        public int Attack { get; set; }
        [XmlAttribute]
        public int Defense { get; set; }
        [XmlAttribute]
        public int Armor { get; set; }
    }
}
