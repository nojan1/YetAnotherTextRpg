using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace YetAnotherTextRpg.Models
{
    [XmlRoot(ElementName = "Scene", IsNullable = false)]
    public class Scene
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public List<Exit> Exits { get; set; } = new List<Exit>();
        public List<Pickup> Pickups { get; set; } = new List<Pickup>();
        public List<Trigger> Triggers { get; set; } = new List<Trigger>();
    }
}
