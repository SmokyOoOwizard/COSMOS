using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Relations;
using UnityEngine;

namespace COSMOS.Space
{
    public class SpaceStation : SpaceObject
    {
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
