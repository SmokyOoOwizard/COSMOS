using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogEditor
{
    [Serializable]
    public class DialogNodeData
    {
        public string GUID;
        public Type Type;
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
    public abstract class DialogNode : Node
    {
        public virtual Vector2 DefaultSize { get; } = new Vector2(200, 100);
        public string GUID { get; protected set; }


        public DialogNode()
        {
            GUID = Guid.NewGuid().ToString();
            SetPosition(new Rect(Vector2.zero, DefaultSize));
        }
        public DialogNode(string GUID) : this()
        {
            this.GUID = GUID;
        }

        protected Port RenamePort(Port port, string name)
        {
            var lable = port.contentContainer.Q<Label>("type");
            lable.text = name;
            return port;
        }

        protected void RemovePort(Port port)
        {
            var edges = port.connections.ToArray();
            port.DisconnectAll();

            for (int i = 0; i < edges.Length; i++)
            {
                edges[i].RemoveFromHierarchy();
            }

            if (outputContainer.Contains(port))
            {
                outputContainer.Remove(port);
            }
            if (inputContainer.Contains(port))
            {
                inputContainer.Remove(port);
            }

            RefreshPorts();
            RefreshExpandedState();
        }

        public DialogNodeData GetData()
        {
            var data = new DialogNodeData();
            data.GUID = GUID;
            data.Rect = GetPosition();
            data.Type = GetType();
            SaveData(data);
            return data;
        }

        public void Restore(DialogNodeData data)
        {
            GUID = data.GUID;
            SetPosition(data.Rect);
            RestoreData(data);
        }

        protected virtual void SaveData(DialogNodeData data)
        {

        }

        protected virtual void RestoreData(DialogNodeData data)
        {

        }
    }
}
