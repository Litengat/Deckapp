using Deckapp.Actions;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Deckapp
{
    class Macro
    {
        public List<Action> actions {get; set;}

        public Macro()
        {
            this.actions = new List<Action>();
        }

        public Macro(string file)
        {
            List<Action> actions = new List<Action>();
            string[] lines = file.Split('\n');
            foreach (string line in lines)
            {
                string[] words = line.Split(' ');
                if (words[0] == "key")
                {
                    actions.Add(new PressKey(words));
                }
                if (words[0] == "sleep")
                {
                    actions.Add(new Sleep(words));
                }
                if (words[0] == "mouse")
                {
                    actions.Add( new Mouse(words));
                }
            }
            this.actions = actions;
        }

        public void run()
        {
            Thread thread = new Thread(runThread);
            thread.Start();
        }
        public void runThread()
        {
            foreach (Action action in actions)
            {
                action.run();
            }
        }

    }
}
