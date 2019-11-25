using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using COSMOS.Relations;
namespace COSMOS.Space
{
    [Serializable]
    public class PlanetProto : SpaceObject
    {
        [XmlAttribute("Size")]
        public float Size;
        [XmlAttribute("Speed")]
        public float OrbitSpeed;
        [XmlAttribute("Oribit")]
        public float OrbitSize;
        public override Fraction GetFraction()
        {
            return null;
        }

        public override Vector2 GetPos()
        {
            return new Vector2();
        }
    }
}
