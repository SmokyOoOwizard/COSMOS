using COSMOS.Character;
using COSMOS.Core;
using COSMOS.Equipment;
using COSMOS.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS
{
    public static class GameData
    {
        public static bool WorldPause { get; private set; }
        public static DateTime CurrentDate { get; set; }

        public static readonly Notifications PlayerNotifications = new Notifications();

        static PlayerData playerData;

        public static COSMOS.Character.Character PlayerCharacter { get; set; }
        public static event Action OnEquipmentPartsUpdate;
        public static event Action OnInventoryUpdate;


        public static Inventory[] GetCharacterInventories()
        {
            return new Inventory[] { new Inventory() { MaxVolume = 10, MaxWeight = 10 } };
        }

        public static EquipmentPart[] GetCharacterEquipments()
        {
            var q = new EquipmentPart();
            q.AddRule(new EquipmentRule());
            q.AddRule(new EquipmentRule());

            return new EquipmentPart[] { q };
        }
        public static PlayerData GetPlayerData()
        {
            if (playerData == null)
            {
                playerData = new PlayerData();
                playerData.CurrentWarp = new WarpState();
                playerData.CurrentWarp.EndChargeTime = CurrentDate.AddMinutes(10);
                playerData.CurrentWarp.EndWarpTime = CurrentDate.AddMinutes(25);

            }
            return playerData;
        }
    }
}
