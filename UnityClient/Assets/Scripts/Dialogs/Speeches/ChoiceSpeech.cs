using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public class ChoiceSpeech : AbstractSpeech
    {
        List<SpeechOption> Options;
        
        public void Choose(ISpeech speech)
        {

        }

        public ISpeech[] GetChoice()
        {
            var enabledOprtions = Options.Where((x) => x.CheckConditions()).Select((x) => x.Speech).ToArray();
            return enabledOprtions;
        }
    }
    public class SpeechOption
    {
        public ISpeech Speech;

        public bool CheckConditions()
        {
            return true;
        }
    }
}
