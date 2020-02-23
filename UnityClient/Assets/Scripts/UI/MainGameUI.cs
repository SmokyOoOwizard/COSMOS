using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using COSMOS.Core.Paterns;
using TMPro;

namespace COSMOS.UI
{
    public class MainGameUI : SingletonMono<MainGameUI>
    {
        [Header("TopBar")]
        public GameObject TopBarObj;
        public TextMeshProUGUI DateUI;
        public TextMeshProUGUI TimeUI;
        public TextMeshProUGUI MoneyUI;
        public GameObject EventBar;
        public MiniMapUI MiniMap;

        [Header("CharacterUI")]
        public ShipUI ShipUI;

        [Header("Menu bar")]
        public GameObject MenuBar;


        private void Awake()
        {
            InitPatern();
        }
    }
}
