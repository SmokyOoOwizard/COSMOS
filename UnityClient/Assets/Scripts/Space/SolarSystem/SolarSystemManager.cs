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
using System.Threading;

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
        public static SolarSystem MaxNotInpotanceSystem { get; private set; }

        static Dictionary<string, SolarSystem> solarSystems = new Dictionary<string, SolarSystem>();
        public static SolarSystemsQuadTree systemsQuadTree = new SolarSystemsQuadTree();

        public static SolarSystem CurrentSystem { get; private set; }
        public static event Action StartLoadSystem;
        public static event Action EndLoadSystem;

        static long lastTicks;
        static Thread systemsUpdateThread;
        static Thread systemsFixedUpdateThread;

        public const int SystemFixedUpdateLength = 100;
        
        [InitMethod(-1)]
        public static void Init()
        {
            LoadSolarSystems();
            systemsUpdateThread = new Thread(new ThreadStart(UpdateSolarSystems));
            systemsUpdateThread.Start();
            systemsFixedUpdateThread = new Thread(new ThreadStart(FixedUpdateSolarSystems));
            systemsFixedUpdateThread.Start();
        }
        [DeInitMethod()]
        public static void Deinit()
        {
            systemsUpdateThread.Abort();
            systemsFixedUpdateThread.Abort();
        }
        #region AddAndLoadSystems
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
                systemsQuadTree.Insert(system);
                if (MaxNotInpotanceSystem == null || 
                    system.ImportanceOnMap > MaxNotInpotanceSystem.ImportanceOnMap) MaxNotInpotanceSystem = system;
                return true;
            }
            return false;
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
        #endregion
        public static List<SolarSystem> SystemsOnMap(Rect rect, float importance)
        {
            return new List<SolarSystem>(systemsQuadTree.QueryRange(rect, importance));
        }
        static void UpdateSolarSystems()
        {
            lastTicks = DateTime.Now.Ticks;
            while (true)
            {
                long ticks = DateTime.Now.Ticks;
                TimeSpan delta = (new TimeSpan(ticks) - new TimeSpan(lastTicks));
                if(delta.TotalMilliseconds < 10)
                {
                    Thread.Sleep(new TimeSpan(0, 0, 0, 0, 10) - delta);
                    delta = new TimeSpan(0, 0, 0, 0, 10);
                }
                Parallel.ForEach(solarSystems, (system) =>
                {
                    system.Value.Update((float)delta.TotalSeconds);
                });
                lastTicks = ticks;
            }
        }
        static void FixedUpdateSolarSystems()
        {
            while (true)
            {
                DateTime pref = DateTime.Now;
                Parallel.ForEach(solarSystems, (system) =>
                {
                    system.Value.FixedUpdate();
                });
                TimeSpan delta = DateTime.Now - pref;
                delta = new TimeSpan(0, 0, 0, 0, SystemFixedUpdateLength) - delta;
                if(delta.Ticks > 0)
                {
                    Thread.Sleep(delta);
                }
            }
        }
    }
}
