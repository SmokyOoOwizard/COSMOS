using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Equipment;

namespace COSMOS.SpaceShip
{
    public class WarpEngine : Item
    {
        public float WarpSpeed { get; protected set; }
        public float ChargeTime { get; protected set; }
        public float ChargeFuelConsumption { get; protected set; }
        public float WarpFuelConsumption { get; protected set; }
    }
}
