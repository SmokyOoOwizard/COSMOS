using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using COSMOS.Relations;
namespace COSMOS.Space
{
    public class PlanetProto : SpaceObject
    {
        public float Size;
        public float OrbitSpeed;
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
