using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Core.Collections;
using COSMOS.Core.EventDispacher;
using COSMOS.Equipment;

namespace COSMOS.Character
{
    public partial class Character
    {
        public readonly ListWithEvents<EquipmentPart> EquipmentsParts = new ListWithEvents<EquipmentPart>();
        public readonly ListWithEvents<Inventory> CharacterInventories = new ListWithEvents<Inventory>();
        public readonly ListWithEvents<Inventory> AccessibleInventories = new ListWithEvents<Inventory>();
    }
}
