using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Equipment;
using COSMOS.Player;

namespace COSMOS.SpaceShip.Equipment
{
    public class WarpEngine : ShipEquipment, INeedEnergy
    {
        public float WarpSpeed { get; protected set; } = 1;
        public float ChargeTime { get; protected set; } = 5;
        public WarpStatus EngineState 
        { 
            get { return engineState; } 
            set { 
                if (value == WarpStatus.Idle) { EnergyConsumption = IdleEnergyConsumption; } 
                else if (value == WarpStatus.Charge) { EnergyConsumption = ChargeEnergyConsumption; } 
                else { EnergyConsumption = WarpEnergyConsumption; } 
                engineState = value;
            } 
        }
        private WarpStatus engineState = WarpStatus.Idle;
        public float ChargeFuelConsumption { get; protected set; } = 10;
        public float WarpFuelConsumption { get; protected set; } = 1;

        public float EnergyConsumption { get; protected set; }
        public float IdleEnergyConsumption { get; protected set; } = 0;
        public float ChargeEnergyConsumption { get; protected set; } = 0;
        public float WarpEnergyConsumption { get; protected set; } = 0;
        public float PowerPercent { get; set; }
    }
}
