using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public class DialogStage
    {
        public IDialogAction CurrentSpeech { get; private set; }
        DialogStage nextStage;

        public void Next()
        {
            IDialogAction nextSpeech = CurrentSpeech.GetNextAction();
            if(CurrentSpeech is EndDialogDialogAction)
            {
                return;
            }
            CurrentSpeech = nextSpeech;
        }
        public bool IsFinished()
        {
            return CurrentSpeech == null || CurrentSpeech is EndDialogDialogAction;
        }
        public DialogStage GetNextStage()
        {
            return nextStage;
        }
    }
}
