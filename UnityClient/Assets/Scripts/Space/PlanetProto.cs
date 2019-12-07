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
    [BindProto("Planet")]
    public class PlanetProto : SpaceObject
    {
        [BindProto(true)]
        public float Size;
        [BindProto(true)]
        public float OrbitSpeed;
        [BindProto(true)]
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
