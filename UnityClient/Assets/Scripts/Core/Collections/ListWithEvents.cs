using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Core.Collections
{
    public class ListWithEvents<T> : List<T>
    {
        public event Action<T> OnAdd;
        public event Action<T, T> OnReplace;
        public event Action<T> OnRemove;

        public new T this[int i]
        {
            get { return base[i]; }
            set
            {
                T old = base[i];
                base[i] = value;
                OnReplace?.Invoke(value, old);
            }
        }

        public new void Add(T item)
        {
            OnAdd?.Invoke(item);
            base.Add(item);
        }
        public new void AddRange(IEnumerable<T> items)
        {
            base.AddRange(items);
            if (OnAdd != null)
            {
                foreach (var item in items)
                {
                    OnAdd(item);
                }
            }
        }
        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            OnAdd?.Invoke(item);
        }
        public new void InsertRange(int index, IEnumerable<T> items)
        {
            base.InsertRange(index, items);
            if (OnAdd != null)
            {
                foreach (var item in items)
                {
                    OnAdd(item);
                }
            }
        }
        public new bool Remove(T item)
        {
            bool result = base.Remove(item);
            if (result)
            {
                OnRemove?.Invoke(item);
            }
            return result;
        }
        public new void RemoveAt(int i)
        {
            T item = default(T);
            if (Count < i)
            {
                item = base[i];
            }
            base.RemoveAt(i);
            if (item != null)
            {
                OnRemove?.Invoke(item);
            }
        }
        [Obsolete("DON'T USE THIS METHOD. THIS METHOD DONT CALL OnRemove EVENT")]
        public new int RemoveAll(Predicate<T> match)
        {
            return base.RemoveAll(match);
        }
        public new void RemoveRange(int index, int count)
        {
            T[] items = base.GetRange(index, count).ToArray();
            base.RemoveRange(index, count);
            if (OnRemove != null)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    OnRemove(items[i]);
                }
            }
        }
    }
}
