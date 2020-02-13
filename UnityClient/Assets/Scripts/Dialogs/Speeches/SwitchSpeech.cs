using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public class SwitchSpeech : AbstractSpeech
    {
        List<SpeechOption> options;

        public override DialogAction GetNextAction()
        {
            DialogAction next = NextSpeech;
            for (int i = 0; i < options.Count; i++)
            {
                if (options[i].CheckConditions())
                {
                    next = options[i].Speech;
                    break;
                }
            }
            return next;
        }
    }
}
