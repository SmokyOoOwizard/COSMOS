using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Equipment;
namespace COSMOS.SpaceShip.Equipment
{
    public class EnergyTank : ShipEquipment
    {
        public float EnergyVolume { get; protected set; }
        public float MaxFuelVolume { get; protected set; }
        public float SelfDischargePercent { get; protected set; }

        public float PourIn(float amount)
        {
            return 0;
        }
    }
}
