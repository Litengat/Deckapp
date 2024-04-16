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

namespace Deckapp
{
    class DeckButton
    {
        int id;
        Button button;
        Macro macro;
        String path;

        public DeckButton(Canvas canvas,int id)
        {
            this.id = id;

            button = new Button();
            button.Content = id;
            button.Click += Button_Click;
            button.MouseRightButtonDown += RightButton_Click;
            Canvas.SetLeft(button,id / 3 * 50 + 100);
            Canvas.SetTop(button,200 +( id % 3) * 50);
            canvas.Children.Add(button);
            
            macro = new Macro();
            List<Action> actions = new List<Action>();
            actions.Add(new pressKey("a"));
            macro.actions = actions;
        }

        public void setMacro( Macro macro )
        {
            this.macro = macro;
        }
        public void pathtoMacro()
        {
            StreamReader sr = new StreamReader(path);
            this.macro = new Macro( sr.ReadToEnd());
            sr.Close();
        }

        private void RightButton_Click(object sender, MouseButtonEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".deckmacro";
            dlg.Filter = "Macros (*.deckmacro)|*.deckmacro";
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                path = dlg.FileName;
                pathtoMacro();
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("hi");
            Thread.Sleep(1000);
            macro.run();
            Console.WriteLine(macro.actions);
        }


    }
}
