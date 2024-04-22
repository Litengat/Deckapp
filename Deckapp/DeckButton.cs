using Deckapp.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Deckapp
{
    class DeckButton
    {
        public static List<DeckButton> Buttons = new List<DeckButton>();

        int id;
        Button button;
        Macro macro;
        bool ismacro = true;
        public string path { get; set; }

        public DeckButton(Canvas canvas,int id,string path)
        {
            this.id = id;

            button = new Button();
            button.Content = id;
            button.Click += Button_Click;
            button.MouseRightButtonDown += RightButton_Click;
            Canvas.SetLeft(button,id / 3 * 50 + 100);
            Canvas.SetTop(button,200 +( id % 3) * 50);
            canvas.Children.Add(button);

            if (File.Exists(path))
            {
                this.path = path;
                this.macro = pathtoMacro(path);
            } else 
            {
                macro = new Macro();
                List<Action> actions = new List<Action>();
                macro.actions = actions;
            }
        }
        public static void createButtons(Canvas canvas, int count = 12)
        {
            string[] ButtonsPaths = Properties.Settings.Default.ButtonsPaths.Split('|');
            for (int i = 0; i < count; i++)
            {
                DeckButton button = new DeckButton(canvas, i, ButtonsPaths[i]);
                Buttons.Add(button);
            }
        }

        public void setMacro( Macro macro )
        {
            this.macro = macro;
        }

        Macro pathtoMacro(string path)
        {
            Console.WriteLine(File.ReadAllText(path));
            return new Macro(File.ReadAllText(path));
        }

        private void RightButton_Click(object sender, MouseButtonEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".deckmacro";
            dlg.Filter = "Macros (*.deckmacro)|*.deckmacro|Python Scripts (*.py)|*.py";
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                path = dlg.FileName;
                if (path.EndsWith(".deckmacro"))
                {
                    macro = pathtoMacro(path);
                }else if (path.EndsWith(".py"))
                {
                    ismacro = false;
                }
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(100);
            press();
        }
        public void press()
        {
            if (ismacro) {
                macro.run();
            }
            else
            {
                ScriptEngine engine = Python.CreateEngine();
                engine.ExecuteFile(path);
            }
        }


    }
}
