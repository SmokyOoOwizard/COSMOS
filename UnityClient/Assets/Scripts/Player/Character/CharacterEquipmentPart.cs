using COSMOS.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Character
{
    public class CharacterEquipmentPart
    {
        public string LKeyName;
        public string LkeyDescription;

        Dictionary<CharacterEquipmentRule, Item> Equipments = new Dictionary<CharacterEquipmentRule, Item>();
    }
    public class CharacterEquipmentRule
    {
        public uint ItemType = uint.MaxValue;
        public string[] AcceptedSecondaryTypes { get; protected set; } = new string[0];
        public string[] NotAllowSecondaryTypes { get; protected set; } = new string[0];

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
