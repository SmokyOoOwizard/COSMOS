using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public class DialogStage
    {
        public DialogAction CurrentSpeech { get; protected set; }
        DialogStage nextStage;

        public void Next()
        {
            DialogAction nextSpeech = CurrentSpeech.GetNextAction();
            if(nextSpeech == null || CurrentSpeech is EndDialogDialogAction)
            {
                return;
            }
            if(CurrentSpeech is SwitchDialogAction)
            {
                nextSpeech = nextSpeech.GetNextAction();
            }
            CurrentSpeech = nextSpeech;
        }
        public bool IsFinished()
        {
            return CurrentSpeech.GetNextAction() == null || CurrentSpeech is EndDialogDialogAction;
        }
        public DialogStage GetNextStage()
        {
            return nextStage;
        }
    }
}
