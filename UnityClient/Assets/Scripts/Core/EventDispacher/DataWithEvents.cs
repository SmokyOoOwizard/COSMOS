using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Core.EventDispacher
{
    public abstract class DataWithEvents : IDataWithEvents
    {
        HashSet<Action<ushort>> listeners = new HashSet<Action<ushort>>();

        public void Notify(ushort flag = ushort.MaxValue)
        {
            foreach (var listener in listeners)
            {
                listener?.Invoke(flag);
            }
        }

        public void AddListener(Action<ushort> listener)
        {
            listeners.Add(listener);
        }
        public void RemoveListener(Action<ushort> listener)
        {
            listeners.Remove(listener);
        }
        public bool ContaintsListener(Action<ushort> listener)
        {
            return listeners.Contains(listener);
        }
    }
}
