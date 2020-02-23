using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public class SwitchDialogAction : AbstractDialogAction
    {
        Dictionary<SwitchOption, IDialogAction> options = new Dictionary<SwitchOption, IDialogAction>();

        public override IDialogAction GetNextAction()
        {
            foreach (var option in options)
            {
                if (option.Key.CheckConditions())
                {
                    nextAction = option.Value;
                    break;
                }
            }
            return base.GetNextAction();
        }
    }
}
