﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Player;
using COSMOS.Space;
using UnityEngine;

namespace COSMOS.SpaceShip
{
    public partial class SpaceShipController
    {
        public event Action<SpaceShipController> WarpChargeStart;
        public event Action<SpaceShipController> WarpChargeStop;
        public event Action<SpaceShipController> WarpChargeEnd;
        public event Action<SpaceShipController> WarpStart;
        public event Action<SpaceShipController> WarpStop;
        public event Action<SpaceShipController> WarpEnd;
        public float WarpChargeTimeLeft { get; protected set; }
        public float WarpChargePercentLeft { get; protected set; }
        public float WarpTimeLeft { get; protected set; }
        public float WarpPercentLeft { get; protected set; }
        public bool WarpCharging { get; protected set; }
        public bool Warping { get; protected set; }
        public SolarSystem WarpTarget { get; protected set; }
        public float CalculateTimeWarp { get; protected set; }
        public bool HasWarpEngine()
        {
            if(SpaceShip != null)
            {
                if(SpaceShip.WarpEngine != null)
                {
                    return true;
                }
            }
            return false;
        }
        public bool CanWarp(SolarSystem ss)
        {
            float distance = Vector2.Distance(SolarSystemManager.CurrentSystem.PosOnMap, ss.PosOnMap);
            float warpTime = SpaceShip.WarpEngine.WarpSpeed * distance;
            float fuelCount = SpaceShip.GetFuelCount("DarkEnergy");
            float allCost = warpTime * SpaceShip.WarpEngine.WarpFuelConsumption + SpaceShip.WarpEngine.ChargeTime * SpaceShip.WarpEngine.ChargeFuelConsumption;
            if (fuelCount >= allCost)
            {
                return true;
            }
            return false;
        }
        public void StartWarp(SolarSystem ss)
        {
            if (HasWarpEngine() && CanWarp(ss))
            {
                WarpChargeTimeLeft = SpaceShip.WarpEngine.ChargeTime;
                WarpTarget = ss;
                WarpTimeLeft = CalculateTimeWarp = SpaceShip.WarpEngine.WarpSpeed * 
                    Vector2.Distance(SolarSystemManager.CurrentSystem.PosOnMap, ss.PosOnMap);
                WarpCharging = true;
                SpaceShip.WarpEngine.EngineState = WarpStatus.Charge;
                Warping = false;
                WarpChargeStart?.Invoke(this);
            }
        }
        void WarpingProcess()
        {
            if (WarpCharging)
            {
                WarpChargeTimeLeft -= Time.deltaTime;
                WarpChargePercentLeft = WarpChargeTimeLeft / SpaceShip.WarpEngine.ChargeTime;
                if(SpaceShip.UseFuel(SpaceShip.WarpEngine.ChargeFuelConsumption * Time.deltaTime, "DarkEnergy") != 0)
                {
                    WarpCharging = false;
                    Warping = false;
                    WarpChargeTimeLeft = 0;
                    WarpChargePercentLeft = 0;
                    SpaceShip.WarpEngine.EngineState = WarpStatus.Idle;
                    WarpChargeStop?.Invoke(this);
                    return;
                }
                if(WarpChargeTimeLeft <= 0)
                {
                    WarpChargeEnd?.Invoke(this);
                    WarpCharging = false;
                    Warping = true;

                    WarpChargeTimeLeft = 0;
                    WarpChargePercentLeft = 0;
                    SpaceShip.WarpEngine.EngineState = WarpStatus.Warp;
                    WarpStart?.Invoke(this);
                    return;
                }
            }
            if (Warping)
            {
                WarpTimeLeft -= Time.deltaTime;
                WarpPercentLeft = WarpTimeLeft / CalculateTimeWarp;
                if (WarpTimeLeft <= 0)
                {
                    WarpCharging = false;
                    Warping = false;

                    WarpChargeTimeLeft = 0;
                    WarpChargePercentLeft = 0;
                    WarpTimeLeft = 0;
                    WarpPercentLeft = 0;
                    SpaceShip.WarpEngine.EngineState = WarpStatus.Idle;
                    WarpEnd?.Invoke(this);
                    return;
                }
                if (SpaceShip.UseFuel(SpaceShip.WarpEngine.WarpFuelConsumption * Time.deltaTime, "DarkEnergy") != 0)
                {
                    WarpCharging = false;
                    Warping = false;
                    WarpChargeTimeLeft = 0;
                    WarpChargePercentLeft = 0;
                    SpaceShip.WarpEngine.EngineState = WarpStatus.Idle;
                    WarpStop?.Invoke(this);
                    return;
                }
            }
        }

    }
}
