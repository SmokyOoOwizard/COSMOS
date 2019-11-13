using UnityEngine;
using UnityEditor;

namespace COSMOS.Skills
{
    public abstract class Skill
    {
        public string inerName;
        public lstring Name;
        public lstring Description;
        public string IconID;
        public virtual void Update() { }
    }
    public abstract class ActiveSkill : Skill
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