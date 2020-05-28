using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Equipment
{
    public class InventorySet
    {
        public delegate void OnAddItemDelegate(InventorySet set, InventorySlot slot, Item item);
        public delegate void OnRemoveItemDelegate(InventorySet set, InventorySlot slot, Item item);
        public delegate void OnReplaceItemDelegate(InventorySet set, InventorySlot slot,
            Item newItem, Item oldItem);

        public event OnAddItemDelegate OnAddItem;
        public event OnRemoveItemDelegate OnRemoveItem;
        public event OnReplaceItemDelegate OnReplaceItem;
        public event Action<InventorySet> OnChange;

        protected readonly List<Func<Item, bool>> localRules = new List<Func<Item, bool>>();

        public string LKeyName;

        public ReadOnlyCollection<InventorySlot> Slots => slots.AsReadOnly();
        protected List<InventorySlot> slots = new List<InventorySlot>();
        
        public ReadOnlyCollection<InventorySlot> FreeSlots => freeSlots.AsReadOnly();
        protected List<InventorySlot> freeSlots = new List<InventorySlot>();


        public ReadOnlyCollection<Item> Items => items.AsReadOnly();
        private List<Item> items = new List<Item>();

        public int FreeSlotsCount { get { return slots.Count - ItemsCount; } }
        public int ItemsCount { get; private set; }

        public float MaxVolume { get; set; }
        public float CurrentVolume { get; protected set; }

        public InventorySet()
        {
            AddLocalRule(checkVolume);
        }

        public bool AddItem(Item item)
        {
            InventorySlot fitSlot = null;
            int maxRulesPassed = -1;
            for (int i = 0; i < freeSlots.Count; i++)
            {
                var slot = freeSlots[i];
                int rulesPassed = 0;
                if (slot.SlotEmpty)
                {
                    if(slot.CheckRules(item, ref rulesPassed))
                    {
                        if(maxRulesPassed < rulesPassed)
                        {
                            fitSlot = slot;
                            maxRulesPassed = rulesPassed;
                        }
                    }
                }
            }

            if(fitSlot != null)
            {
                return fitSlot.AddItem(item);
            }
            return false;
        }

        public void AddSlot(InventorySlot slot)
        {
            if (slot != null)
            {
                slots.Add(slot);
                slot.OnAddItem += OnSlotAddItemHandler;
                slot.OnRemoveItem += OnSlotRemoveItemHandler;
                slot.OnReplaceItem += OnSlotReplaceItemHandler;

                slot.AddCheckRule(checkRules);

                slots.Sort();

                if (slot.SlotEmpty)
                {
                    freeSlots.Add(slot);
                }
                OnChange?.Invoke(this);
            }
        }
        public void RemoveSlot(InventorySlot slot)
        {
            if (slot != null)
            {
                slots.Remove(slot);
                slot.OnAddItem -= OnSlotAddItemHandler;
                slot.OnRemoveItem -= OnSlotRemoveItemHandler;
                slot.OnReplaceItem -= OnSlotReplaceItemHandler;

                slot.RemoveCheckRule(checkRules);

                freeSlots.Remove(slot);

                OnChange?.Invoke(this);
            }
        }

        public void AddLocalRule(Func<Item, bool> rule)
        {
            localRules.Add(rule);
        }
        public void RemoveLocalRule(Func<Item, bool> rule)
        {
            localRules.Remove(rule);
        }
        public virtual bool checkRules(Item item)
        {
            for (int i = 0; i < localRules.Count; i++)
            {
                if (!localRules[i](item))
                {
                    return false;
                }
            }
            return true;
        }
        public virtual bool checkRules(Item item, ref int rulesPassed)
        {
            rulesPassed = 0;
            for (int i = 0; i < localRules.Count; i++)
            {
                if (!localRules[i](item))
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

        protected void onChange()
        {
            OnChange?.Invoke(this);
        }
        protected virtual void OnSlotAddItemHandler(InventorySlot slot, Item item)
        {
            ItemsCount++;
            CurrentVolume += item.Volume;
            items.Add(item);
            if (!slot.SlotEmpty)
            {
                freeSlots.Remove(slot);
            }
            OnAddItem?.Invoke(this, slot, item);
        }
        protected virtual void OnSlotRemoveItemHandler(InventorySlot slot, Item item)
        {
            ItemsCount--;
            CurrentVolume -= item.Volume;
            items.Remove(item);
            if (slot.SlotEmpty)
            {
                freeSlots.Add(slot);
            }
            OnRemoveItem?.Invoke(this, slot, item);
        }
        protected virtual void OnSlotReplaceItemHandler(InventorySlot slot, Item newItem,
            Item oldItem)
        {
            CurrentVolume -= oldItem.Volume;
            CurrentVolume += newItem.Volume;

            items.Remove(oldItem);
            items.Add(newItem);
            OnReplaceItem?.Invoke(this, slot, newItem, oldItem);
        }

        public virtual bool checkVolume(Item item)
        {
            return item.Volume + CurrentVolume <= MaxVolume;
        }
    }
    public class InventorySet<T> : InventorySet where T : Item
    {
        public InventorySet()
        {
            AddLocalRule(checkGenericType);
        }

        private bool checkGenericType(Item item)
        {
            return item is T;
        }
    }
}
