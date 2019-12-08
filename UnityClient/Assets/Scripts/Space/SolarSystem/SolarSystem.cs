using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

namespace COSMOS.Space {
    public class SolarSystem
    {
        public SolarSystemProto Proto { get; protected set; }
        public List<Planet> Planets = new List<Planet>();

        public SolarSystem(SolarSystemProto proto)
        {
            Proto = proto;
            foreach (var planet in proto.Planets)
            {
                Planets.Add(new Planet(planet));
            }
        }
    }
}