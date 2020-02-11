using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public interface ISpeech
    {
        string GetSpeechKey();
        ISpeech GetNextSpeech();
        void OnSelect();
    }
}
