using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Deckapp.Actions
{
    internal class Sleep : Action
    {
        int time;

        public Sleep(string[] args)
        {
            time = int.Parse(args[1]);
        }
        public void run()
        {
            Thread.Sleep(time);
        }
    }
}
