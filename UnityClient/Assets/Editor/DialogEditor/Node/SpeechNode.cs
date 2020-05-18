using UnityEngine;
using UnityEngine.UIElements;

namespace DialogEditor
{
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
            Text = title;

            TextField.RegisterValueChangedCallback(evt =>
            {
                Text = evt.newValue;
            });
            mainContainer.Add(TextField);
        }

        protected override void SaveData(DialogNodeData data)
        {
            base.SaveData(data);
            data.SetData("Speech", Text);
        }
        protected override void RestoreData(DialogNodeData data)
        {
            base.RestoreData(data);
            var speech = data.GetData<string>("Speech");
            TextField.SetValueWithoutNotify(speech);
            Text = speech;
        }
    }
}
