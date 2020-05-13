using COSMOS.Character;
using COSMOS.Core;
using COSMOS.Equipment;
using COSMOS.Player;
using COSMOS.Space;
using COSMOS.SpaceShip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS
{
    public static class GameData
    {
        public static bool WorldPause { get; private set; }
        public static DateTime CurrentDate { get; set; }

        public static readonly Notifications PlayerNotifications = new Notifications();

        public static long Money;
        public static WarpState CurrentWarp { get; private set; }

        public static IControllable CurrentControllableObject
        {
            get
            {
                return currentControllableObject;
            }
            set
            {
                if(currentControllableObject != null && currentControllableObject is SpaceShipController)
                {
                    UnsubControllableSpaceShip(currentControllableObject as SpaceShipController);
                }
                if(value != null && value is SpaceShipController)
                {
                    SubControllableSpaceShip(value as SpaceShipController);
                }
                currentControllableObject = value;
            }
        }
        static IControllable currentControllableObject;

        public static COSMOS.Character.Character PlayerCharacter { get; set; }
        public static event Action OnEquipmentPartsUpdate;
        public static event Action OnInventoryUpdate;


        public static Inventory[] GetCharacterInventories()
        {
            return new Inventory[] { new Inventory() { MaxVolume = 10, MaxWeight = 10 } };
        }

        public static EquipmentSet[] GetCharacterEquipments()
        {
            return new EquipmentSet[0];
        }

        static void OnSpaceShipWarp(SpaceShip.SpaceShipController spaceShip)
        {
            WarpStatus warpStatus = spaceShip.Hull.WarpEngine.EngineState;
            if(warpStatus == WarpStatus.Charge || warpStatus == WarpStatus.Warp)
            {
                CurrentWarp = new WarpState();
                CurrentWarp.Status = warpStatus;
                CurrentWarp.To = spaceShip.WarpTarget;
                CurrentWarp.From = SolarSystemManager.CurrentSystem;
                CurrentWarp.ChargeTimeLeft = CurrentDate.AddSeconds(spaceShip.WarpChargeTimeLeft);
                CurrentWarp.WarpTimeLeft = CurrentWarp.ChargeTimeLeft.AddSeconds(spaceShip.WarpTimeLeft);
            }
            else
            {
                CurrentWarp = null;
            }
        }
        static void UnsubControllableSpaceShip(SpaceShip.SpaceShipController spaceShip)
        {
            if(spaceShip != null)
            {
                spaceShip.WarpChargeStart -= OnSpaceShipWarp;
                spaceShip.WarpChargeStop -= OnSpaceShipWarp;
                spaceShip.WarpChargeEnd -= OnSpaceShipWarp;
                spaceShip.WarpStart -= OnSpaceShipWarp;
                spaceShip.WarpStop -= OnSpaceShipWarp;
                spaceShip.WarpEnd -= OnSpaceShipWarp;

            }
        }
        static void SubControllableSpaceShip(SpaceShip.SpaceShipController spaceShip)
        {
            if (spaceShip != null)
            {
                spaceShip.WarpChargeStart += OnSpaceShipWarp;
                spaceShip.WarpChargeStop += OnSpaceShipWarp;
                spaceShip.WarpChargeEnd += OnSpaceShipWarp;
                spaceShip.WarpStart += OnSpaceShipWarp;
                spaceShip.WarpStop += OnSpaceShipWarp;
                spaceShip.WarpEnd += OnSpaceShipWarp;
                spaceShip.WarpEnd += (ship) => { SolarSystemManager.LoadSystem(ship.WarpTarget); };

            }
        }
    }
}
