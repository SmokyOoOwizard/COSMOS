using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public interface DialogAction
    {
        DialogAction GetNextAction();
        void OnSelect();
    }
}
