using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace COSMOS.UI 
{
    public class ShipUI : MonoBehaviour
    {
        public Image HP;
        public Image DarkEnergy;
        public Image Shield;
        public Image Energy;
        public Image Fuel;

        private void Update()
        {
            if(GameData.CurrentControllableObject is SpaceShip.SpaceShipController && GameData.CurrentControllableObject.Exist())
            {
                SpaceShip.SpaceShipController ssc = GameData.CurrentControllableObject as SpaceShip.SpaceShipController;
                HP.fillAmount = 1;
                Fuel.fillAmount = ssc.SpaceShip.GetFuelCount("fuel") / ssc.SpaceShip.GetMaxFuelCount("fuel");
                DarkEnergy.fillAmount = ssc.SpaceShip.GetFuelCount("DarkEnergy") / ssc.SpaceShip.GetMaxFuelCount("DarkEnergy");
                //Energy.fillAmount = ssc.Hull.get

            }
        }
    }
}
