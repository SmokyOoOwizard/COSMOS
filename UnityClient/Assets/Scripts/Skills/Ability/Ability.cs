using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Skills.Ability
{
    public abstract class Ability
    {
        public string inerName;
        public string Name;
        public uint CurrentXP;
        public uint CurrentLevel;
        public List<uint> Levels = new List<uint>();

        public void AddXP(uint xp)
        {
            uint needXpToNextLevel = Levels[(int)CurrentLevel];
            CurrentXP += xp;
            if(CurrentXP >= needXpToNextLevel)
            {
                CurrentXP -= needXpToNextLevel;

                nextLevel();
            }
        }

        void nextLevel()
        {
            if (CurrentLevel < Levels.Count - 1)
            {
                CurrentLevel++;
                levelUp();
            }
        }
        protected abstract void levelUp();
    }
    public abstract class SubAbility : Ability
    {

    }
    public abstract class MainAblity : Ability
    {
        public List<SubAbility> SubAbilities = new List<SubAbility>();

    }
}
