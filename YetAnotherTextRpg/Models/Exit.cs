﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace YetAnotherTextRpg.Models
{
    public class Exit
    {
        [XmlAttribute]
        public Direction Direction { get; set; }
        [XmlAttribute]
        public string To { get; set; }
        [XmlAttribute]
        public string Conditional { get; set; }
    }
}
