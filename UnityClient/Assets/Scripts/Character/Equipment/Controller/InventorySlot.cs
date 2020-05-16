using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Equipment
{
    public class InventorySlot : IComparable<InventorySlot>
    {
        public delegate void OnAddItemDelegate(InventorySlot slot, Item item);
        public delegate void OnRemoveItemDelegate(InventorySlot slot, Item item);
        public delegate void OnReplaceItemDelegate(InventorySlot slot,
            Item newItem, Item oldItem);

        public event OnAddItemDelegate OnAddItem;
        public event OnRemoveItemDelegate OnRemoveItem;
        public event OnReplaceItemDelegate OnReplaceItem;

        public int Preority { get; protected set; }

        public bool SlotLock { get; protected set; }

        public bool SlotEmpty => CurrentItem == null;

        public Item CurrentItem { get; protected set; }

        protected readonly List<Func<Item, bool>> rules = new List<Func<Item, bool>>();

        public void AddCheckRule(Func<Item, bool> rule)
        {
            rules.Add(rule);
        }
        public void RemoveCheckRule(Func<Item, bool> rule)
        {
            rules.Remove(rule);
        }
        public virtual bool CheckRules(Item item)
        {
            for (int i = 0; i < rules.Count; i++)
            {
                if (!rules[i](item))
                {
                    return false;
                }
            }
            return true;
        }
        public bool CheckRules(Item item, ref int rulesPassed)
        {
            rulesPassed = 0;
            for (int i = 0; i < rules.Count; i++)
            {
                if (!rules[i](item))
                {
                    return false;
                }
                else
                {
                    rulesPassed++;
                }
            }
            return true;
        }

        public bool AddItem(Item item)
        {
            if (!SlotLock && item != null && CheckRules(item) && CurrentItem == null)
            {
                CurrentItem = item;
                OnAddItem?.Invoke(this, item);
                return true;
            }
            return false;
        }
        public bool RemoveItem(out Item oldItem)
        {
            oldItem = null;
            if (!SlotLock)
            {
                oldItem = CurrentItem;
                CurrentItem = null;
                OnRemoveItem?.Invoke(this, oldItem);
                return true;
            }
            return false;
        }
        public bool ReplaceItem(Item newItem, out Item oldItem)
        {
            oldItem = null;
            if (!SlotLock && newItem != null && CheckRules(newItem))
            {
                oldItem = CurrentItem;
                CurrentItem = newItem;
                OnReplaceItem?.Invoke(this, newItem, oldItem);
                return true;
            }
            return false;
        }


        public virtual int CompareTo(InventorySlot x)
        {
            if (x == null || x.SlotEmpty) return -1;
            return x.Preority.CompareTo(Preority);
        }
    }
    public class InventorySlot<T> : InventorySlot where T : Item
    {
        public new virtual T CurrentItem
        {
            get { return base.CurrentItem as T; }
            protected set { base.CurrentItem = value as T; }
        }
        public InventorySlot()
        {
            AddCheckRule(checkEquipmentType);
        }

        protected virtual bool checkEquipmentType(Item item)
        {
            return item is T;
        }
    }
}
