using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor
{
    public class GraphEditorView : GraphView
    {
        public GraphEditorView()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new ClickSelector());
            this.AddManipulator(new RectangleSelector());

            AddElement(Gener());
            AddElement(Gener());
        }

        private GraphNode Gener()
        {
            var node = new GraphNode()
            {
                title = "Start",
                GUID = Guid.NewGuid().ToString()
            };
            node.inputContainer.Add(
                node.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float)));
            node.outputContainer.Add(
                node.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float)));

            node.RefreshExpandedState();
            node.RefreshPorts();

            node.SetPosition(new Rect(0, 0, 100, 200));
            return node;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> p = new List<Port>();
            ports.ForEach((port) =>
            {
                if(port != startPort && startPort.node != port.node)
                {
                    p.Add(port);
                }
            });
            return p;
        }
    }
}
