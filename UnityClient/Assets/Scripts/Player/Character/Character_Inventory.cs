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
        List<EquipmentPart> EquipmentsParts = new List<EquipmentPart>();
        List<Inventory> Inventories = new List<Inventory>();

        public Inventory[] GetInventories()
        {
            return Inventories.ToArray();
        }
        public EquipmentPart[] GetEquipmentsParts()
        {
            return EquipmentsParts.ToArray();
        }

    }
}
