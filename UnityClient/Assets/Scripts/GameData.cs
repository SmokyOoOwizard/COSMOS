using COSMOS.Character;
using COSMOS.Core;
using COSMOS.Equipment;
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
        public static DateTime CurrentDate { get; private set; }
        public static PlayerData PlayerData { get; private set; }
        
        public static COSMOS.Character.Character PlayerCharacter { get; set; }
        public static event Action OnEquipmentPartsUpdate;
        public static event Action OnInventoryUpdate;

        public static readonly Notifications PlayerNotifications = new Notifications();

        public static Inventory[] GetCharacterInventories()
        {
            return new Inventory[0];
        }

        public static EquipmentPart[] GetCharacterEquipments()
        {
            return new EquipmentPart[0];
        }
    }
}
