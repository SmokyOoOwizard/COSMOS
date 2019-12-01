using COSMOS.Relations;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using UnityEngine;
using COSMOS.HelpfullStuff;
namespace COSMOS.Space
{
    [Serializable]
    public class SolarSystemProto
    {
        public lstring Name;
        public Vector2 PosOnMap;
        public float WarpSafeDistance = 2;
        public List<PlanetProto> Planets = new List<PlanetProto>();
        public List<SpaceStation> SpaceStaions = new List<SpaceStation>();
        public float AsteroidPercent;

        public SolarSystemProto()
        {

        }
        public SolarSystemProto(XmlElement xml)
        {
            if(xml.Name == "SolarSystem")
            {
                XmlAttributeCollection attributes = xml.Attributes;
                foreach (XmlAttribute att in attributes)
                {
                    switch (att.Name)
                    {
                        case "Name":
                            Name = att.Value;
                            break;
                        case "WarpSafe":
                            float? ws = Parser.ParseFloatN(att.Value); 
                            if(ws == null || !ws.HasValue)
                            {
                                Log.Error("fail parse float. Attribute: WarpSafe Value: " + att.Value);
                            }
                            WarpSafeDistance = ws.Value;
                            break;
                        case "Asteroids":
                            float? a = Parser.ParseFloatN(att.Value);
                            if (a == null || !a.HasValue)
                            {
                                Log.Error("fail parse float. Attribute: WarpSafe Value: " + att.Value);
                            }
                            AsteroidPercent = a.Value;
                            break;
                        case "Position":
                            Vector2? v2 = Parser.ParseVector2N(att.Value);
                            if (v2 == null || !v2.HasValue)
                            {
                                Log.Error("fail parse float. Attribute: WarpSafe Value: " + att.Value);
                            }
                            PosOnMap = v2.Value;
                            break;
                        default:
                            Log.Warning("unknown attribute: " + att.Name + " name: " + Name.Key);
                            break;
                    }
                }
                foreach (XmlElement child in xml.ChildNodes)
                {
                    try
                    {
                        if(child.Name == "Planets")
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("fail parse " + child.Name + "\n" + ex);
                    }
                }
            }
            else
            {
                throw new Exception("wrong type " + xml.Name);
            }
        }
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
