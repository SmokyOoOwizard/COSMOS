﻿using System;
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
                        energyNeed += ec.EnergyConsumption * Time.deltaTime;
                    }
                }
                if(energyAdd >= energyNeed)
                {
                    foreach (var ec in Hull.EnergyСonsumptions)
                    {
                        ec.PowerPercent = 1;
                    }
                    energyAdd -= energyNeed;
                    if(Hull.EnergyTanks != null)
                    {
                        List<Equipment.EnergyTank> tanks = new List<Equipment.EnergyTank>(Hull.EnergyTanks);
                        int energyTanksCount = tanks.Count;
                        tanks.Sort((x, y) => { return y.FreeFuelVolume.CompareTo(x.FreeFuelVolume); });

                        float add = energyAdd / energyTanksCount;
                        for (int i = 0; i < energyTanksCount; i++)
                        {
                            var tank = tanks[i];
                            float excess = tank.PourIn(add);                                                                                                                                                        
                            if(excess > 0)
                            {
                                energyAdd -= excess;
                                add = energyAdd / (energyTanksCount - i);
                            }
                        }
                    }
                }
                else
                {
                    float diff = energyNeed - energyAdd;
                    if(Hull.EnergyTanks != null)
                    {
                        List<Equipment.EnergyTank> tanks = new List<Equipment.EnergyTank>(Hull.EnergyTanks);
                        int energyTanksCount = tanks.Count;
                        tanks.Sort((x, y) => { return x.FreeFuelVolume.CompareTo(y.FreeFuelVolume); });

                        float need = diff / energyTanksCount;
                        for (int i = 0; i < energyTanksCount; i++)
                        {
                            var tank = tanks[i];
                            float excess = tank.Drain(need);
                            if (excess > 0)
                            {
                                diff -= excess;
                                need = diff / (energyTanksCount - i);
                            }
                        }
                        if(diff > 0)
                        {
                            float power = (energyNeed - diff) / energyNeed;
                            foreach (var ec in Hull.EnergyСonsumptions)
                            {
                                ec.PowerPercent = power;
                            }
                        }
                    }
                    else
                    {
                        float power = energyAdd / energyNeed;
                        foreach (var ec in Hull.EnergyСonsumptions)
                        {
                            ec.PowerPercent = power;
                        }
                    }
                }
            }

        }

    }
}