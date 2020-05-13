using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Equipment
{
    public class EquipmentController
    {
        public event Action<EquipmentSet> OnAddEquipmentSet;
        public event Action<EquipmentSet> OnRemoveEquipmentSet;
        public event EquipmentSet.OnAddEquipmentDelegate OnAddEquipment;
        public event EquipmentSet.OnRemoveEquipmentDelegate OnRemoveEquipment;
        public event EquipmentSet.OnReplaceEquipmentDelegate OnReplaceEquipment;

        public ReadOnlyCollection<EquipmentSet> EquipmentSets => equipmentSets.AsReadOnly();
        protected List<EquipmentSet> equipmentSets;

        protected List<Func<Equipment, bool>> globalRules = new List<Func<Equipment, bool>>();

        public ReadOnlyCollection<Equipment> Equipments => equipments.AsReadOnly();
        protected List<Equipment> equipments = new List<Equipment>();


        public EquipmentController()
        {

        }

        public void AddGlobalRule(Func<Equipment, bool> rule)
        {
            globalRules.Add(rule);
        }
        public void RemoveGlobalRule(Func<Equipment, bool> rule)
        {
            globalRules.Remove(rule);
        }

        public void AddEquipmentSet(EquipmentSet set)
        {
            if (set != null)
            {
                equipmentSets.Add(set);
                set.AddLocalRule(CheckRules);

                set.OnAddEquipment += OnAddEquipment;
                set.OnAddEquipment += onAddEquipmentHandler;

                set.OnRemoveEquipment += OnRemoveEquipment;
                set.OnRemoveEquipment += onRemoveEquipmentHandler;

                set.OnReplaceEquipment += OnReplaceEquipment;
                set.OnReplaceEquipment += onReplaceEquipmentHandler;


                OnAddEquipmentSet?.Invoke(set);
            }
        }
        public void RemoveEquipmentSet(EquipmentSet set)
        {
            if (set != null)
            {
                equipmentSets.Remove(set);
                set.RemoveLocalRule(CheckRules);

                set.OnAddEquipment -= OnAddEquipment;
                set.OnAddEquipment -= onAddEquipmentHandler;

                set.OnRemoveEquipment -= OnRemoveEquipment;
                set.OnRemoveEquipment -= onRemoveEquipmentHandler;

                set.OnReplaceEquipment -= OnReplaceEquipment;
                set.OnReplaceEquipment -= onReplaceEquipmentHandler;

                OnRemoveEquipmentSet?.Invoke(set);
            }
        }

        public virtual bool CheckRules(Equipment equipment)
        {
            for (int i = 0; i < globalRules.Count; i++)
            {
                if (!globalRules[i](equipment))
                {
                    return false;
                }
            }
            return true;
        }

        private void onAddEquipmentHandler(EquipmentSet set, EquipmentSlot slot, Equipment equipment)
        {
            equipments.Add(equipment);
        }
        private void onRemoveEquipmentHandler(EquipmentSet set, EquipmentSlot slot, Equipment equipment)
        {
            equipments.Remove(equipment);
        }
        private void onReplaceEquipmentHandler(EquipmentSet set, EquipmentSlot slot, Equipment newEquipment,
            Equipment oldEquipment)
        {
            equipments.Remove(newEquipment);
            equipments.Add(oldEquipment);
        }
    }
}
