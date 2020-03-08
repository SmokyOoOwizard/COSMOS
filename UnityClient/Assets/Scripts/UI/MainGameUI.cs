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
        public NotificationsUI NotificationsBar;
        public MiniMapUI MiniMap;

        [Header("CharacterUI")]
        public ShipUI ShipUI;

        [Header("Menu bar")]
        public GameObject MenuBar;

        int day = -1;
        private void Awake()
        {
            InitPatern();
        }

        private void Update()
        {
            if(GameData.CurrentDate.Day != day)
            {
                day = GameData.CurrentDate.Day;
                DateUI.SetText(GameData.CurrentDate.ToString("dd.MM.yyyy"));
            }

        }
    }
}
