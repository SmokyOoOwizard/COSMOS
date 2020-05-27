using COSMOS.Dialogs;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogEditor
{
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
            data.Type = GetType().FullName;
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

        public virtual RawDialogNode Export()
        {
            var rawNode = new RawDialogNode();
            rawNode.GUID = GUID;
            return rawNode;
        }

    }
}
