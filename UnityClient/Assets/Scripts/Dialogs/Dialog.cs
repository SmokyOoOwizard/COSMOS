using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public class Dialog
    {
        DialogStage FirstStage;
        DialogStage CurrentStage;

        public DialogAction GetCurrentAction()
        {
            if(CurrentStage != null)
            {
                return CurrentStage.CurrentSpeech;
            }
            return null;
        }
        public void Next()
        {
            if(CurrentStage == null)
            {
                return;
            }
            if (CurrentStage.IsFinished())
            {
                CurrentStage = CurrentStage.GetNextStage();
            }
            else
            {
                CurrentStage.Next();
            }
        }
        public bool IsDialogFinished()
        {
            if(CurrentStage == null)
            {
                return true;
            }
            if(CurrentStage.IsFinished() && CurrentStage.GetNextStage() == null)
            {
                return true;
            }
            return CurrentStage.CurrentSpeech is EndDialogDialogAction;
        }
    }
}
