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
        public SolarSystem SolarSytem;

        public void OnPointerDown(PointerEventData eventData)
        {
            GalaxyMapUI.instance.SelectSystem(SolarSytem);
            Log.Info("Clicl on " + SolarSytem.Name.Key);
        }
    }
}
