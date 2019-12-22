using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace COSMOS.UI
{
    public class MapUI : MonoBehaviour
    {
        public TMP_InputField Search;
        public MapZoomUI Zoom;

        [Header("Solar system")]
        [Header("Description")]
        #region SolarSystem
        public TextMeshProUGUI SystemName;
        public TextMeshProUGUI SystemPosition;
        public TextMeshProUGUI SystemPlanetCount;
        public TextMeshProUGUI SystemSpaceStationCount;
        public RectTransform SystemEvents;
        public RectTransform SystemObjects;
        public TextMeshProUGUI SystemDescription;
        #endregion

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}