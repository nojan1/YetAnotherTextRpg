using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace YetAnotherTextRpg.Models
{
    public class Encounter
    {
        [XmlAttribute]
        public bool CanRun { get; set; } = true;
        [XmlAttribute]
        public string RunConditional { get; set; }
        [XmlAttribute]
        public string OnVictoryAction { get; set; }
        [XmlAttribute]
        public string OnDefeatAction { get; set; }

        public string InitialText { get; set; }
        public List<Enemy> Enemies { get; set; }
    }

    public class Enemy : CombatProfile
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string ImagePath { get; set; }
        [XmlAttribute]
        public int Health { get; set; }
        [XmlAttribute]
        public string OnDeathAction { get; set; }
        [XmlAttribute]
        public string OnTurnAction { get; set; }
    }
}
