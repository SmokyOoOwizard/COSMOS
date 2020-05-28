using UnityEditor.Experimental.GraphView;

namespace DialogEditor
{
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
}
