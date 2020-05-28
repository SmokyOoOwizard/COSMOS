using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS
{
    [Serializable]
    public class Damage
    {
        public ReadOnlyDictionary<DamageType, float> Damages
        {
            get
            {
                return new ReadOnlyDictionary<DamageType, float>(damages);
            }
        }
        private Dictionary<DamageType, float> damages = new Dictionary<DamageType, float>();

        public Damage(KeyValuePair<DamageType, float>[] damage)
        {
            damages = new List<KeyValuePair<DamageType, float>>(damage).ToDictionary(x => x.Key, x => x.Value);
        }

        public float GetRawDamageAmount()
        {
            float amount = 0;

            var d = damages.Values.ToArray();
            for (int i = 0; i < d.Length; i++)
            {
                amount += d[i];
            }

            return amount;
        }
    }
    [Serializable]
    public class DamageType
    {
        public string Type;
        public Dictionary<string, float> DefenseModifiers;

    }
}
