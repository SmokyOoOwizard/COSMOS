using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Space
{
    public class Planet
    {
        public readonly PlanetProto Proto;
        public float OrbitSpeed { get; protected set; }
        public float OrbitSize { get; protected set; }

        public Planet(PlanetProto proto)
        {
            Proto = proto;
            OrbitSize = proto.OrbitSize;
            OrbitSpeed = proto.OrbitSpeed;
        }
    }
}
