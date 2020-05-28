using COSMOS.Equipment;
using COSMOS.SpaceShip.Equipment;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.SpaceShip
{
    public partial class SpaceShip
    {
        public List<COSMOS.Equipment.Equipment> Equipments { get; protected set; }
        public MainEngine MainEngine { get; protected set; }
        public BrakingEngine BrakingEngine { get; protected set; }
        public TurnEngine TurnEngines { get; protected set; }
        public SideEngine SideEngines { get; protected set; }
        public WarpEngine WarpEngine { get; protected set; }
        public List<Tank> Tanks { get; protected set; }
        public List<Generator> Generators { get; protected set; }
        public List<EnergyTank> EnergyTanks { get; protected set; }
        public List<INeedEnergy> EnergyСonsumptions { get; protected set; }

        public List<Weapon> Weapons { get; protected set; } = new List<Weapon>();

        public void InitEquipments()
        {
            Equipments = new List<COSMOS.Equipment.Equipment>();
            Tanks = new List<Tank>();
            EnergyTanks = new List<EnergyTank>();
            Generators = new List<Generator>();
            EnergyСonsumptions = new List<INeedEnergy>();
        }

        public Engine ReplaceEngine(Engine engine)
        {
            if (engine is MainEngine)
            {
                Engine tmp = MainEngine;
                MainEngine = engine as MainEngine;
                return tmp;
            }
            else if (engine is BrakingEngine)
            {
                Engine tmp = BrakingEngine;
                BrakingEngine = engine as BrakingEngine;
                return tmp;
            }
            else if (engine is TurnEngine)
            {
                Engine tmp = TurnEngines;
                TurnEngines = engine as TurnEngine;
                return tmp;
            }
            else if (engine is SideEngine)
            {
                Engine tmp = SideEngines;
                SideEngines = engine as SideEngine;
                return tmp;
            }
            return null;
        }
        public void AddEquipment(COSMOS.Equipment.Equipment equipment)
        {
            switch (equipment)
            {
                case Tank tank:
                    AddTank(tank);
                    break;
                case Generator g:
                    AddGenerator(g);
                    break;
                case WarpEngine g:
                    WarpEngine = g;
                    break;
                case Weapon weapon:
                    Weapons.Add(weapon);
                    break;
            }

            if (equipment is INeedEnergy)
            {
                EnergyСonsumptions.Add(equipment as INeedEnergy);
            }
        }
        public void RemoveEquipment(COSMOS.Equipment.Equipment equipment)
        {
            switch (equipment)
            {
                case Tank tank:
                    RemoveTank(tank);
                    break;
                case Generator g:
                    RemoveGenerator(g);
                    break;
            }

            if (equipment is INeedEnergy)
            {
                EnergyСonsumptions.Remove(equipment as INeedEnergy);
            }
        }
        void AddGenerator(Generator generator)
        {
            Generators.Add(generator);
        }
        void RemoveGenerator(Generator generator)
        {
            Generators.Remove(generator);
        }
        void AddTank(Tank tank)
        {
            Tanks.Add(tank);
        }
        void RemoveTank(Tank tank)
        {
            Tanks.Remove(tank);
        }
        public float GetFuelCount(string fuelType)
        {
            float count = 0;
            Tanks.ForEach((x) => { if (x != null) if (x.FuelType == fuelType) count += x.FuelVolume; });
            return count;
        }
        public float GetMaxFuelCount(string fuelType)
        {
            float count = 0;
            Tanks.ForEach((x) => { if (x != null) if (x.FuelType == fuelType) count += x.MaxFuelVolume; });
            return count;
        }
        public float UseFuel(float amount, string fuelType)
        {
            Tank tank = Tanks.Find((x) => { return x.FuelType == fuelType; });
            if (tank != null)
            {
                return tank.Drain(amount);
            }
            return amount;
        }
    }
}
