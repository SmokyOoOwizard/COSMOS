﻿using COSMOS.Relations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COSMOS.Space
{
    public class SolarSystem
    {
        public lstring Name;
        public Vector2 PosOnMap;
        public List<Planet> Planets = new List<Planet>();
        public List<SpaceStation> SpaceStaions = new List<SpaceStation>();
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
