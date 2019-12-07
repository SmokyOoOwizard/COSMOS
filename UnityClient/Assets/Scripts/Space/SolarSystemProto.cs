using COSMOS.Relations;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using UnityEngine;
using COSMOS.HelpfullStuff;

namespace COSMOS.Space
{
    [BindProto("SolarSystem")] 
    public class SolarSystemProto
    {
        [BindProto(true)]
        public lstring Name;
        [BindProto("Position", true)]
        public Vector2 PosOnMap;
        [BindProto("WarpSafe",true)]
        public float WarpSafeDistance = 2;
        [BindProto("Asteroids")]
        public float AsteroidPercent;
        [BindProto]
        public List<PlanetProto> Planets = new List<PlanetProto>();
        [BindProto]
        public List<SpaceStation> SpaceStations = new List<SpaceStation>();

        public SolarSystemProto()
        {

        }
    
        public Dictionary<SpaceObject, Fraction> GetFractions()
        {
            Dictionary<SpaceObject, Fraction> fractions = new Dictionary<SpaceObject, Fraction>();
            for (int i = 0; i < Planets.Count; i++)
            {
                var frac = Planets[i].GetFraction();
                fractions.Add(Planets[i], frac);
            }
            for (int i = 0; i < SpaceStations.Count; i++)
            {
                var frac = SpaceStations[i].GetFraction();
                fractions.Add(SpaceStations[i], frac);
            }

            return fractions;
        }
    }
}
