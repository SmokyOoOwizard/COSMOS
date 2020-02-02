using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
namespace COSMOS.Space
{
    [BindProto("Planet")]
    public class Planet : SpaceObject
    {
        [BindProto(true)]
        public float Size;
        [BindProto(true)]
        public float OrbitSpeed;
        [BindProto(true)]
        public float OrbitSize;

        public override Vector2 GetPos()
        {
            return new Vector2();
        }
    }
}
