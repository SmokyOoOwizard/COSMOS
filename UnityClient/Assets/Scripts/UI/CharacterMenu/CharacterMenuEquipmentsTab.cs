using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.UI
{
    public class CharacterMenuEquipmentsTab : MonoBehaviour
    {
        [SerializeField]
        GameObject EquipmentContent;
        [SerializeField]
        GameObject InventoriesContent;

        public void Init()
        {
            refreshEquipments();
            refreshInventories();
        }
        void refreshEquipments()
        {
            foreach (Transform child in EquipmentContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            if(GameData.PlayerCharacter == null)
            {
                return;
            }
            
            var equipmentParts = GameData.PlayerCharacter.GetEquipmentsParts();
            foreach (var part in equipmentParts)
            {
                EquipmentUI ui = EquipmentUI.Spawn();
                ui.transform.SetParent(EquipmentContent.transform);
                ui.Init(part);
            }
        }
        void refreshInventories()
        {
            foreach (Transform child in InventoriesContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            if (GameData.PlayerCharacter == null)
            {
                return;
            }

            var inventories = GameData.PlayerCharacter.GetInventories();
            foreach (var part in inventories)
            {
                InventoryUI ui = InventoryUI.Spawn();
                ui.transform.SetParent(InventoriesContent.transform);
                ui.Init(part);
            }
        }
    }
}
