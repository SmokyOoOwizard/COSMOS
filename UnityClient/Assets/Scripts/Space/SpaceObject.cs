using COSMOS.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.Space
{
    public abstract class SpaceObject
    {
        public abstract Vector2 GetPos();
        public abstract Fraction GetFraction();
    }
}
