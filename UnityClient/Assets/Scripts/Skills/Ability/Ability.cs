using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Skills.Ability
{
    public abstract class Ability
    {
        public delegate void levelUpEvent(AbilityStatBonus[] bonus);

        public string inerName;
        public lstring Name;
        public string IconID;
        public lstring Description;

        public uint CurrentXP;
        public uint CurrentLevel;
        public List<uint> Levels = new List<uint>();

        public event levelUpEvent LevelUp;
        public uint AddXP(uint xp)
        {
            if (CurrentLevel < Levels.Count - 1)
            {
                uint needXpToNextLevel = Levels[(int)CurrentLevel];
                CurrentXP += xp;
                if (CurrentXP >= needXpToNextLevel)
                {
                    CurrentXP -= needXpToNextLevel;

                    nextLevel();
                    uint tmp = CurrentXP;
                    CurrentXP = 0;
                    return tmp;
                }
                return 0;
            }
            else
            {
                return xp;
            }
        }

        void nextLevel()
        {
            if (CurrentLevel < Levels.Count - 1)
            {
                CurrentLevel++;
                LevelUp.Invoke(levelUp());

            }
        }
        protected abstract AbilityStatBonus[] levelUp();
        public abstract void GetActiveSkills();
        public abstract void GetPassiveSkills();
    }
}
