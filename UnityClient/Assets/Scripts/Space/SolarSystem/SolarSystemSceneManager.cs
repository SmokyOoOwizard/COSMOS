using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using COSMOS.Core.Paterns;
namespace COSMOS.Space
{
    public class SolarSystemSceneManager : SingletonMono<SolarSystemSceneManager>
    {
        public const string PLANET_PREFAB_NAME = @"Prefabs\Planet";
        public static event Action StartLoadSystem;
        public static event Action EndLoadSystem;
        public SolarSystem SolarSystem;
        public List<PlanetScene> InstancePlanets = new List<PlanetScene>();
        private void Awake()
        {
            InitPatern();
        }

        internal void LoadSystem(SolarSystem ss)
        {
            StartLoadSystem?.Invoke();
            if(SolarSystem != null)
            {
                UnloadSystem();
            }
            SolarSystem = ss;
            Log.Info("loading system: " + ss.Name.Key);
            StartCoroutine(Loading());
        }
        IEnumerator Loading()
        {
            if(SolarSystem != null)
            {
                foreach (var planet in SolarSystem.Planets)
                {
                    GameObject tmp = AssetsDatabase.LoadGameObject(PLANET_PREFAB_NAME);
                    tmp = Instantiate(tmp);
                    tmp.transform.SetParent(transform);
                    PlanetScene ps = tmp.GetComponent<PlanetScene>();
                    ps.Init(planet);
                    InstancePlanets.Add(ps);
                    yield return new WaitForEndOfFrame(); 
                }
            }
            EndLoadSystem?.Invoke();
            yield return null;
        }
        private void UnloadSystem()
        {
            foreach (var planet in InstancePlanets)
            {
                Destroy(planet.gameObject);
            }
            InstancePlanets.Clear();
        }
    }
}
