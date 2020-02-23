using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Equipment;

namespace COSMOS.Character
{
    public partial class Character
    {
        List<CharacterEquipmentPart> EquipmentsParts = new List<CharacterEquipmentPart>();
        List<Inventory> Inventories = new List<Inventory>();

        public Inventory[] GetInventories()
        {
            return Inventories.ToArray();
        }
        public CharacterEquipmentPart[] GetEquipmentsParts()
        {
            return EquipmentsParts.ToArray();
        }

    }
}
