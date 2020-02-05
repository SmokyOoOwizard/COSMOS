using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using COSMOS.Space;

namespace COSMOS.UI.Map
{
    public class MapPlanetUI : MonoBehaviour
    {
        public Planet Planet;

        private void Update()
        {
            transform.localPosition = new Vector3(Planet.GetPos().x, Planet.GetPos().y, 0);
        }

        public static MapPlanetUI Spawn(Planet planet)
        {
            GameObject go = new GameObject("MapPlanetUI", typeof(MapPlanetUI));
            var tmp = go.GetComponent<MapPlanetUI>();
            tmp.Planet = planet;
            return tmp;
        }
    }
}
