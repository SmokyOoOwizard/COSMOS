using COSMOS.Core.EventDispacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Equipment
{
    public class Item : DataWithEvents
    {
        public const uint ITEM_TYPE_CAN_SELL = 1;

        public uint ItemType { get; protected set; }
        public string[] SecondaryItemType { get; protected set; }
        public float Weight { get; protected set; }
        public float Volume { get; protected set; }

    }
}
