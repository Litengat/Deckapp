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
                Console.WriteLine(line);
                string[] words = line.Split(' ');
                if (words[0] == "key")
                {
                    Console.WriteLine(words[1]);
                    pressKey key = new pressKey(words[1]);
                    actions.Add(key);
                }
            }
            this.actions = actions;
        }

        public void run()
        {
            foreach (Action action in actions)
            {
                action.run();
            }
        }

    }
}
