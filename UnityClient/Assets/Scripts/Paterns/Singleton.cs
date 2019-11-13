using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.Paterns
{
    public abstract class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance;
        public void InitFather()
        {
            if (!instance)
            {
                instance = this as T;
                GameObject.DontDestroyOnLoad(instance.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    public abstract class Singleton<T> where T : class
    {
        public static T instance;
        public Singleton(T obj)
        {
            instance = obj;
        }
    }
}
