using COSMOS.Relations;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using UnityEngine;

namespace COSMOS.Space
{
    [Serializable]
    public class SolarSystemProto
    {
        [XmlAttribute("Name")]
        public lstring Name;
        [XmlElement("Position")]
        public Vector2 PosOnMap;
        [XmlAttribute("WarpSafe")]
        public float WarpSafeDistance = 2;
        [XmlArray("Planets")]
        public List<PlanetProto> Planets = new List<PlanetProto>();
        [XmlArray("Stations")]
        public List<SpaceStation> SpaceStaions = new List<SpaceStation>();
        [XmlAttribute("Asteroids")]
        public float AsteroidPercent;

        public Dictionary<SpaceObject, Fraction> GetFractions()
        {
            Dictionary<SpaceObject, Fraction> fractions = new Dictionary<SpaceObject, Fraction>();
            for (int i = 0; i < Planets.Count; i++)
            {
                var frac = Planets[i].GetFraction();
                fractions.Add(Planets[i], frac);
            }
            for (int i = 0; i < SpaceStaions.Count; i++)
            {
                var frac = SpaceStaions[i].GetFraction();
                fractions.Add(SpaceStaions[i], frac);
            }

            return fractions;
        }
    }
}
