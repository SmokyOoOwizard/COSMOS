using COSMOS.Core.EventDispacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Character
{
    public class Ability : DataWithEvents
    {
        public string innerName;
        public string NameLKey;
        public string IconID;
        public string DescriptionLKey;

        public uint CurrentLevel { get; private set; }
        public uint MaxLevel;

        public Dictionary<uint, object> BonusesByLevels = new Dictionary<uint, object>();
        public List<object> BonusesReceived = new List<object>();

        public void LevelUp()
        {
            if (CurrentLevel < MaxLevel)
            {
                CurrentLevel++;
                if (BonusesByLevels.ContainsKey(CurrentLevel))
                {
                    BonusesReceived.Add(BonusesByLevels[CurrentLevel]);
                }
            }
        }

    }
}
