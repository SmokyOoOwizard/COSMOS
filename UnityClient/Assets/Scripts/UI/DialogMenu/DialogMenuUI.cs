using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using COSMOS.Dialogs;

namespace COSMOS.UI.DialogMenu
{
    public class DialogMenuUI : MonoBehaviour
    {
        [SerializeField]
        GameObject Content;


        public static DialogMenuUI Instance { get; private set; }
        public Dialog CurrentDialog { get; private set; }
        public event Action<IDialogAction> DialogAction;

        void UpdateCurrentStage()
        {
            if(Content.transform.childCount > 0)
            {
                foreach (Transform child in Content.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            if (CurrentDialog.IsDialogFinished())
            {
                return;
            }
            IDialogAction action = CurrentDialog.GetCurrentAction();
            if(action is AbstractSpeech)
            {
                if(action is ChoiceSpeech)
                {
                    var choice = (action as ChoiceSpeech).GetChoice();
                    foreach (var item in choice)
                    {
                        DialogSpeechUI speech = DialogSpeechUI.Spawn(item);
                        speech.OnClick += OnChoiceSpeech;
                        speech.OnClick += (x) => OnClick();
                        speech.transform.SetParent(Content.transform);
                    }
                }
                else if(action is SimpleSpeech)
                {
                    DialogSpeechUI speech = DialogSpeechUI.Spawn(action as SimpleSpeech);
                    speech.OnClick += (x) => OnClick();
                    speech.transform.SetParent(Content.transform);
                }
            }
            else
            {
                while (action != null && action is AbstractDialogAction)
                {
                    CurrentDialog.Next();
                    action = CurrentDialog.GetCurrentAction();
                    if (action != null)
                    {
                        DialogAction?.Invoke(action);
                        action.OnSelect();
                    }
                    if (CurrentDialog.IsDialogFinished())
                    {
                        break;
                    }
                }
                UpdateCurrentStage();
            }
        }
        void OnClick()
        {
            IDialogAction action = CurrentDialog.GetCurrentAction();
            if (action is AbstractSpeech)
            {
                CurrentDialog.Next();
                action = CurrentDialog.GetCurrentAction();
                if(action != null)
                {
                    DialogAction?.Invoke(action);
                    action.OnSelect();
                }
            }
            UpdateCurrentStage();

        }
        void OnChoiceSpeech(DialogSpeechUI speech)
        {
            IDialogAction action = CurrentDialog.GetCurrentAction();
            if (action is AbstractSpeech)
            {
                if (action is ChoiceSpeech)
                {
                    (action as ChoiceSpeech).Choose(speech.CurrentSpeech);
                }
            }
        }

        public static void CloseMenu()
        {
            Instance = null;
        }
        public static DialogMenuUI SpawnMenu(Dialog dialog)
        {
#warning NEED COMPLETE
            return null;
        }
    }
}
