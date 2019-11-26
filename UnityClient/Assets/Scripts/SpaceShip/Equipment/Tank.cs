using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Equipment;

namespace COSMOS.SpaceShip.Equipment
{
    public class Tank : ShipEquipment
    {
        public string FuelType { get; protected set; }
        public float FuelVolume { get; protected set; }
        public float MaxFuelVolume { get; protected set; }

        public Tank(string fuelType, float maxFuelVolume, float fuelVolume)
        {
            FuelType = fuelType;
            FuelVolume = fuelVolume;
            MaxFuelVolume = maxFuelVolume;
        }

        public float Drain(float amount)
        {
            if (FuelVolume - amount > 0)
            {
                //FuelVolume -= amount;
                return 0;
            }
            else
            {
                float d = (FuelVolume - amount) + amount;
                //FuelVolume -= d;
                return d;
            }
        }
    }
}
