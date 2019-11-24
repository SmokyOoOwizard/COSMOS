﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COSMOS.Space {
    public class SolarSystem : MonoBehaviour
    {
        public static SolarSystem CurrentSystem { get; protected set; }
        public SolarSystemProto Proto { get; protected set; }
        public List<Planet> InstancePlanets = new List<Planet>();

        private void Awake()
        {
            SolarSystemProto p = new SolarSystemProto();
            p.Planets = new List<PlanetProto>();
            p.Planets.Add(new PlanetProto() { OrbitSize = 20, OrbitSpeed = 1f, Size = 5 });
            Init(p);
        }

        public void Init(SolarSystemProto proto)
        {
            Proto = proto;
            CurrentSystem = this;

            foreach (var planet in proto.Planets)
            {
                Planet p = Planet.CreatePlanet(planet, this.gameObject);
                InstancePlanets.Add(p);
            }
        }
    }
}