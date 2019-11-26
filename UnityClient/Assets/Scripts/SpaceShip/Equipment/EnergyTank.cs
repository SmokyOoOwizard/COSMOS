using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Equipment;
namespace COSMOS.SpaceShip.Equipment
{
    public class EnergyTank : Tank
    {
        public float SelfDischargePercent { get; protected set; }

        public EnergyTank(float maxEnergyVolume, float EnergyVolume) : base("Energy", maxEnergyVolume, EnergyVolume)
        {

        }
    }
}
