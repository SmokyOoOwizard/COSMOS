using COSMOS.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using COSMOS.Database;

namespace COSMOS.UI
{
    public class InventoryUI : MonoBehaviour
    {
        public const string PREFAB_ID = @"Prefabs\UI\Inventory\InventorySlotsUI";
        public const string SLOT_PREFAB_ID = @"Prefabs\UI\Slot";
        public InventorySet CurrentInventory { get; protected set; }
        Dictionary<SlotUI, Item> itemsBySlots = new Dictionary<SlotUI, Item>();

        [SerializeField]
        TextMeshProUGUI header;
        [SerializeField]
        GameObject Content;

        public void Init(InventorySet inventory)
        {
            CurrentInventory = inventory;
            if (inventory != null)
            {
                header.SetText(TextFormat.GetLKeyAndFormat(inventory.LKeyName));
            }
            Refresh();
        }
        public void Refresh()
        {
            GameObject slotPrefab = AssetsDatabase.LoadGameObject(SLOT_PREFAB_ID);

            HashSet<SlotUI> freeSlots = new HashSet<SlotUI>(itemsBySlots.Keys);
            itemsBySlots.Clear();
            if (CurrentInventory != null)
            {
                Item[] items = CurrentInventory.Items.ToArray();

                for (int i = 0; i < items.Length; i++)
                {
                    Item item = items[i];
                    SlotUI slot = null;
                    if (freeSlots.Count > 0)
                    {
                        slot = freeSlots.First();
                        freeSlots.Remove(slot);
                    }
                    else
                    {
                        GameObject slotObj = GameObject.Instantiate(slotPrefab);
                        slot = slotObj.GetComponent<SlotUI>();
                        slotObj.transform.SetParent(Content.transform);

                        //slot.SetCustomAcceptFunc(checkRuleForSlot);
                        //slot.OnDropInSlot += onAcceptEquipmentInSlot;
                    }
                    itemsBySlots.Add(slot, item);
                    slot.SetStuff(item);
                    slot.UpdateData();
                    slot.transform.SetSiblingIndex(i);
                }
            }
            if(freeSlots.Count > 0)
            {
                foreach (var child in freeSlots)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            for (int i = 0; i < CurrentInventory.FreeSlotsCount; i++)
            {
                createEmptySlot();
            }
        }
        void createEmptySlot()
        {
            GameObject slotPrefab = AssetsDatabase.LoadGameObject(SLOT_PREFAB_ID);
            GameObject slotObj = GameObject.Instantiate(slotPrefab);
            SlotUI slot = slotObj.GetComponent<SlotUI>();
            slotObj.transform.SetParent(Content.transform);

            //slot.SetCustomAcceptFunc(checkRuleForSlot);
            //slot.OnDropInSlot += onAcceptEquipmentInSlot;
            slot.UpdateData();

            itemsBySlots.Add(slot, null);
        }
        public static InventoryUI Spawn()
        {
            GameObject prefab = AssetsDatabase.LoadGameObject(PREFAB_ID);
            if (prefab == null)
            {
                Log.Error("prefab for inventory not found. path:" + PREFAB_ID);
                return null;
            }
            GameObject go = Instantiate(prefab);
            return go.GetComponent<InventoryUI>();
        }
    }
}
