using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if(Hull != null)
            {
                if(Hull.WarpEngine != null)
                {
                    return true;
                }
            }
            return false;
        }
        public bool CanWarp(SolarSystem ss)
        {
            float distance = Vector2.Distance(SolarSystemManager.CurrentSystem.Proto.PosOnMap, ss.Proto.PosOnMap);
            float warpTime = Hull.WarpEngine.WarpSpeed * distance;
            float fuelCount = Hull.GetFuelCount("DarkEnergy");
            float allCost = warpTime * Hull.WarpEngine.WarpFuelConsumption + Hull.WarpEngine.ChargeTime * Hull.WarpEngine.ChargeFuelConsumption;
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
                WarpChargeStart?.Invoke(this);
                WarpChargeTimeLeft = Hull.WarpEngine.ChargeTime;
                WarpTarget = ss;
                CalculateTimeWarp = Hull.WarpEngine.WarpSpeed * Vector2.Distance(SolarSystemManager.CurrentSystem.Proto.PosOnMap, ss.Proto.PosOnMap);
                WarpCharging = true;
                Warping = false;
            }
        }
        void WarpingProcess()
        {
            if (WarpCharging)
            {
                WarpChargeTimeLeft -= Time.deltaTime;
                WarpChargePercentLeft = WarpChargeTimeLeft / Hull.WarpEngine.ChargeTime;
                if(Hull.UseFuel(Hull.WarpEngine.ChargeFuelConsumption * Time.deltaTime, "DarkEnergy") <= 0)
                {
                    WarpCharging = false;
                    Warping = false;
                    WarpChargeTimeLeft = 0;
                    WarpChargePercentLeft = 0;
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
                    WarpEnd?.Invoke(this);
                    WarpCharging = false;
                    Warping = false;

                    WarpChargeTimeLeft = 0;
                    WarpChargePercentLeft = 0;
                    WarpTimeLeft = 0;
                    WarpPercentLeft = 0;
                    WarpEnd?.Invoke(this);
                    return;
                }
                if (Hull.UseFuel(Hull.WarpEngine.WarpFuelConsumption * Time.deltaTime, "DarkEnergy") <= 0)
                {
                    WarpCharging = false;
                    Warping = false;
                    WarpChargeTimeLeft = 0;
                    WarpChargePercentLeft = 0;
                    WarpStop?.Invoke(this);
                    return;
                }
            }
        }

    }
}
