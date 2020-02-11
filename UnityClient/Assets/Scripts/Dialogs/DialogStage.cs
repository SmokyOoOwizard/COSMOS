using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public class DialogStage
    {
        public ISpeech SelectedSpeech;
        public ISpeech CurrentSpeech;

        public void Next()
        {
            ISpeech nextSpeech = CurrentSpeech.GetNextSpeech();
            if(nextSpeech == null || CurrentSpeech is EndSpeech)
            {
                return;
            }
            CurrentSpeech = nextSpeech;
        }
        public bool IsFinished()
        {
            return CurrentSpeech.GetNextSpeech() == null || CurrentSpeech is EndSpeech;
        }
        public DialogStage GetNextStage()
        {
            return null;
        }
    }
}
