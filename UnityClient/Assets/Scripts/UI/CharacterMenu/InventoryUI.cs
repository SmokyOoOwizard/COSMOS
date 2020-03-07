using COSMOS.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace COSMOS.UI
{
    public class InventoryUI : MonoBehaviour
    {
        public const string PREFAB_ID = @"Prefabs\UI\Inventory\InventorySlotsUI";
        public const string SLOT_PREFAB_ID = @"Prefabs\UI\Slot";
        public Inventory CurrentInventory { get; protected set; }
        Dictionary<SlotUI, Item> itemsBySlots = new Dictionary<SlotUI, Item>();
        int freeSlotsCount = 0;
        [SerializeField]
        GameObject Content;

        bool checkRuleForSlot(SlotUI slot, ICanPlaceInSlot stuff)
        {
            if (stuff is Item)
            {
                Item item = stuff as Item;
                float newWeight = item.Weight;
                float newVolume = item.Volume;
                if (slot.CurrentStuff != null && slot.CurrentStuff is Item)
                {
                    Item currentItem = slot.CurrentStuff as Item;
                    newWeight -= currentItem.Weight;
                    newVolume -= currentItem.Volume;
                }
                if (newWeight < CurrentInventory.FreeWeight && newVolume < CurrentInventory.FreeVolume)
                {
                    return true;
                }
            }
            return false;
        }
        void onAcceptEquipmentInSlot(SlotUI slot, ICanPlaceInSlot newItem, ICanPlaceInSlot oldItem)
        {
            Item item = newItem as Item;
            if (oldItem != null)
            {
                Item old = oldItem as Item;
                if (item != null)
                {
                    if (itemsBySlots.ContainsValue(item))
                    {
                        CurrentInventory.Swap(item, old);
                    }
                    else
                    {
                        if (!CurrentInventory.Replace(old, item))
                        {
                            Log.Error("Old item in slot not contains in current inventory");
                        }
                    }
                }
                else
                {
                    if (!CurrentInventory.Remove(old))
                    {
                        Log.Error("Old item in slot not contains in current inventory");
                    }
                    freeSlotsCount++;
                }
            }
            else
            {
                if (item != null)
                {
                    CurrentInventory.AddItem(item);
                    freeSlotsCount--;
                    if (freeSlotsCount < 1)
                    {
                        createEmptySlot();
                        freeSlotsCount++;
                    }
                }
            }
        }
        public void Init(Inventory inventory)
        {
            CurrentInventory = inventory;
            Refresh();
        }
        public void Refresh()
        {
            GameObject slotPrefab = AssetsDatabase.LoadGameObject(SLOT_PREFAB_ID);

            HashSet<SlotUI> freeSlots = new HashSet<SlotUI>(itemsBySlots.Keys);
            itemsBySlots.Clear();
            if (CurrentInventory != null)
            {
                Item[] items = CurrentInventory.GetItems();

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

                        slot.SetCustomAcceptFunc(checkRuleForSlot);
                        slot.OnDropInSlot += onAcceptEquipmentInSlot;
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
            freeSlotsCount = 0;
            if (CurrentInventory != null && CurrentInventory.FreeVolume > 0 && CurrentInventory.FreeWeight > 0)
            {
                createEmptySlot();
                freeSlotsCount++;
            }
        }
        void createEmptySlot()
        {
            GameObject slotPrefab = AssetsDatabase.LoadGameObject(SLOT_PREFAB_ID);
            GameObject slotObj = GameObject.Instantiate(slotPrefab);
            SlotUI slot = slotObj.GetComponent<SlotUI>();
            slotObj.transform.SetParent(Content.transform);

            slot.SetCustomAcceptFunc(checkRuleForSlot);
            slot.OnDropInSlot += onAcceptEquipmentInSlot;
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
