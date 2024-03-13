using System.Text;
using System.Windows;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using CoreAudio;
using System.Diagnostics;
using System.Reflection;

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

            SerialCom.StartSerialCom();
        }

        private void Main(object sender, EventArgs e)
        {
            Fader.allFadersMain();
        }
    }
}