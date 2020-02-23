using UnityEngine;
using UnityEditor;
using COSMOS.Player;

namespace COSMOS.Skills
{
    public abstract class Skill
    {
        public string inerName;
        public string LKeyName { get; set; }
        public string LkeyDescription;
        public string IconID { get; set; }
        public virtual void Update() { }
    }
    public abstract class ActiveSkill : Skill, ICanPlaceInQuickSlot
    {
        public bool Use(params SkillTarget[] targets)
        {
            if (!CanUse(targets))
            {
                return false;
            }
            return use(targets);

        }
        public abstract bool CanUse(params SkillTarget[] targets);
        protected abstract bool use(params SkillTarget[] targets);
    }

    public abstract class PassiveSkill : Skill
    {

    }
}