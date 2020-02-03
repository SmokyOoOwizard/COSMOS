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

        public static MapPlanetUI Spawn(Planet planet)
        {
            GameObject go = new GameObject("MapPlanetUI", typeof(MapPlanetUI));
            return go.GetComponent<MapPlanetUI>();
        }
    }
}
