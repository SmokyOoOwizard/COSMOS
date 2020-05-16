using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.SpaceShip
{
    public partial class SpaceShip
    {
        public float CalculateForwardEngineForce(float power)
        {
            if (MainEngine != null)
            {
                return MainEngine.CalculateEngineForce(power);
            }
            return 0;
        }
        public float UseForwardEngine(float power)
        {
            if (MainEngine != null)
            {
                float fuelConsumption = power * MainEngine.FuelConsumption * Time.deltaTime;
                float fuelUsedUp = UseFuel(fuelConsumption, MainEngine.FuelType);
                return MainEngine.CalculateEngineForce(power * ((fuelConsumption - fuelUsedUp) / fuelConsumption));
            }
            return 0;
        }
        public float CalculateBrakingEngineForce(float power)
        {
            if (BrakingEngine != null)
            {
                return BrakingEngine.CalculateEngineForce(power);
            }
            return 0;
        }
        public float UseBrakingEngine(float power)
        {
            if (BrakingEngine != null)
            {
                float fuelConsumption = power * BrakingEngine.FuelConsumption * Time.deltaTime;
                float fuelUsedUp = UseFuel(fuelConsumption, BrakingEngine.FuelType);
                return BrakingEngine.CalculateEngineForce(power * ((fuelConsumption - fuelUsedUp) / fuelConsumption));
            }
            return 0;
        }
        public float CalculateTurnEngineForce(float power)
        {
            if (TurnEngines != null)
            {
                return TurnEngines.CalculateEngineForce(power);
            }
            return 0;
        }
        public float UseTurnEngine(float power)
        {
            if (TurnEngines != null)
            {
                float fuelConsumption = power * TurnEngines.FuelConsumption * Time.deltaTime;
                float fuelUsedUp = UseFuel(fuelConsumption, TurnEngines.FuelType);
                return TurnEngines.CalculateEngineForce(power * ((fuelConsumption - fuelUsedUp) / fuelConsumption));
            }
            return 0;
        }
        public float CalculateSideEngineForce(float power)
        {
            if (SideEngines != null)
            {
                return SideEngines.CalculateEngineForce(power);
            }
            return 0;
        }
        public float UseSideEngine(float power)
        {
            if (SideEngines != null)
            {
                float fuelConsumption = power * SideEngines.FuelConsumption * Time.deltaTime;
                float fuelUsedUp = UseFuel(fuelConsumption, SideEngines.FuelType);
                return SideEngines.CalculateEngineForce(power * ((fuelConsumption - fuelUsedUp) / fuelConsumption));
            }
            return 0;
        }
    }
}
