using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public abstract class AbstractDialogAction : IDialogAction
    {
        protected IDialogAction nextAction;
        public virtual IDialogAction GetNextAction()
        {
            return nextAction;
        }

        public virtual void OnSelect()
        {
            
        }
    }
}
