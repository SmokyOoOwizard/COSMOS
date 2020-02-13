using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public abstract class AbstractDialogAction : DialogAction
    {
        public virtual DialogAction GetNextAction()
        {
            throw new NotImplementedException();
        }

        public virtual void OnSelect()
        {
            throw new NotImplementedException();
        }
    }
}
