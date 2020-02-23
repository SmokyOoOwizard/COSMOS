using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Skills;

namespace COSMOS.Character
{
    public partial class Character
    {
        List<ActiveSkill> activeSkills = new List<ActiveSkill>();
        List<PassiveSkill> passiveSkills = new List<PassiveSkill>();

        public ActiveSkill[] GetActiveSkills()
        {
            return activeSkills.ToArray();
        }
        public PassiveSkill[] GetPassiveSkills()
        {
            return passiveSkills.ToArray();
        }
    }
}
