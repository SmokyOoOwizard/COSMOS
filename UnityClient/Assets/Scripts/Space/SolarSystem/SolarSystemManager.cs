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
        static void Init()
        {
            SolarSystemSceneManager.StartLoadSystem += () => { CurrentSystem = SolarSystemSceneManager.instance.SolarSystem; };
            LoadSolarSystems();
            systemsUpdateThread = new Thread(new ThreadStart(UpdateSolarSystems));
            systemsUpdateThread.Start();
            systemsFixedUpdateThread = new Thread(new ThreadStart(FixedUpdateSolarSystems));
            systemsFixedUpdateThread.Start();
        }
        [DeInitMethod()]
        static void Deinit()
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
                return true;
            }
            return false;
        }
        public static bool LoadSystem(SolarSystem system)
        {
            if (solarSystems.ContainsValue(system))
            {
                SolarSystemSceneManager.instance.LoadSystem(system);
                return true;
            }
            return false;
        }
        #endregion
        public static List<SolarSystem> SystemsOnMap(Rect rect, float importance)
        {
            return new List<SolarSystem>(systemsQuadTree.QueryRange(rect, importance));
        }
        public static bool CheckWarp(SolarSystem system, Vector2 posInSystem, SolarSystem target)
        {
            float sqrSafeDistance = system.WarpSafeDistance * system.WarpSafeDistance;
            if(sqrSafeDistance <= posInSystem.sqrMagnitude)
            {
                Vector2 v1 = posInSystem - system.PosOnMap; ;
                Vector2 v2 = target.PosOnMap - system.PosOnMap;

                float dx = v2.x - v1.x;
                float dy = v2.y - v1.y;

                float a = dx * dx + dy * dy;
                float b = 2 * (v1.x * dx + v1.y * dy);
                float c = v1.x * v1.x + v1.y * v1.y - sqrSafeDistance;

                if(-b < 0)
                {
                    return c < 0;
                }
                if(-b < (2 * a))
                {
                    return (4 * a * c - b * b) < 0;
                }
                return a + b + c < 0;
            }
            return false;
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
