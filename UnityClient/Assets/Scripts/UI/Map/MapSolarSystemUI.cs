using COSMOS.Space;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace COSMOS.UI.Map
{
    public class MapSolarSystemUI : MonoBehaviour, IPointerDownHandler
    {
        public SolarSystem SolarSystem { get; private set; }

        List<MapPlanetUI> InstancePlanets = new List<MapPlanetUI>();

        public void OnPointerDown(PointerEventData eventData)
        {
            GalaxyMapUI.instance.SelectSystem(SolarSystem);
        }
        public void Init(SolarSystem system)
        {
            SolarSystem = system;
            gameObject.name = system.Name.Key;
            createSystemStuff();
        }
        void deleteSystemStuff()
        {
            foreach (var planet in InstancePlanets)
            {
                Destroy(planet.gameObject);
            }
            InstancePlanets.Clear();
        }
        void createSystemStuff()
        {
            deleteSystemStuff();
            foreach (var planet in SolarSystem.Planets)
            {
                MapPlanetUI planetUI = MapPlanetUI.Spawn(planet);
                planetUI.transform.SetParent(transform);

                InstancePlanets.Add(planetUI);
            }
        }
    }
}
