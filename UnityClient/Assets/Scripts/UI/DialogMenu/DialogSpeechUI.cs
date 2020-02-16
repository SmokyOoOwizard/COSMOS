using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using COSMOS.Dialogs;
using TMPro;
using UnityEngine.UI;

namespace COSMOS.UI.DialogMenu
{
    public class DialogSpeechUI : MonoBehaviour
    {
        public event Action<DialogSpeechUI> OnClick;
        public SimpleSpeech CurrentSpeech { get; private set; }
        [SerializeField]
        TextMeshProUGUI text;
        [SerializeField]
        Button button;

        private void Start()
        {
            button.onClick.AddListener(() => OnClick?.Invoke(this));
        }

        void UpdateDialog(SimpleSpeech speech)
        {
            CurrentSpeech = speech;
            text.text = speech.LStringKey;
        }
        public static DialogSpeechUI Spawn(SimpleSpeech speech)
        {
            GameObject prefab = AssetsDatabase.LoadGameObject(@"Prefabs\UI\Dialog\Speech");
            GameObject go = Instantiate(prefab);
            DialogSpeechUI ds = go.GetComponent<DialogSpeechUI>();
            ds.UpdateDialog(speech);
            return ds;
        }
    }
}
