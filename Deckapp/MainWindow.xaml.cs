using System;
using System.Windows;
using System.Windows.Media;
using CoreAudio;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Numerics;


namespace Deckapp
{
    public partial class MainWindow : Window
    {
        public static List<AudioSessionControl2> Sessions = new List<AudioSessionControl2>();

        public MainWindow()
        {
            InitializeComponent();
            CompositionTarget.Rendering += Main;
            

            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator(Guid.NewGuid());
            MMDevice device = DevEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            foreach (var session in device.AudioSessionManager2.Sessions)
            {
                if (!session.IsSystemSoundsSession)
                {
                    Sessions.Add(session);
                    Console.WriteLine(Process.GetProcessById((int)session.ProcessID));
                }
            }
            Fader fader0 = new Fader(MainCanvas, 0);
            Fader.Faders.Add(fader0);
            Fader fader1 = new Fader(MainCanvas, 1);
            Fader.Faders.Add(fader1);
            Fader fader2 = new Fader(MainCanvas, 2);
            Fader.Faders.Add(fader2);
            Fader fader3 = new Fader(MainCanvas, 3);
            Fader.Faders.Add(fader3);

            for (int i = 0; i < 12; i++)
            {
                DeckButton button = new DeckButton(MainCanvas, i);
            }

            SendKeys.SendWait("k");

            //SerialCom.StartSerialCom();
        }
        private void Main(object sender, EventArgs e)
        {
            Fader.allFadersMain();
        }
    }
}