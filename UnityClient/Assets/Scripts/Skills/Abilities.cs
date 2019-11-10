using COSMOS.Skills.Ability;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace COSMOS
{
    public class Abilities
    {
        List<Ability> abilities = new List<Ability>();
        Dictionary<string, float> BaseStats = new Dictionary<string, float>();
        Dictionary<string, float> Stats = new Dictionary<string, float>();
        Dictionary<string, List<AbilityStatBonus>> StatBonuses = new Dictionary<string, List<AbilityStatBonus>>();
        public Ability GetAbility(string ability)
        {
            return abilities.Find((x) => x.inerName == ability);
        }
        public float? GetStat(string stat)
        {
            if (Stats.ContainsKey(stat))
            {
                return Stats[stat];
            }
            else
            {
                return null;
            }
        }
        public float? GetBaseStat(string stat)
        {
            if (BaseStats.ContainsKey(stat))
            {
                return BaseStats[stat];
            }
            else
            {
                return null;
            }
        }
        public void ApplyBonuses(List<AbilityStatBonus> bonuses)
        {
            foreach (var bonus in bonuses)
            {
                ApplyBonus(bonus);
            }
        }
        public void ApplyBonus(AbilityStatBonus bonus)
        {
            if (StatBonuses.ContainsKey(bonus.StatName))
            {
                StatBonuses[bonus.StatName].Add(bonus);
            }
            else
            {
                StatBonuses.Add(bonus.StatName, new List<AbilityStatBonus>() { bonus });
            }

            if (Stats.ContainsKey(bonus.StatName))
            {
                Stats[bonus.StatName] = bonus.Apply(Stats[bonus.StatName]);
            }
            else
            {
                float s = 0;
                if (BaseStats.ContainsKey(bonus.StatName))
                {
                    s = BaseStats[bonus.StatName];
                }
                s = bonus.Apply(s);
                Stats.Add(bonus.StatName, s);
            }
        }
        public void RecalculateStats()
        {
            Dictionary<string, float> stats = new Dictionary<string, float>();
            foreach (var bonuses in StatBonuses)
            {
                float s = 0;
                if (BaseStats.ContainsKey(bonuses.Key))
                {
                    s = BaseStats[bonuses.Key];
                }
                foreach (var bonus in bonuses.Value)       
                {
                    s = bonus.Apply(s);
                }
                stats.Add(bonuses.Key, s);
            }
            foreach (var stat in BaseStats)
            {
                if (!stats.ContainsKey(stat.Key))
                {
                    stats.Add(stat.Key, stat.Value);
                }
            }

            Stats = stats;
        }
        public void RecalculateStat(string stat)
        {
            float s = 0;
            if (BaseStats.ContainsKey(stat))
            {
                s = BaseStats[stat];
            }
            if (StatBonuses.ContainsKey(stat))
            {
                List<AbilityStatBonus> bonuses = StatBonuses[stat];
                foreach (var bonus in bonuses)
                {
                    s = bonus.Apply(s);
                }
            }
            if (Stats.ContainsKey(stat))
            {
                Stats[stat] = s;
            }
            else
            {
                Stats.Add(stat, s);
            }
        }
    }
}