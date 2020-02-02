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
    }
}
