using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogEditor
{
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
}
