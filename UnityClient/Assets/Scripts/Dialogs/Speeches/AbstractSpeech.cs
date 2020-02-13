﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public abstract class AbstractSpeech : DialogAction
    {
        public string LStringKey;
        public DialogAction NextSpeech;
        public virtual DialogAction GetNextAction()
        {
            return NextSpeech;
        }

        public string GetSpeechKey()
        {
            return LStringKey;
        }

        public virtual void OnSelect()
        {

        }
    }
}
