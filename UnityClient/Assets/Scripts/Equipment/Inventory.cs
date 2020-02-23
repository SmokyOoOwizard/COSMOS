using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Equipment
{
    public class Inventory
    {
        public string LKeyName;

        public float MaxWeight;
        public float CurrentWeight;
        public float MaxVolume;
        public float CurrentVolume;

        List<Item> items = new List<Item>();

        public bool AddItem(Item item)
        {
            if (CurrentVolume + item.Volume <= MaxVolume)
            {
                if (CurrentWeight + item.Weight <= MaxWeight)
                {
                    items.Add(item);
                    return true;
                }
            }
            return false;
        }
        public bool ExistItem(Item item)
        {
            return items.Contains(item);
        }
        public bool Swap(Item from, Item to)
        {
            int i1 = items.FindIndex((item) => item == from);
            int i2 = items.FindIndex((item) => item == to);

            if (i1 == -1 || i2 == -1)
            {
                return false;
            }

            Item tmp = items[i1];
            items[i1] = items[i2];
            items[i2] = tmp;
            return true;
        }
        public bool Remove(Item item)
        {
            return items.Remove(item);
        }
        public Item[] GetItems()
        {
            return items.ToArray();
        }

    }
}
