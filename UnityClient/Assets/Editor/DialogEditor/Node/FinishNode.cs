using UnityEditor.Experimental.GraphView;

namespace DialogEditor
{
    public class FinishNode : DialogNode
    {
        public FinishNode()
        {
            title = "Finish Node";

            inputContainer.Add(
                RenamePort(
                InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(string)),
                "Flow"));
        }
    }
}
