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
        public EquipmentSet CurrentEquipmentSet { get; protected set; }

        private List<SlotUI> Slots = new List<SlotUI>();

        [SerializeField]
        TextMeshProUGUI header;
        [SerializeField]
        GameObject Content;

        public void Init(EquipmentSet equipment)
        {
            CurrentEquipmentSet = equipment;
            if(equipment != null)
            {
                header.SetText(TextFormat.GetLKeyAndFormat(equipment.LKeyName));
            }
            Refresh();
        }
        public void Refresh()
        {
            GameObject slotPrefab = AssetsDatabase.LoadGameObject(SLOT_PREFAB_ID);

            if (CurrentEquipmentSet != null)
            {
                var slots = CurrentEquipmentSet.Slots;

                if(slots.Count < Slots.Count)
                {
                    // destroy
                    for (int i = 0; i < slots.Count - Slots.Count; i++)
                    {
                        Destroy(Slots[i].gameObject);
                        Slots.RemoveAt(i);
                    }
                }
                else if(slots.Count > Slots.Count)
                {
                    // create
                    GameObject slotObj = GameObject.Instantiate(slotPrefab);
                    Slots.Add(slotObj.GetComponent<SlotUI>());
                    slotObj.transform.SetParent(Content.transform);
                    slotObj.transform.SetAsLastSibling();
                }

                for (int i = 0; i < slots.Count; i++)
                {
                    var equipmentSlot = slots[i];
                    var slotUI = Slots[i];

                    slotUI.ClearSlot();

                    slotUI.SetCustomAcceptFunc((slot, stuff) =>
                    {
                        if(stuff is Equipment.Equipment)
                        {
                            if(equipmentSlot.checkRules(stuff as Equipment.Equipment))
                            {
                                return true;
                            }
                        }
                        return false;
                    });
                    if(equipmentSlot.CurrentEquipment != null)
                    {
                        slotUI.SetStuff(equipmentSlot.CurrentEquipment);
                    }
                    slotUI.OnDropInSlot += (slot, newStuff, oldStuff) =>
                    {
                        if (oldStuff != null && oldStuff is Equipment.Equipment)
                        {
                            var oldEquipment = oldStuff as Equipment.Equipment;
                            if (equipmentSlot.RemoveEquipment(out oldEquipment))
                            {

                            }
                        }
                        if(newStuff != null && newStuff is Equipment.Equipment)
                        {
                            if(equipmentSlot.AddEquipment(newStuff as Equipment.Equipment))
                            {

                            }
                        }
                    };
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
