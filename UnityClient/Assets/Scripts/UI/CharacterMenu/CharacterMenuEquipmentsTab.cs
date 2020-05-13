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

        Dictionary<Inventory, InventoryUI> usedInventoryObjects = new Dictionary<Inventory, InventoryUI>();
        Dictionary<EquipmentSet, EquipmentUI> usedEquipmentObjects = new Dictionary<EquipmentSet, EquipmentUI>();

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
            List<(Inventory, InventoryUI)> inventories = new List<(Inventory, InventoryUI)>();
            HashSet<InventoryUI> freeUI = new HashSet<InventoryUI>(usedInventoryObjects.Values);
            List<Inventory> availableInventories = new List<Inventory>(GameData.GetCharacterInventories());

            if (availableInventories != null)
            {
                // search allready pair ui
                for (int i = 0; i < availableInventories.Count; i++)
                {
                    Inventory item = availableInventories[i];
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
                // creat new need pair
                for (int i = 0; i < inventories.Count; i++)
                {
                    (Inventory, InventoryUI) pair = inventories[i];
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
                    (Inventory, InventoryUI) pair = inventories[i];
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
            List<(EquipmentSet, EquipmentUI)> equipments = new List<(EquipmentSet, EquipmentUI)>();
            HashSet<EquipmentUI> freeUI = new HashSet<EquipmentUI>(usedEquipmentObjects.Values);
            List<EquipmentSet> availableEquipments = new List<EquipmentSet>(GameData.GetCharacterEquipments());

            if (availableEquipments != null)
            {
                // search allready pair ui
                for (int i = 0; i < availableEquipments.Count; i++)
                {
                    EquipmentSet item = availableEquipments[i];
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
                // creat new need pair
                for (int i = 0; i < equipments.Count; i++)
                {
                    (EquipmentSet, EquipmentUI) pair = equipments[i];
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
                    (EquipmentSet, EquipmentUI) pair = equipments[i];
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
