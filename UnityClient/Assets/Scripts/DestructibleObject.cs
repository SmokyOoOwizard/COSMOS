using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace COSMOS
{
    [Serializable]
    public class DamageEvent : UnityEvent<Damage>
    {

    }

    [RequireComponent(typeof(Collider))]
    public class DestructibleObject : MonoBehaviour, IDestructible
    {
        [SerializeField]
        public DamageEvent OnApplyDamage;
        public UnityEvent OnDead;
        
        [SerializeField]
        public float HP;

        public void ApplyDamage(Damage damage)
        {
            HP -= damage.GetRawDamageAmount();
        }

        public KeyValuePair<string, float>[] GetDefenseModifiers()
        {
            return new KeyValuePair<string, float>[0];
        }
    }
}
