using COSMOS.Core.EventDispacher;
using COSMOS.Player;
using COSMOS.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Equipment
{
    public class ItemType
    {
        public string Type { get; protected set; }
        public string[] SecondaryItemType { get; protected set; } = new string[0];
    }
    public class Item : DataWithEvents, ICanPlaceInQuickSlot, ICanPlaceInSlot
    {
        public const uint ITEM_TYPE_CAN_SELL = 1;

        public string LKeyName { get; set; }
        public string LkeyDescription { get; set; }
        public string IconID { get; set; }
        public string GOID { get; set; }
        public ItemType ItemType { get; protected set; }
        public float Weight { get; protected set; }
        public float Volume { get; protected set; }

        public InventoryController Inventory { get; protected set; } = null;

    }
}
