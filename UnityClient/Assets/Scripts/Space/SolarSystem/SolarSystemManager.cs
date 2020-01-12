using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using COSMOS.Prototype;
using System.Collections.ObjectModel;
using UnityEngine;

namespace COSMOS.Space
{
    [Manager]
    public static class SolarSystemManager
    {
        public static ReadOnlyDictionary<string, SolarSystemProto> SolarSystemPrototypes
        {
            get { return new ReadOnlyDictionary<string, SolarSystemProto>(solarSystemPrototypes); }
        }
        public static ReadOnlyDictionary<string, SolarSystem> SolarSystems
        {
            get { return new ReadOnlyDictionary<string, SolarSystem>(solarSystems);  }
        }
        public static ReadOnlyCollection<SolarSystemProto> SortedSolarSystemPrototypesByStarBrightness
        {
            get { return new ReadOnlyCollection<SolarSystemProto>(sortedSolarSystemPrototypesByStarBrightness); }
        }
        public static SolarSystemProto MaxFarSystem { get; private set; }
        static Dictionary<string, SolarSystemProto> solarSystemPrototypes = new Dictionary<string, SolarSystemProto>();
        static Dictionary<string, SolarSystem> solarSystems = new Dictionary<string, SolarSystem>();
        static List<SolarSystemProto> sortedSolarSystemPrototypesByStarBrightness = new List<SolarSystemProto>();
        public static SolarSystem CurrentSystem { get; private set; }
        public static event Action StartLoadSystem;
        public static event Action EndLoadSystem;

        [InitMethod(-1)]
        public static void Init()
        {
            SolarSystemSceneManager.StartLoadSystem += ()=>StartLoadSystem?.Invoke();
            SolarSystemSceneManager.EndLoadSystem += ()=>EndLoadSystem?.Invoke();
            LoadPrototypes();
            for (int i = 0; i < 100; i++)
            {
                sortedSolarSystemPrototypesByStarBrightness.Add(new SolarSystemProto() { PosOnMap = new UnityEngine.Vector2(i*10,i*10), SystemStar = new Star() { Brightness = 100 } });
            }
            MaxFarSystem = sortedSolarSystemPrototypesByStarBrightness[sortedSolarSystemPrototypesByStarBrightness.Count - 1];
            InitSolarSystems();
        }
        static void LoadPrototypes()
        {
            string config = AssetsDatabase.LoadPrototype("SolarSystems");
            if (!string.IsNullOrEmpty(config))
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(config);
                XmlElement xroot = xdoc.DocumentElement;
                if (xroot.Name == "SolarSystems")
                {
                    foreach (XmlElement item in xroot)
                    {
                        if (item.Name == "SolarSystem")
                        {
                            SolarSystemProto tmpProto = PrototypeManager.Parse<SolarSystemProto>(item);
                            if(tmpProto != null)
                            {
                                if (!AddSolarSystemProto(tmpProto))
                                {
                                    Log.Warning("duplicate SolarSystemProto. name: " + tmpProto.Name.Key);
                                }
                            }
                        }
                    }
                }
            }
        }
        static bool AddSolarSystemProto(SolarSystemProto ssp)
        {
            if (!solarSystemPrototypes.ContainsKey(ssp.Name.Key))
            {
                solarSystemPrototypes.Add(ssp.Name.Key, ssp);
                if (MaxFarSystem == null || MaxFarSystem.PosOnMap.sqrMagnitude < ssp.PosOnMap.sqrMagnitude)
                {
                    MaxFarSystem = ssp;
                }
                return true;
            }
            return false;
        }
        static void InitSolarSystems()
        {
            foreach (var system in solarSystemPrototypes)
            {
                solarSystems.Add(system.Key, new SolarSystem(system.Value));
            }
        }
        public static bool LoadSystem(string name)
        {
            if (solarSystems.ContainsKey(name))
            {
                SolarSystemSceneManager.instance.LoadSystem(solarSystems[name]);
                CurrentSystem = solarSystems[name];
                return true;
            }
            return false;
        }
    }
}
