﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogEditor
{
    [Serializable]
    public class DialogEditorContainer : ScriptableObject
    {
        public List<DialogNodeData> Nodes = new List<DialogNodeData>();
    }
    public class DialogEditorView : GraphView
    {
        private NodeSearchWindow searchWindow;
        private DialogEditorWindow editorWindow;

        public DialogEditorView(DialogEditorWindow editorWindow)
        {
            this.editorWindow = editorWindow;

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new ClickSelector());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            AddElement(new EntryNode());

            addSearchWindow();
        }

        private void addSearchWindow()
        {
            searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
            searchWindow.Init(editorWindow, this);
        }

        public void Load(DialogEditorContainer dialog)
        {
            Dictionary<string, DialogNode> nodes = new Dictionary<string, DialogNode>();

            foreach (var node in dialog.Nodes)
            {
                try
                {
                    if (node.Type != null)
                    {
                        if (node.Type.IsSubclassOf(typeof(DialogNode)))
                        {
                            var instance = Activator.CreateInstance(node.Type) as DialogNode;

                            (instance).Restore(node);

                            AddElement(instance);

                            nodes.Add(node.GUID, instance);
                        }
                    }
                    else
                    {
                        Debug.LogError("Restore node failed. type is null");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }

        }
        public DialogEditorContainer Save()
        {
            var container = ScriptableObject.CreateInstance<DialogEditorContainer>();
            var nodes = this.nodes.ToList();
            foreach (var node in nodes)
            {
                if(node is DialogNode)
                {
                    var data = (node as DialogNode).GetData();
                    container.Nodes.Add(data);
                }
            }
            return container;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> p = new List<Port>();
            ports.ForEach((port) =>
            {
                if (port != startPort && startPort.node != port.node)
                {
                    p.Add(port);
                }
            });
            return p;
        }
    }
}
