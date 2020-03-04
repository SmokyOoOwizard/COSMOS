using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.Core.HelpfulStuff
{
    public class ObjectPool<T>
    {
        private readonly ConcurrentBag<T> items = new ConcurrentBag<T>();
        public int counter { get; private set; } = 0;
        public int MAX { get; private set; } = 30;

        Func<T> createObject;
        Action<T> release;
        public bool CanOverSize { get; private set; }

        public ObjectPool(T[] objects, Action<T> releaseMethod)
        {
            MAX = objects.Length;
            counter = MAX;
            release = releaseMethod;
            for (int i = 0; i < objects.Length; i++)
            {
                items.Add(objects[i]);
            }
        }
        public ObjectPool(int size, bool canGrow, Func<T> createObjectMethod, Action<T> releaseMethod)
        {
            release = releaseMethod;
            if (createObjectMethod != null)
            {
                this.createObject = createObjectMethod;
                CanOverSize = canGrow;
                MAX = size;
                counter = MAX;
                for (int i = 0; i < size; i++)
                {
                    items.Add(createObjectMethod());
                }
            }
        }

        public T GetObject()
        {
            T item;
            if (items.TryTake(out item))
            {
                counter--;
                return item;
            }
            else if (CanOverSize && createObject != null)
            {
                T obj = createObject();
                counter--;
                return obj;
            }
            return default(T);
        }
        public bool Release(T item)
        {
            if (counter < MAX && item != null)
            {
                release?.Invoke(item);
                if (item != null)
                {
                    items.Add(item);
                    counter++;
                    return true;
                }
            }
            return false;
        }
    }
}