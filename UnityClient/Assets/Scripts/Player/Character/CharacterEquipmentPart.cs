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

        Dictionary<CharacterEqiopmentRule, Item> Equipments = new Dictionary<CharacterEqiopmentRule, Item>();
    }
    public class CharacterEqiopmentRule
    {

    }
}
