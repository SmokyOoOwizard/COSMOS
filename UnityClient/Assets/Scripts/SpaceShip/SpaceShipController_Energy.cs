using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.SpaceShip
{
    public partial class SpaceShipController
    {
        void EnergyUpdate()
        {
            if (Hull != null)
            {
                float energyAdd = 0;
                float energyNeed = 0;
                if (Hull.Generators != null)
                {
                    foreach (var generator in Hull.Generators)
                    {
                        float amount = Hull.UseFuel(generator.FuelConsumption, generator.FuelType);
                        float percent = amount / generator.FuelConsumption;
                        energyAdd += percent * generator.GeneratePerTick * Time.deltaTime;
                    }
                }

                if(Hull.EnergyСonsumptions != null)
                {
                    foreach (var ec in Hull.EnergyСonsumptions)
                    {
                        energyNeed += ec.EnergyConsumption;
                    }
                }
                if(energyAdd > energyNeed)
                {
                    foreach (var ec in Hull.EnergyСonsumptions)
                    {
                        ec.PowerPercent = 1;
                    }
                    energyAdd -= energyNeed;
                    if(Hull.EnergyTanks != null)
                    {
                        int energyTanksCount = Hull.EnergyTanks.Count;
                        float add = energyAdd / Hull.EnergyTanks.Count;
                        for (int i = 0; i < energyTanksCount; i++)
                        {
                            var tank = Hull.EnergyTanks[i];
                            float excess = tank.PourIn(add);                                                                                                                                                        
                            if(excess > 0)
                            {
                                energyAdd -= add;
                                add = energyAdd / (energyTanksCount - i);
                            }
                        }
                    }
                }
            }

        }

    }
}
