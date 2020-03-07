using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Core.EventDispacher
{
    public abstract class DataWithEvents : IDataWithEvents
    {
        Dictionary<uint, HashSet<Action<uint, EventArg>>> listeners = new Dictionary<uint, HashSet<Action<uint, EventArg>>>();

        public void Notify(uint flag = uint.MaxValue, EventArg args = null)
        {
            if(args == null)
            {
                args = new EventArg();
            }
            foreach (var listenerType in listeners)
            {
                if ((listenerType.Key & flag) != 0)
                {
                    foreach (var listener in listenerType.Value)
                    {
                        listener?.Invoke(flag, args);
                    }
                }
            }
        }

        public void AddListener(Action<uint, EventArg> listener, uint flag = uint.MaxValue)
        {
            if (!listeners.ContainsKey(flag))
            {
                listeners.Add(flag, new HashSet<Action<uint, EventArg>>());
            }
            listeners[flag].Add(listener);
        }
        public void RemoveListener(Action<uint, EventArg> listener, uint flag = uint.MaxValue)
        {
            if (listeners.ContainsKey(flag))
            {
                listeners[flag].Remove(listener);
                if(listeners[flag].Count == 0)
                {
                    listeners.Remove(flag);
                }
            }
        }
        public bool ContaintsListener(Action<uint, EventArg> listener, uint flag = uint.MaxValue)
        {
            if (listeners.ContainsKey(flag))
            {
                if (listeners[flag].Contains(listener))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
