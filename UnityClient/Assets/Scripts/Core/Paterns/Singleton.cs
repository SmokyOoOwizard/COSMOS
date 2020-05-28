using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.Core.Paterns
{
    public abstract class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance;
        protected void InitPatern()
        {
            if (instance == null)
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
            if(instance == null)
            {
                instance = obj;
            }
        }
    }
}
