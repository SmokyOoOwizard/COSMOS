using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogEditor
{
    [Serializable]
    public class DialogNodeData
    {
        public string GUID;
        public string Type;
        public Rect Rect;
        public Dictionary<string, object> Data = new Dictionary<string, object>();

        public T GetData<T>(string key)
        {
            if (Data.ContainsKey(key))
            {
                object obj = Data[key];
                if (obj != null)
                {
                    if (obj is T)
                    {
                        return (T)obj;
                    }
                    else
                    {
                        Debug.LogError($"Restore failde. Wrong type: \"{obj.GetType()}\" need type " +
                            $"\"{typeof(T)}\" key \"{key}\"");
                    }
                }
                else
                {
                    Debug.LogError($"Restore failed. Obj by Key: \"{key}\" is null");
                }
            }
            else
            {
                Debug.LogError($"Restore failed. Key: \"{key}\" not found");
            }
            return default;
        }
        public void SetData<T>(string key, T data)
        {
            if (!Data.ContainsKey(key))
            {
                Data.Add(key, data);
            }
            else
            {
                Debug.LogError($"Save data failed. Key: \"{key}\" already added");
            }
        }
    }
}
