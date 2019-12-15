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
            if(Player.PlayerController.instance.IC is SpaceShip.SpaceShipController && Player.PlayerController.instance.IC.Exist())
            {
                SpaceShip.SpaceShipController ssc = Player.PlayerController.instance.IC as SpaceShip.SpaceShipController;
                HP.fillAmount = 1;
                Fuel.fillAmount = ssc.Hull.GetFuelCount("fuel") / ssc.Hull.GetMaxFuelCount("fuel");
                DarkEnergy.fillAmount = ssc.Hull.GetFuelCount("DarkEnergy") / ssc.Hull.GetMaxFuelCount("DarkEnergy");
                //Energy.fillAmount = ssc.Hull.get

            }
        }
    }
}
