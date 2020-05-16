using COSMOS.Character;
using COSMOS.Core.HelpfulStuff;
using COSMOS.Equipment;
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

        Dictionary<InventorySet, InventoryUI> usedInventoryObjects = new Dictionary<InventorySet, InventoryUI>();
        Dictionary<InventorySet, EquipmentUI> usedEquipmentObjects = new Dictionary<InventorySet, EquipmentUI>();

        private void Awake()
        {
            GameData.OnInventoryUpdate += fullRefreshInventories;
            GameData.OnEquipmentPartsUpdate += fullRefreshEquipments;
        }
        public void Refresh()
        {
            fullRefreshEquipments();
            fullRefreshInventories();
        }

        void fullRefreshInventories()
        {
            List<(InventorySet, InventoryUI)> inventories = new List<(InventorySet, InventoryUI)>();
            HashSet<InventoryUI> freeUI = new HashSet<InventoryUI>(usedInventoryObjects.Values);

            InventoryController[] availableInventoriesControllers = GameData.GetCharacterInventories();

            List<InventorySet> availableInventories = new List<InventorySet>();
            for (int i = 0; i < availableInventoriesControllers.Length; i++)
            {
                InventoryController controller = availableInventoriesControllers[i];
                if(controller != null && controller.InventorySets != null)
                {
                    availableInventories.AddRange(controller.InventorySets);
                }
            }


            if (availableInventories != null)
            {
                // search allready pair ui
                for (int i = 0; i < availableInventories.Count; i++)
                {
                    InventorySet item = availableInventories[i];
                    if (usedInventoryObjects.ContainsKey(item))
                    {
                        inventories.Add((item, usedInventoryObjects[item]));
                        freeUI.Remove(usedInventoryObjects[item]);
                    }
                    else
                    {
                        inventories.Add((item, null));
                    }
                }
                // create new need pair
                for (int i = 0; i < inventories.Count; i++)
                {
                    var pair = inventories[i];
                    if (pair.Item2 == null)
                    {
                        if (freeUI.Count > 0)
                        {
                            pair.Item2 = freeUI.First();
                            freeUI.Remove(pair.Item2);
                        }
                        else
                        {
                            InventoryUI ui = InventoryUI.Spawn();
                            ui.transform.SetParent(InventoriesContent.transform);
                            pair.Item2 = ui;
                        }
                    }
                    inventories[i] = pair;
                }
                usedInventoryObjects.Clear();
                // init pairs
                for (int i = 0; i < inventories.Count; i++)
                {
                    var pair = inventories[i];
                    if (pair.Item2 == null)
                    {
                        Log.Error("InventoryUI is null");
                        continue;
                    }
                    pair.Item2.Init(pair.Item1);
                    pair.Item2.transform.SetSiblingIndex(i);

                    usedInventoryObjects.Add(pair.Item1, pair.Item2);
                }
            }

            foreach (var child in freeUI)
            {
                Destroy(child.gameObject);
            }
        }
        void fullRefreshEquipments()
        {
            List<(InventorySet, EquipmentUI)> equipments = new List<(InventorySet, EquipmentUI)>();
            HashSet<EquipmentUI> freeUI = new HashSet<EquipmentUI>(usedEquipmentObjects.Values);
            InventoryController[] availableEquipmentControllers = GameData.GetCharacterEquipments();
            List<InventorySet> availableEquipments = new List<InventorySet>();

            for (int i = 0; i < availableEquipmentControllers.Length; i++)
            {
                InventoryController equipmentController = availableEquipmentControllers[i];
                if(equipmentController != null && equipmentController.InventorySets != null)
                {
                    availableEquipments.AddRange(equipmentController.InventorySets);
                }
            }

            if (availableEquipments != null)
            {
                // search allready pair ui
                for (int i = 0; i < availableEquipments.Count; i++)
                {
                    InventorySet item = availableEquipments[i];
                    if (usedEquipmentObjects.ContainsKey(item))
                    {
                        equipments.Add((item, usedEquipmentObjects[item]));
                        freeUI.Remove(usedEquipmentObjects[item]);
                    }
                    else
                    {
                        equipments.Add((item, null));
                    }
                }
                // create new need pair
                for (int i = 0; i < equipments.Count; i++)
                {
                    var pair = equipments[i];
                    if (pair.Item2 == null)
                    {
                        if (freeUI.Count > 0)
                        {
                            pair.Item2 = freeUI.First();
                            freeUI.Remove(pair.Item2);
                        }
                        else
                        {
                            EquipmentUI ui = EquipmentUI.Spawn();
                            ui.transform.SetParent(EquipmentContent.transform);
                            pair.Item2 = ui;
                        }
                    }
                    equipments[i] = pair;
                }
                usedEquipmentObjects.Clear();
                // init pairs
                for (int i = 0; i < equipments.Count; i++)
                {
                    var pair = equipments[i];
                    if (pair.Item2 == null)
                    {
                        Log.Error("InventoryUI is null");
                        continue;
                    }
                    pair.Item2.Init(pair.Item1);
                    pair.Item2.transform.SetSiblingIndex(i);

                    usedEquipmentObjects.Add(pair.Item1, pair.Item2);
                }
            }

            foreach (var child in freeUI)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
