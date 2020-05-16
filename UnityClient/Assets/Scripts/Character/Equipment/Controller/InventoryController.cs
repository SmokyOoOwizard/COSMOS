using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Equipment
{
    public class InventoryController
    {
        public event Action<InventorySet> OnAddInventorySet;
        public event Action<InventorySet> OnRemoveInventorySet;
        public event InventorySet.OnAddItemDelegate OnAddItem;
        public event InventorySet.OnRemoveItemDelegate OnRemoveItem;
        public event InventorySet.OnReplaceItemDelegate OnReplaceItem;

        public ReadOnlyCollection<InventorySet> InventorySets => inventorySets.AsReadOnly();
        protected List<InventorySet> inventorySets = new List<InventorySet>();

        protected List<Func<Item, bool>> globalRules = new List<Func<Item, bool>>();

        public ReadOnlyCollection<Item> Items => items.AsReadOnly();
        private List<Item> items = new List<Item>();


        public InventoryController()
        {

        }

        public bool AddItem(Item item)
        {
            InventorySet fitSet = null;
            int maxRulesPassed = -1;
            for (int i = 0; i < inventorySets.Count; i++)
            {
                var set = inventorySets[i];
                int rulesPassed = 0;
                if(set.checkRules(item, ref rulesPassed))
                {
                    if(maxRulesPassed < rulesPassed)
                    {
                        fitSet = set;
                        maxRulesPassed = rulesPassed;
                    }
                }
            }
            if (fitSet != null)
            {
                return fitSet.AddItem(item);
            }
            return false;
        }

        public void AddGlobalRule(Func<Item, bool> rule)
        {
            globalRules.Add(rule);
        }
        public void RemoveGlobalRule(Func<Item, bool> rule)
        {
            globalRules.Remove(rule);
        }

        public void AddInventorySet(InventorySet set)
        {
            if (set != null)
            {
                inventorySets.Add(set);
                set.AddLocalRule(CheckRules);

                set.OnAddItem += OnAddItem;
                set.OnAddItem += onAddItemHandler;

                set.OnRemoveItem += OnRemoveItem;
                set.OnRemoveItem += onRemoveItemHandler;

                set.OnReplaceItem += OnReplaceItem;
                set.OnReplaceItem += onReplaceItemHandler;


                OnAddInventorySet?.Invoke(set);
            }
        }
        public void RemoveEquipmentSet(InventorySet set)
        {
            if (set != null)
            {
                inventorySets.Remove(set);
                set.RemoveLocalRule(CheckRules);

                set.OnAddItem -= OnAddItem;
                set.OnAddItem -= onAddItemHandler;

                set.OnRemoveItem -= OnRemoveItem;
                set.OnRemoveItem -= onRemoveItemHandler;

                set.OnReplaceItem -= OnReplaceItem;
                set.OnReplaceItem -= onReplaceItemHandler;

                OnRemoveInventorySet?.Invoke(set);
            }
        }

        public virtual bool CheckRules(Item item)
        {
            for (int i = 0; i < globalRules.Count; i++)
            {
                if (!globalRules[i](item))
                {
                    return false;
                }
            }
            return true;
        }
        public virtual bool CheckRules(Item item, ref int rulesPassed)
        {
            rulesPassed = 0;
            for (int i = 0; i < globalRules.Count; i++)
            {
                if (!globalRules[i](item))
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

        private void onAddItemHandler(InventorySet set, InventorySlot slot, Item item)
        {
            items.Add(item);
        }
        private void onRemoveItemHandler(InventorySet set, InventorySlot slot, Item item)
        {
            items.Remove(item);
        }
        private void onReplaceItemHandler(InventorySet set, InventorySlot slot, Item newItem,
            Item oldItem)
        {
            items.Remove(newItem);
            items.Add(oldItem);
        }
    }
}
