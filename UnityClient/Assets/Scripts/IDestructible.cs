using System.Collections.Generic;

namespace COSMOS
{
    public interface IDestructible
    {
        KeyValuePair<string, float>[] GetDefenseModifiers();
        void ApplyDamage(Damage damage);
    }
}
