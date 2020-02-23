using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Character
{
    public partial class Character
    {
        Dictionary<string, Ability> Abilities = new Dictionary<string, Ability>();
        Dictionary<string, Dictionary<AbilityBonus.Operation, List<AbilityBonus>>> AbilitiesBonuses;
        Dictionary<string, float> CalculatedAbilitiesStatWithBonuses;

        public bool HasAbility(string Name)
        {
            return Abilities.ContainsKey(Name);
        }
        public Ability GetAbility(string Name)
        {
            if (Abilities.ContainsKey(Name))
            {
                return Abilities[Name];
            }
            return null;
        }
        public Dictionary<AbilityBonus.Operation, List<AbilityBonus>> GetAbilityBonuses(string Name)
        {
            if (AbilitiesBonuses.ContainsKey(Name))
            {
                return AbilitiesBonuses[Name];
            }
            return null;
        }
        public float GetAbilityValueWithBonuses(string Name)
        {
            if (Abilities.ContainsKey(Name))
            {
                return CalculatedAbilitiesStatWithBonuses[Name];
            }
            return 0;
        }
        public void RecalculateAbilityBonuses(string AbilityName = null)
        {
            if (!string.IsNullOrEmpty(AbilityName))
            {
                recalculateAbility(AbilityName);
            }
        }
        public void RecalulateAllAbilitiesBonuses()
        {
            foreach (var item in Abilities)
            {
                recalculateAbility(item.Key);
            }
        }
        void recalculateAbility(string name)
        {
            if (Abilities.ContainsKey(name))
            {
                float baseValue = Abilities[name].CurrentLevel;
                if (AbilitiesBonuses.ContainsKey(name))
                {
                    foreach (var item in AbilitiesBonuses[name])
                    {
                        if(item.Value != null)
                        {
                            float modifier = item.Value.Select((bonus) => bonus.Value).Sum();
                            switch (item.Key)
                            {
                                case AbilityBonus.Operation.Multiply:
                                    baseValue *= modifier;
                                    break;
                                case AbilityBonus.Operation.Divide:
                                    baseValue /= modifier;
                                    break;
                                case AbilityBonus.Operation.Add:
                                    baseValue += modifier;
                                    break;
                                case AbilityBonus.Operation.Subtract:
                                    baseValue -= modifier;
                                    break;
                            }
                        }
                    }
                }
                if (CalculatedAbilitiesStatWithBonuses.ContainsKey(name))
                {
                    CalculatedAbilitiesStatWithBonuses[name] = baseValue;
                }
                else
                {
                    CalculatedAbilitiesStatWithBonuses.Add(name, baseValue);
                }
            }
        }

    }
}
