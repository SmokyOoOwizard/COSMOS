using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public class ChoiceSpeech : AbstractSpeech
    {
        Dictionary<SwitchOption, DialogAction> Options;
        
        public void Choose(DialogAction speech)
        {
            if (Options.ContainsValue(speech))
            {
                NextSpeech = speech;
            }
        }

        public DialogAction[] GetChoice()
        {
            var enabledOprtions = Options.Where((x) => x.Key.CheckConditions()).Select((x) => x.Value).ToArray();
            return enabledOprtions;
        }
    }
}
