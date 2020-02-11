using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public abstract class AbstractSpeech : ISpeech
    {
        public string LStringKey;
        public ISpeech NextSpeech;
        public virtual ISpeech GetNextSpeech()
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
