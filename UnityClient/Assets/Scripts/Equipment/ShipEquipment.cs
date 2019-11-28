using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Equipment
{
    public abstract class ShipEquipment : Item
    {
        public float Durability { get; protected set; }
        public float RateWear { get; protected set; }
    }
}
