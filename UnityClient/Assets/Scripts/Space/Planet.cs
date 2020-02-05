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

        float Angle = 0;

        public override Vector2 GetPos()
        {
            Vector2 pos = new Vector2();

            pos.x = Mathf.Sin(Angle) * OrbitSize;
            pos.y = Mathf.Cos(Angle) * OrbitSize;


            return pos;
        }

        public override void Update(float delta)
        {
            Angle = (Angle + (OrbitSpeed * delta) / OrbitSize) % 360;
        }
    }
}
