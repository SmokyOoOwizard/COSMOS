using COSMOS.Core.EventDispacher;
using COSMOS.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Equipment
{
    public class Item : DataWithEvents, ICanPlaceInQuickSlot
    {
        public const uint ITEM_TYPE_CAN_SELL = 1;

        public string LKeyName { get; set; }
        public string LkeyDescription { get; set; }
        public string IconID { get; set; }
        public uint ItemType { get; protected set; }
        public string[] SecondaryItemType { get; protected set; }
        public float Weight { get; protected set; }
        public float Volume { get; protected set; }

    }
}
