using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Equipment;

namespace COSMOS.SpaceShip.Equipment
{
    public class WarpEngine : ShipEquipment, INeedEnergy
    {
        public enum State
        {
            Idle,
            Charge,
            Warp
        }
        public float WarpSpeed { get; protected set; } = 1;
        public float ChargeTime { get; protected set; } = 5;
        public State EngineState 
        { 
            get { return engineState; } 
            set { 
                if (value == State.Idle) { EnergyConsumption = IdleEnergyConsumption; } 
                else if (value == State.Charge) { EnergyConsumption = ChargeEnergyConsumption; } 
                else { EnergyConsumption = WarpEnergyConsumption; } 
                engineState = value;
            } 
        }
        private State engineState = State.Idle;
        public float ChargeFuelConsumption { get; protected set; } = 10;
        public float WarpFuelConsumption { get; protected set; } = 1;

        public float EnergyConsumption { get; protected set; }
        public float IdleEnergyConsumption { get; protected set; } = 0;
        public float ChargeEnergyConsumption { get; protected set; } = 0;
        public float WarpEnergyConsumption { get; protected set; } = 0;
        public float PowerPercent { get; set; }
    }
}
