using COSMOS.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace COSMOS.SpaceShip
{
    public class SpaceShipHull
    {
        public float HullWeight { get; protected set; }
        public float TotalWeight { get; protected set; }
        public float MaxWeight { get; protected set; }

        public float FreeVolume { get; protected set; }
        public float MaxVolume { get; protected set; }

        public float MaxSpeed { get; protected set; } = 20;

        #region Equipments
        public MainEngine MainEngine { get; protected set; }
        public BrakingEngine BrakingEngine { get; protected set; }
        public TurnEngine TurnEngines { get; protected set; }
        public SideEngine SideEngines { get; protected set; }

        public List<Tank> Tanks { get; protected set; }
        #endregion

        public Engine ReplaceEngine(Engine engine)
        {
            if(engine is MainEngine)
            {
                Engine tmp = MainEngine;
                MainEngine = engine as MainEngine;
                return tmp;
            }
            else if(engine is BrakingEngine)
            {
                Engine tmp = BrakingEngine;
                BrakingEngine = engine as BrakingEngine;
                return tmp;
            }
            else if(engine is TurnEngine)
            {
                Engine tmp = TurnEngines;
                TurnEngines = engine as TurnEngine;
                return tmp;
            }
            else if(engine is SideEngine)
            {
                Engine tmp = SideEngines;
                SideEngines = engine as SideEngine;
                return tmp;
            }
            return null;
        }
        public void AddTank(Tank tank)
        {
            if (Tanks == null)
            {
                Tanks = new List<Tank>();
            }
            Tanks.Add(tank);
        }
        public float UseFuel(float amount, string fuelType)
        {
            Tank tank = Tanks.Find((x) => { return x.FuelType == fuelType; });
            if(tank != null)
            {
                return tank.Drain(amount);
            }
            return 0;
        }
        #region Engines
        public float CalculateForwardEngineForce(float power)
        {
            if(MainEngine != null)
            {
                return MainEngine.CalculateEngineForce(power);
            }
            return 0;
        }
        public float UseForwardEngine(float power)
        {
            if (MainEngine != null)
            {
                float fuelConsumption = power * MainEngine.FuelConsumption;
                float fuelUsedUp = UseFuel(fuelConsumption, MainEngine.FuelType);
                if (fuelUsedUp > 0)
                {
                    return MainEngine.CalculateEngineForce(power * (fuelUsedUp / fuelConsumption));
                }
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
                float fuelConsumption = power * BrakingEngine.FuelConsumption;
                float fuelUsedUp = UseFuel(fuelConsumption, BrakingEngine.FuelType);
                if (fuelUsedUp > 0)
                {
                    return BrakingEngine.CalculateEngineForce(power * (fuelUsedUp / fuelConsumption));
                }
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
                float fuelConsumption = power * TurnEngines.FuelConsumption;
                float fuelUsedUp = UseFuel(fuelConsumption, TurnEngines.FuelType);
                if (fuelUsedUp > 0)
                {
                    return TurnEngines.CalculateEngineForce(power * (fuelUsedUp / fuelConsumption));
                }
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
                float fuelConsumption = power * SideEngines.FuelConsumption;
                float fuelUsedUp = UseFuel(fuelConsumption, SideEngines.FuelType);
                if (fuelUsedUp > 0)
                {
                    return SideEngines.CalculateEngineForce(power * (fuelUsedUp / fuelConsumption));
                }
            }
            return 0;
        }
        #endregion
    }
}
