using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using UnityEngine;
using COSMOS.Core.HelpfulStuff;

namespace COSMOS.Space
{
    [BindProto("SolarSystem")] 
    public class SolarSystem
    {
        [BindProto(true)]
        public lstring Name;
        [BindProto("Position", true)]
        public Vector2 PosOnMap;
        [BindProto("ImportanceOnMap", true)]
        public float ImportanceOnMap;
        [BindProto("WarpSafe",true)]
        public float WarpSafeDistance = 2;
        [BindProto("Asteroids")]
        public float AsteroidPercent;
        [BindProto]
        public Star SystemStar;
        [BindProto]
        public List<Planet> Planets = new List<Planet>();
        [BindProto]
        public List<SpaceStation> SpaceStations = new List<SpaceStation>();

        public SolarSystem()
        {

        }

        public virtual void Update(float delta)
        {
            var planets = new List<Planet>(Planets);
            for (int i = 0; i < planets.Count; i++)
            {
                planets[i].Update(delta);
            }
            var stations = new List<SpaceStation>(SpaceStations);
            for (int i = 0; i < stations.Count; i++)
            {
                stations[i].Update(delta);
            }
        }

        public virtual void FixedUpdate()
        {
            var planets = new List<Planet>(Planets);
            for (int i = 0; i < planets.Count; i++)
            {
                planets[i].FixedUpdate();
            }
            var stations = new List<SpaceStation>(SpaceStations);
            for (int i = 0; i < stations.Count; i++)
            {
                stations[i].FixedUpdate();
            }
        } 
    }
}
