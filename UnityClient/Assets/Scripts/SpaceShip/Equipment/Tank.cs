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
        public bool Infinite { get; protected set; }
        public string FuelType { get; protected set; }
        public float FuelVolume { get; protected set; }
        public float FreeFuelVolume { get { return MaxFuelVolume - FuelVolume; } }
        public float MaxFuelVolume { get; protected set; }

        public Tank(string fuelType, float maxFuelVolume, float fuelVolume)
        {
            FuelType = fuelType;
            FuelVolume = fuelVolume;
            MaxFuelVolume = maxFuelVolume;
        }
        public float PourIn(float amount)
        {
            FuelVolume += amount;
            if(Infinite || FuelVolume <= MaxFuelVolume)
            {
                return 0;
            }
            else
            {
                float diff = FuelVolume - MaxFuelVolume;
                FuelVolume = MaxFuelVolume;
                return diff;
            }
        }
        public float Drain(float amount)
        {
            FuelVolume -= amount;
            if (Infinite || FuelVolume >= 0)
            {
                return 0;
            }
            else
            {
                float diff = Math.Abs(FuelVolume);
                FuelVolume = 0;
                return diff;
            }
        }
    }
}
