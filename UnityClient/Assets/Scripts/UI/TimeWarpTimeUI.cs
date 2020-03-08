using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using COSMOS;
using System;
using COSMOS.Player;

namespace COMOS.UI
{
    class TimeWarpTimeUI : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI timeText;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (GameData.GetPlayerData().CurrentWarp != null && GameData.GetPlayerData().CurrentWarp.Status != WarpStatus.Idle)
            {
                WarpStatus status = GameData.GetPlayerData().CurrentWarp.Status;
                int seconds = 0;
                if (status == WarpStatus.Charge)
                {
                    seconds = (int)(GameData.GetPlayerData().CurrentWarp.EndChargeTime - GameData.CurrentDate).TotalSeconds;
                }
                else if (status == WarpStatus.Warp)
                {
                    seconds = (int)(GameData.GetPlayerData().CurrentWarp.EndWarpTime - GameData.CurrentDate).TotalSeconds;
                }
                seconds = Mathf.Clamp(seconds, 0, 9999);
                timeText.SetText(seconds.ToString());
            }
            else
            {
                timeText.SetText(GameData.CurrentDate.ToString("HH:mm"));
            }
        }
    }
}
