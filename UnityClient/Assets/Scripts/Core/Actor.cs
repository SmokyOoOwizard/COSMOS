using COSMOS.Core.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.Core
{
    [DisallowMultipleComponent]
    public sealed class Actor : MonoBehaviour
    {
        public readonly DictionaryList<Type, ABehaviour> Behaviours = new DictionaryList<Type, ABehaviour>();
        public readonly DictionaryList<Type, AData> Data = new DictionaryList<Type, AData>();

        private readonly List<ITick> tickBehaviours = new List<ITick>();
        #region Behaviours
        public void AddBehaviour<T>(T behaviour) where T : ABehaviour
        {
            Behaviours.Add(typeof(T), behaviour);
            behaviour.Awake();
            if(behaviour is ITick)
            {
                tickBehaviours.Add(behaviour as ITick);
            }
        }
        public T GetBehaviour<T>() where T : ABehaviour
        {
            return (T)Behaviours.Get(typeof(T));
        }
        public bool ExistBehaviour<T>() where T : ABehaviour
        {
            return Behaviours.ContainsKey(typeof(T));
        }
        public bool ExistBehaviour<T>(T behaviour) where T : ABehaviour
        {
            return Behaviours.ContainsValue(behaviour);
        }
        public bool RemoveBehaviour<T>(T behaviour) where T : ABehaviour
        {
            if(Behaviours.Remove(typeof(T), behaviour))
            {
                behaviour.OnDestroy();
                if(behaviour is ITick)
                {
                    tickBehaviours.Remove(behaviour as ITick);
                }
                return true;
            }
            return false;
        }
        #endregion
        #region Data
        public T GetData<T>() where T : AData
        {
            Data.TryGetValue(typeof(T), out AData data);
            return (T)data;
        }
        public bool ExistData<T>() where T : AData
        {
            return Data.ContainsKey(typeof(T));
        }
        public bool ExistData<T>(T data) where T : AData
        {
            return Data.Contains(typeof(T), data);
        }
        public bool AddData<T>(T data) where T : AData
        {
            if (data.Deleted)
            {
                return false;
            }
            data.OnDelete += removeData;
            Data.Add(typeof(T), data);
            return true;
        }
        public bool RemoveData<T>(T data) where T : AData
        {
            if (!data.Immortal)
            {
                if (ExistData(data))
                {
                    data.OnDelete -= removeData;
                    if (Data.Remove(typeof(T), data))
                    {
                        data.Delete();
                        return true;

                    }
                }
            }
            return false;
        }
        public bool RemoveData<T>() where T : AData
        {
            if (Data.RemoveFirst(typeof(T), out AData data))
            {
                if (data.Immortal)
                {
                    Data.Add(typeof(T), data);
                    return false;
                }
                data.OnDelete -= removeData;
                data.Delete();
                return true;
            }
            return false;
        }
        private void removeData<T>(T data) where T : AData
        {
            RemoveData(data);
        }
        #endregion


        private void Update()
        {
            for (int i = 0; i < tickBehaviours.Count; i++)
            {
                tickBehaviours[i].OnTick();
            }
        }
    }
}
