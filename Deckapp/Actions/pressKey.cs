using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;

namespace Deckapp.Actions
{
    class pressKey : Action
    {
        public string key { get; set; }
        public pressKey(string key)
        {
            this.key = key;
        }

        public void run()
        {
            SendKeys.SendWait(key);
            Console.WriteLine(key);
        }
    }
}
