using UnityEditor.Experimental.GraphView;

namespace DialogEditor
{
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
}
