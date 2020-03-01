using COSMOS.Core.EventDispacher;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Character
{
    public class Ability : DataWithEvents
    {
        public string innerName;
        public string LKeyName;
        public string IconID;
        public string DescriptionLKey;

        public uint CurrentLevel { get; private set; }
        public uint MaxLevel;
        public float CalculatedStatWithBonuses { get; private set; }
        public string DetailTotalStat { get; private set; }

        public Dictionary<uint, object> BonusesByLevels = new Dictionary<uint, object>();
        public List<object> BonusesReceived = new List<object>();

        Dictionary<AbilityBonus.Operation, List<AbilityBonus>> bonuses;

        public ReadOnlyDictionary<AbilityBonus.Operation, ReadOnlyCollection<AbilityBonus>> GetBonuses()
        {
            Dictionary<AbilityBonus.Operation, ReadOnlyCollection<AbilityBonus>> b = new Dictionary<AbilityBonus.Operation, ReadOnlyCollection<AbilityBonus>>();
            foreach (var item in bonuses)
            {
                b.Add(item.Key, item.Value.AsReadOnly());
            }
            return new ReadOnlyDictionary<AbilityBonus.Operation, ReadOnlyCollection<AbilityBonus>>(b);
        }
        public void RecalculateStat()
        {
            string detail = CurrentLevel.ToString();
            float total = CurrentLevel;
            foreach (var bonusType in bonuses)
            {
                float totalType = 0;
                switch (bonusType.Key)
                {
                    case AbilityBonus.Operation.Multiply:
                        totalType = bonusType.Value.Select((bonus) => bonus.Value).Sum();
                        total *= totalType;
                        detail += "*" + totalType;
                        break;
                    case AbilityBonus.Operation.Divide:
                        totalType = bonusType.Value.Select((bonus) => bonus.Value).Sum();
                        total /= totalType;
                        detail += "/" + totalType;
                        break;
                    case AbilityBonus.Operation.Add:
                        totalType = bonusType.Value.Select((bonus) => bonus.Value).Sum();
                        total += totalType;
                        detail += "+" + totalType;
                        break;
                    case AbilityBonus.Operation.Subtract:
                        totalType = bonusType.Value.Select((bonus) => bonus.Value).Sum();
                        total -= totalType;
                        detail += "-" + totalType;
                        break;
                }
            }

            bool needNotify = false;
            if(CalculatedStatWithBonuses != total)
            {
                CalculatedStatWithBonuses = total;
                needNotify = true;
            }
            if(DetailTotalStat != detail)
            {
                DetailTotalStat = detail;
                needNotify = true;
            }
            if (needNotify)
            {
                Notify();
            }
        }
        public void LevelUp()
        {
            if (CurrentLevel < MaxLevel)
            {
                CurrentLevel++;
                if (BonusesByLevels.ContainsKey(CurrentLevel))
                {
                    BonusesReceived.Add(BonusesByLevels[CurrentLevel]);
                }
                RecalculateStat();
            }
        }

    }
}
