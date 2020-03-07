using COSMOS.Character;
using COSMOS.Database;
using COSMOS.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace COSMOS.UI
{
    public class EquipmentUI : MonoBehaviour
    {
        public const string PREFAB_ID = @"Prefabs\UI\Inventory\EquipmentPartSlotsUI";
        public const string SLOT_PREFAB_ID = @"Prefabs\UI\Slot";
        public EquipmentPart CurrentEquipmentPart { get; protected set; }

        Dictionary<SlotUI, EquipmentRule> RulesForSlots = new Dictionary<SlotUI, EquipmentRule>();

        [SerializeField]
        TextMeshProUGUI header;
        [SerializeField]
        GameObject Content;

        bool checkRuleForSlot(SlotUI slot, ICanPlaceInSlot stuff)
        {
            if (stuff is Item)
            {
                if (RulesForSlots.ContainsKey(slot))
                {
                    EquipmentRule rule = RulesForSlots[slot];
                    if (rule.ItemFit(stuff as Item))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        void onAcceptEquipmentInSlot(SlotUI slot, ICanPlaceInSlot item, ICanPlaceInSlot oldItem)
        {
            if (RulesForSlots.ContainsKey(slot) && item is Item)
            {
                CurrentEquipmentPart.SetItem(RulesForSlots[slot], item as Item);
            }
        }
        public void Init(EquipmentPart equipment)
        {
            CurrentEquipmentPart = equipment;
            if(equipment != null)
            {
                header.SetText(TextFormat.GetLKeyAndFormat(equipment.LKeyName));
            }
            Refresh();
        }
        public void Refresh()
        {
            GameObject slotPrefab = AssetsDatabase.LoadGameObject(SLOT_PREFAB_ID);

            HashSet<SlotUI> freeSlots = new HashSet<SlotUI>(RulesForSlots.Keys);
            RulesForSlots.Clear();
            if (CurrentEquipmentPart != null)
            {
                (EquipmentRule, Item)[] items = CurrentEquipmentPart.Equipments.Select((x) => (x.Key, x.Value)).ToArray();

                for (int i = 0; i < items.Length; i++)
                {
                    (EquipmentRule, Item) item = items[i];
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
                    RulesForSlots.Add(slot, item.Item1);
                    if (item.Item2 != null)
                    {
                        slot.SetStuff(item.Item2);
                    }
                    slot.EmptySlotImageID = item.Item1.SlotImageID;
                    slot.UpdateData();
                    slot.transform.SetSiblingIndex(i);
                }
            }
            if (freeSlots.Count > 0)
            {
                foreach (var child in freeSlots)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }
        public static EquipmentUI Spawn()
        {
            GameObject prefab = AssetsDatabase.LoadGameObject(PREFAB_ID);
            if (prefab == null)
            {
                Log.Error("prefab for equipment not found. path:" + PREFAB_ID);
                return null;
            }
            GameObject go = Instantiate(prefab);
            return go.GetComponent<EquipmentUI>();
        }
    }
}
