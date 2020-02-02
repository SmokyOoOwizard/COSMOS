using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using COSMOS.Prototype;
using COSMOS.UI.HelpfulStuff;
using System.Collections.ObjectModel;
using UnityEngine;

namespace COSMOS.Space
{
    [Manager]
    public static class SolarSystemManager
    {
        public static ReadOnlyDictionary<string, SolarSystem> SolarSystems
        {
            get { return new ReadOnlyDictionary<string, SolarSystem>(solarSystems);  }
        }

        public static SolarSystem MaxFarSystem { get; private set; }

        static Dictionary<string, SolarSystem> solarSystems = new Dictionary<string, SolarSystem>();
        static SolarSystemsQuadTree systemsQuadTree = new SolarSystemsQuadTree(new Rect());

        public static SolarSystem CurrentSystem { get; private set; }
        public static event Action StartLoadSystem;
        public static event Action EndLoadSystem;

        [InitMethod(-1)]
        public static void Init()
        {
            LoadSolarSystems();
            SolarSystem tm1, tm2;
            tm1 = new SolarSystem();
            tm1.Name = "test 1";
            tm1.PosOnMap = new Vector2(10, 10);
            tm2 = new SolarSystem();
            tm2.Name = "test 2";

            systemsQuadTree = new SolarSystemsQuadTree(new Rect(0, 0, 20, 20));
            systemsQuadTree.Insert(tm1);
            systemsQuadTree.Insert(tm2);

        }
        static void LoadSolarSystems()
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
                            SolarSystem tmpSystem = PrototypeManager.Parse<SolarSystem>(item);
                            if(tmpSystem != null)
                            {
                                if (!AddSolarSystem(tmpSystem))
                                {
                                    Log.Warning("duplicate SolarSystem name: " + tmpSystem.Name.Key);
                                }
                            }
                        }
                    }
                }
            }
        }
        public static bool AddSolarSystem(SolarSystem system)
        {
            if (!solarSystems.ContainsKey(system.Name.Key))
            {
                solarSystems.Add(system.Name.Key, system);
                return true;
            }
            return false;
        }
        public static List<SolarSystem> SystemsOnMap(Rect rect, float importance)
        {
            return new List<SolarSystem>(systemsQuadTree.QueryRange(rect, importance));
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
