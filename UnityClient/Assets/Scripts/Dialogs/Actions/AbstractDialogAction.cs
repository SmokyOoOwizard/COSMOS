﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public abstract class AbstractDialogAction : DialogAction
    {
        protected DialogAction nextAction;
        public virtual DialogAction GetNextAction()
        {
            return nextAction;
        }

        public virtual void OnSelect()
        {
            
        }
    }
}
