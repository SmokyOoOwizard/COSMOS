using COSMOS.Character;
using COSMOS.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.UI
{
    public class EquipmentUI : MonoBehaviour
    {
        public const string SLOT_PREFAB_ID = "";
        public EquipmentPart CurrentEquipmentPart { get; protected set; }

        Dictionary<SlotUI, EquipmentRule> RulesForSlots = new Dictionary<SlotUI, EquipmentRule>();

        [SerializeField]
        GameObject Content;

        bool checkRuleForSlot(SlotUI slot, ICanPlaceInSlot stuff)
        {
            if(stuff is Item)
            {
                if (RulesForSlots.ContainsKey(slot))
                {
                    EquipmentRule rule = RulesForSlots[slot];
                    if(rule.ItemFit(stuff as Item))
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
            CreateSlots();
        }
        public void CreateSlots()
        {
            SlotUI[] oldSlots = RulesForSlots.Keys.ToArray();
            RulesForSlots.Clear();
            for (int i = 0; i < oldSlots.Length; i++)
            {
                Destroy(oldSlots[i].gameObject);
            }
            foreach (Transform child in Content.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            GameObject slotPrefab = AssetsDatabase.LoadGameObject(SLOT_PREFAB_ID);

            foreach (var rule in CurrentEquipmentPart.Equipments)
            {
                GameObject slotObj = GameObject.Instantiate(slotPrefab);
                SlotUI slot = slotObj.GetComponent<SlotUI>();
                slotObj.transform.SetParent(Content.transform);

                slot.SetCustomAcceptFunc(checkRuleForSlot);
                slot.OnDropInSlot += onAcceptEquipmentInSlot;
                slot.EmptySlotImageID = rule.Key.SlotImageID;
                slot.SetStuff(rule.Value);
                slot.UpdateData();
                RulesForSlots.Add(slot, rule.Key);
            }
        } 
    }
}
