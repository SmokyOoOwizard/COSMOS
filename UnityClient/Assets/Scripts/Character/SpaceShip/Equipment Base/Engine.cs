using COSMOS.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.SpaceShip.Equipment
{
    public abstract class Engine : COSMOS.Equipment.Equipment
    {
        public float MaxForce { get; protected set; }
        public string FuelType { get; protected set; }
        public float FuelConsumption { get; protected set; }
        public float CalculateEngineForce(float power)
        {
            return MaxForce * power;
        }

        public Engine(float maxForce, string fuelType, float fuelConsumption)
        {
            MaxForce = maxForce;
            FuelType = fuelType;
            FuelConsumption = fuelConsumption;
        }
    }
    public class MainEngine : Engine
    {
        public MainEngine(float maxForce, string fuelType, float fuelConsumption) : base(maxForce, fuelType, fuelConsumption)
        {

        }
    }
    public class BrakingEngine : Engine
    {
        public BrakingEngine(float maxForce, string fuelType, float fuelConsumption) : base(maxForce, fuelType, fuelConsumption)
        {

        }
    }
    public class SideEngine : Engine
    {
        public SideEngine(float maxForce, string fuelType, float fuelConsumption) : base(maxForce, fuelType, fuelConsumption)
        {

        }
    }
    public class TurnEngine : Engine
    {
        public TurnEngine(float maxForce, string fuelType, float fuelConsumption) : base(maxForce, fuelType, fuelConsumption)
        {

        }
    }
}
