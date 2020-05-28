using COSMOS.Equipment;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Character
{
    public abstract partial class Character
    {
        public Dialogs.Dialog BaseDialog { get; protected set; }

        public Character()
        {
            ctorInventoryControllers();
        }


        public void SetBaseDialog(Dialogs.Dialog baseDialog)
        {
            BaseDialog = baseDialog;
        }
    }
    public abstract partial class Character
    {
        public virtual InventoryController EquipmentController { get; protected set; }
        = new InventoryController();
        public virtual InventoryController InventoryController { get; protected set; }
        = new InventoryController();
        public virtual ReadOnlyCollection<InventoryController> AdditionalInventoryControllers
        {
            get { return additionalInventoryControllers.AsReadOnly(); }
        }
        protected readonly List<InventoryController> additionalInventoryControllers = new List<InventoryController>();

        private void ctorInventoryControllers()
        {
            EquipmentController.OnAddItem += onAddEquipment_CheckAddInventory;
            EquipmentController.OnRemoveItem += onRemoveEquipment_CheckRemoveInventory;
            EquipmentController.OnReplaceItem += onReplaceEquipment_CheckReplaceInventory;
        }

        private void onAddEquipment_CheckAddInventory(InventorySet set, InventorySlot slot, Item item)
        {
            if(item.Inventory != null)
            {
                additionalInventoryControllers.Add(item.Inventory);
                // add slots
            }
        }
        private void onRemoveEquipment_CheckRemoveInventory(InventorySet set, InventorySlot slot, Item item)
        {
            if(item.Inventory != null)
            {
                additionalInventoryControllers.Remove(item.Inventory);
                // remove slots
            }
        }
        private void onReplaceEquipment_CheckReplaceInventory(InventorySet set, InventorySlot slot, 
            Item newItem, Item oldItem)
        {
            if(oldItem.Inventory != null)
            {
                additionalInventoryControllers.Remove(oldItem.Inventory);
                // remove slots
            }

            if(newItem.Inventory != null)
            {
                additionalInventoryControllers.Add(newItem.Inventory);
                // add slots
            }
        }
    }
}
