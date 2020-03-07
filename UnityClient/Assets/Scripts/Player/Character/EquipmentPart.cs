using COSMOS.Core.EventDispacher;
using COSMOS.Equipment;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Character
{
	public class EquipmentPart : DataWithEvents
	{
		public string LKeyName;
		public string LkeyDescription;

		public ReadOnlyDictionary<EquipmentRule, Item> Equipments 
		{ get { return new ReadOnlyDictionary<EquipmentRule, Item>(equipments); } }
		Dictionary<EquipmentRule, Item> equipments = new Dictionary<EquipmentRule, Item>();

		public EquipmentRule[] GetEquipmentRules()
		{
			return equipments.Keys.ToArray();
		}
		public void SetItem(EquipmentRule rule, Item item)
		{
			if (equipments.ContainsKey(rule))
			{
				equipments[rule] = item;
				// TODO notify
			}
		}
		public void AddRule(EquipmentRule rule)
		{
			equipments.Add(rule, null);
		}
		public bool RemoveRule(EquipmentRule rule)
		{
			return equipments.Remove(rule);
		}
	}
	public class EquipmentRule
	{
		public uint ItemType = uint.MaxValue;
		public string[] AcceptedSecondaryTypes { get; protected set; } = new string[0];
		public string[] NotAllowSecondaryTypes { get; protected set; } = new string[0];
		public string SlotImageID;

		public bool ItemFit(Item item)
		{
			if ((ItemType & item.ItemType) != 0)
			{
				if (NotAllowSecondaryTypes.Length == 0 || item.SecondaryItemType.Length == 0 || item.SecondaryItemType.Intersect(NotAllowSecondaryTypes).Count() < 1)
				{
					if (AcceptedSecondaryTypes.Length == 0 || item.SecondaryItemType.Length == 0 || (item.SecondaryItemType.Intersect(AcceptedSecondaryTypes).Count() > 0))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
