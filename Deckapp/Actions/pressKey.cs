﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;

namespace Deckapp.Actions
{
    class PressKey : Action
    {
        string key;
        public PressKey(string[] args)
        {
            key = args[1];
        }

        public void run()
        {
            SendKeys.SendWait(key);
        }
    }
}
