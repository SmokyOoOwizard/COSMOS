using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Equipment
{
    public class EquipmentSet
    {
        public delegate void OnAddEquipmentDelegate(EquipmentSet set, EquipmentSlot slot, Equipment equipment);
        public delegate void OnRemoveEquipmentDelegate(EquipmentSet set, EquipmentSlot slot, Equipment equipment);
        public delegate void OnReplaceEquipmentDelegate(EquipmentSet set, EquipmentSlot slot,
            Equipment newEquipment, Equipment oldEquipment);

        public event OnAddEquipmentDelegate OnAddEquipment;
        public event OnRemoveEquipmentDelegate OnRemoveEquipment;
        public event OnReplaceEquipmentDelegate OnReplaceEquipment;
        public event Action<EquipmentSet> OnChange;

        protected readonly List<Func<Equipment, bool>> localRules = new List<Func<Equipment, bool>>();

        public string LKeyName;

        public ReadOnlyCollection<EquipmentSlot> Slots => slots.AsReadOnly();
        protected List<EquipmentSlot> slots;

        public int FreeSlotsCount { get { return slots.Count - EquipmentCount; } }
        public int EquipmentCount { get; private set; }

        public float MaxVolume { get; set; }
        public float CurrentVolume { get; protected set; }

        public EquipmentSet()
        {
            AddLocalRule(checkVolume);
        }

        public void AddSlot(EquipmentSlot slot)
        {
            if (slot != null)
            {
                slots.Add(slot);
                slot.OnAddEquipment += OnSlotAddEquipmentHandler;
                slot.OnRemoveEquipment += OnSlotRemoveEquipmentHandler;
                slot.OnReplaceEquipment += OnSlotReplaceEquipmentHandler;

                slot.AddCheckRule(checkRules);

                slots.Sort();

                OnChange?.Invoke(this);
            }
        }
        public void RemoveSlot(EquipmentSlot slot)
        {
            if (slot != null)
            {
                slots.Remove(slot);
                slot.OnAddEquipment -= OnSlotAddEquipmentHandler;
                slot.OnRemoveEquipment -= OnSlotRemoveEquipmentHandler;
                slot.OnReplaceEquipment -= OnSlotReplaceEquipmentHandler;

                slot.RemoveCheckRule(checkRules);

                OnChange?.Invoke(this);
            }
        }

        public void AddLocalRule(Func<Equipment, bool> rule)
        {
            localRules.Add(rule);
        }
        public void RemoveLocalRule(Func<Equipment, bool> rule)
        {
            localRules.Remove(rule);
        }
        public virtual bool checkRules(Equipment equipment)
        {
            for (int i = 0; i < localRules.Count; i++)
            {
                if (!localRules[i](equipment))
                {
                    return false;
                }
            }
            return true;
        }

        protected void onChange()
        {
            OnChange?.Invoke(this);
        }
        protected virtual void OnSlotAddEquipmentHandler(EquipmentSlot slot, Equipment equipment)
        {
            EquipmentCount--;
            CurrentVolume += equipment.Volume;
            OnAddEquipment?.Invoke(this, slot, equipment);
        }
        protected virtual void OnSlotRemoveEquipmentHandler(EquipmentSlot slot, Equipment equipment)
        {
            EquipmentCount++;
            CurrentVolume -= equipment.Volume;
            OnRemoveEquipment?.Invoke(this, slot, equipment);
        }
        protected virtual void OnSlotReplaceEquipmentHandler(EquipmentSlot slot, Equipment newEquipment,
            Equipment oldEquipment)
        {
            CurrentVolume -= oldEquipment.Volume;
            CurrentVolume += newEquipment.Volume;
            OnReplaceEquipment?.Invoke(this, slot, newEquipment, oldEquipment);
        }

        public virtual bool checkVolume(Equipment equipment)
        {
            return equipment.Volume + CurrentVolume <= MaxVolume;
        }
    }
}
