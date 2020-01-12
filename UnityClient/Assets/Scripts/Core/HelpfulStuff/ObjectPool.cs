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
        public int MAX { get; private set; } = 10;

        Func<T> createObject;
        public bool CanOverSize { get; private set; }

        public ObjectPool(T[] objects)
        {
            MAX = objects.Length;
            counter = MAX;
            for (int i = 0; i < objects.Length; i++)
            {
                items.Add(objects[i]);
            }
        }
        public ObjectPool(int size, bool canGrow, Func<T> createObject)
        {
            if (createObject != null)
            {
                this.createObject = createObject;
                CanOverSize = canGrow;
                MAX = size;
                counter = MAX;
                for (int i = 0; i < size; i++)
                {
                    items.Add(createObject());
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
            else if(CanOverSize && createObject != null)
            {
                T obj = createObject();
                items.Add(obj);
                counter++;
                return obj;
            }
            return default(T);
        }
        public bool Release(T item)
        {
            if (counter < MAX)
            {
                items.Add(item);
                counter++;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}