using UnityEditor.Experimental.GraphView;

namespace DialogEditor
{
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
