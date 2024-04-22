using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Deckapp.Actions
{
    internal class Mouse : Action
    {
        int x;  
        int y;

        public Mouse(string[] args) 
        {
            x = int.Parse(args[1]);
            y = int.Parse(args[2]);
        }

        public void run()
        {
            Cursor.Position = new Point(Cursor.Position.X + x, Cursor.Position.Y + y);
        }

    }
}
