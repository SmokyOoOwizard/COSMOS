using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Equipment
{
    public class EquipmentSlot : IComparable<EquipmentSlot>
    {
        public delegate void OnAddEquipmentDelegate(EquipmentSlot slot, Equipment equipment);
        public delegate void OnRemoveEquipmentDelegate(EquipmentSlot slot, Equipment equipment);
        public delegate void OnReplaceEquipmentDelegate(EquipmentSlot slot,
            Equipment newEquipment, Equipment oldEquipment);

        public event OnAddEquipmentDelegate OnAddEquipment;
        public event OnRemoveEquipmentDelegate OnRemoveEquipment;
        public event OnReplaceEquipmentDelegate OnReplaceEquipment;

        public int Preority { get; protected set; }

        public bool SlotLock { get; protected set; }

        public bool SlotFree => CurrentEquipment == null;

        public Equipment CurrentEquipment { get; protected set; }

        protected readonly List<Func<Equipment, bool>> rules = new List<Func<Equipment, bool>>();

        public void AddCheckRule(Func<Equipment, bool> rule)
        {
            rules.Add(rule);
        }
        public void RemoveCheckRule(Func<Equipment, bool> rule)
        {
            rules.Remove(rule);
        }
        public virtual bool checkRules(Equipment equipment)
        {
            for (int i = 0; i < rules.Count; i++)
            {
                if (!rules[i](equipment))
                {
                    return false;
                }
            }
            return true;
        }

        public bool AddEquipment(Equipment equipment)
        {
            if (!SlotLock && equipment != null && checkRules(equipment) && CurrentEquipment == null)
            {
                CurrentEquipment = equipment;
                OnAddEquipment?.Invoke(this, equipment);
                return true;
            }
            return false;
        }
        public bool RemoveEquipment(out Equipment oldEquipment)
        {
            oldEquipment = null;
            if (!SlotLock)
            {
                oldEquipment = CurrentEquipment;
                CurrentEquipment = null;
                OnRemoveEquipment?.Invoke(this, oldEquipment);
                return true;
            }
            return false;
        }
        public bool ReplaceEquipment(Equipment newEquipment, out Equipment oldEquipment)
        {
            oldEquipment = null;
            if (!SlotLock && newEquipment != null && checkRules(newEquipment))
            {
                oldEquipment = CurrentEquipment;
                CurrentEquipment = newEquipment;
                OnReplaceEquipment?.Invoke(this, newEquipment, oldEquipment);
                return true;
            }
            return false;
        }

        public virtual int CompareTo(EquipmentSlot x)
        {
            if (x == null || x.SlotFree) return -1;
            return x.Preority.CompareTo(Preority);
        }
    }
    public class EquipmentSlot<T> : EquipmentSlot where T : Equipment
    {
        public new virtual T CurrentEquipment
        {
            get { return base.CurrentEquipment as T; }
            protected set { base.CurrentEquipment = value as T; }
        }
        public EquipmentSlot()
        {
            AddCheckRule(checkEquipmentType);
        }

        protected virtual bool checkEquipmentType(Equipment equipment)
        {
            return equipment is T;
        }
    }
}
