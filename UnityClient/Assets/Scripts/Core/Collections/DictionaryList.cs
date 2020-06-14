using System.Collections.Generic;
using System.Linq;

namespace COSMOS.Core.Collections
{
    public class DictionaryList<K, VL>
    {
        public readonly Dictionary<K, List<VL>> Data = new Dictionary<K, List<VL>>();

        public void Add(K key, VL value)
        {
            if (!Data.ContainsKey(key))
            {
                Data.Add(key, new List<VL>());
            }
            Data[key].Add(value);
        }
        public VL Get(K key)
        {
            if (Data.ContainsKey(key))
            {
                var list = Data[key];
                if (list.Count > 0)
                {
                    return list[0];
                }
            }
            return default(VL);
        }
        public VL Get(K key, int index)
        {
            if (Data.ContainsKey(key))
            {
                var list = Data[key];
                if (list.Count > index)
                {
                    return list[index];
                }
            }
            return default(VL);
        }
        public bool TryGetValue(K key, out VL value)
        {
            if (Data.ContainsKey(key))
            {
                var list = Data[key];
                if (list.Count > 0)
                {
                    value = list[0];
                    return true;
                }
            }
            value = default(VL);
            return false;
        }
        public bool TryGetValue(K key, int index, out VL value)
        {
            if (Data.ContainsKey(key))
            {
                var list = Data[key];
                if (list.Count > index)
                {
                    value = list[index];
                    return true;
                }
            }
            value = default(VL);
            return false;
        }
        public bool ContainsKey(K key)
        {
            return Data.ContainsKey(key);
        }
        public bool ContainsValue(VL value)
        {
            foreach (var data in Data)
            {
                if (data.Value.Contains(value))
                {
                    return true;
                }
            }
            return false;
        }
        public bool Contains(K key, VL value)
        {
            if (Data.ContainsKey(key))
            {
                return Data[key].Contains(value);
            }
            return false;
        }
        public bool RemoveFirst(K key)
        {
            if (Data.ContainsKey(key))
            {
                var list = Data[key];
                if (list.Count > 0)
                {
                    list.RemoveAt(0);
                    return true;
                }
            }
            return false;
        }
        public bool RemoveFirst(K key, out VL value)
        {
            if (Data.ContainsKey(key))
            {
                var list = Data[key];
                if (list.Count > 0)
                {
                    value = list[0];
                    list.RemoveAt(0);
                    return true;
                }
            }
            value = default(VL);
            return false;
        }
        public bool Remove(K key, VL value)
        {
            if (Data.ContainsKey(key))
            {
                var list = Data[key];
                return list.Remove(value);
            }
            return false;
        }

    }
}
