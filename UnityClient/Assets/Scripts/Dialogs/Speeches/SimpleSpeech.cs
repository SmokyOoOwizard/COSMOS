﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public class SimpleSpeech : AbstractSpeech
    {
        public SimpleSpeech(string key)
        {
            LStringKey = key;
        }
    }
}