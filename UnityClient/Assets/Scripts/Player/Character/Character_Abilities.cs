using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Character
{
    public partial class Character
    {
        Dictionary<string, Ability> Abilities = new Dictionary<string, Ability>();

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
        public Ability[] GetAbilities()
        {
            return Abilities.Values.ToArray();
        }
        public ReadOnlyDictionary<AbilityBonus.Operation, ReadOnlyCollection<AbilityBonus>> GetAbilityBonuses(string Name)
        {
            if (Abilities.ContainsKey(Name))
            {
                return Abilities[Name].GetBonuses();
            }
            return null;
        }
        public float GetAbilityValueWithBonuses(string Name)
        {
            if (Abilities.ContainsKey(Name))
            {
                return Abilities[Name].CalculatedStatWithBonuses;
            }
            return 0;
        }
        public void RecalculateAbilityBonuses(string AbilityName = null)
        {
            if (!string.IsNullOrEmpty(AbilityName))
            {
                if (Abilities.ContainsKey(AbilityName))
                {
                    Abilities[AbilityName].RecalculateStat();
                }
            }
        }
        public void RecalulateAllAbilitiesBonuses()
        {
            foreach (var item in Abilities)
            {
                item.Value.RecalculateStat();
            }
        }
    }
}
