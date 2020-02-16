using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public abstract class AbstractSpeech : IDialogAction
    {
        public string LStringKey;
        public IDialogAction NextSpeech;
        public virtual IDialogAction GetNextAction()
        {
            return NextSpeech;
        }

        public string GetSpeechKey()
        {
            return LStringKey;
        }

        public virtual void OnSelect()
        {

        }
    }
}
