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

    public class EntryNode : DialogNode
    {

        public EntryNode()
        {
            title = "Entry Node";

            outputContainer.Add(
                RenamePort(
                InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(string)),
                "Flow"));
        }
    }
    public abstract class SimpleNode : DialogNode
    {
        public SimpleNode()
        {
            inputContainer.Add(
                RenamePort(
                    InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(string)),
                    "Flow"));

            outputContainer.Add(
                RenamePort(
                InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(string)),
                "Flow"));
        }
    }
    public class SpeechNode : SimpleNode
    {
        public string Text { get; protected set; }
        
        private TextField TextField;

        public SpeechNode()
        {
            title = "Speech Node";

            TextField = new TextField();
            TextField.multiline = true;
            TextField.SetValueWithoutNotify(title);

            TextField.RegisterValueChangedCallback(evt =>
            {
                Text = evt.newValue;
            });
            mainContainer.Add(TextField);
        }

        protected override void SaveData(DialogNodeData data)
        {
            base.SaveData(data);
            data.Data.Add("Speech", Text);
        }
        protected override void RestoreData(DialogNodeData data)
        {
            base.RestoreData(data);
            if (data.Data.ContainsKey("Speech"))
            {
                object obj = data.Data["Speech"];
                if(obj is string)
                {
                    TextField.SetValueWithoutNotify(obj as string);
                    Text = obj as string;
                }
                else
                {
                    Debug.LogError("Restore failde. Key: Speech - Wrong type");
                }
            }
            else
            {
                Debug.LogError("Restore failed. Key Speech not found");
            }
        }
    }
    public class ChoiceNode : DialogNode
    {
        public ChoiceNode()
        {
            title = "Choice Node";

            inputContainer.Add(
                InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(string)));

            var button = new Button(() => AddChoice())
            {
                text = "Add choice"
            };
            titleContainer.Add(button);
        }

        public void AddChoice()
        {
            var port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(string));
            var portLabel = port.contentContainer.Q<Label>("type");
            portLabel.text = "  ";
            //port.contentContainer.Remove(portLable);

            var txtField = new TextField();
            txtField.SetValueWithoutNotify("Choice");

            var deleteButton = new Button(() => RemovePort(port))
            {
                text = "X"
            };

            port.contentContainer.Add(deleteButton);
            port.contentContainer.Add(new Label("   "));
            port.contentContainer.Add(txtField);
            outputContainer.Add(port);

            RefreshPorts();
            RefreshExpandedState();
        }
    }
    public abstract class CheckNode : DialogNode
    {
        public CheckNode()
        {

            inputContainer.Add(
                InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(string)));

            outputContainer.Add(
                InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(string)));
            outputContainer.Add(
                InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(string)));
        }
    }
}
