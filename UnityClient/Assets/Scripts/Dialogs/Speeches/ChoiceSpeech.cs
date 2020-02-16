using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public class ChoiceSpeech : AbstractSpeech
    {
        Dictionary<SwitchOption, SimpleSpeech> Options = new Dictionary<SwitchOption, SimpleSpeech>();
        
        public void Choose(SimpleSpeech speech)
        {
            if (Options.ContainsValue(speech))
            {
                NextSpeech = speech.GetNextAction();
            }
        }

        public SimpleSpeech[] GetChoice()
        {
            var enabledOprtions = Options.Where((x) => x.Key.CheckConditions()).Select((x) => x.Value).ToArray();
            return enabledOprtions;
        }
    }
}
