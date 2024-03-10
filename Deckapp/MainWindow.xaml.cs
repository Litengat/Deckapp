using System.Text;
using System.Windows;
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

namespace Deckapp
{
    public partial class MainWindow : Window
    {

        static List<AudioSessionControl2> Sessions = new List<AudioSessionControl2>();
        public static AudioSessionControl2 selectedSession = null;
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
                        Process p = Process.GetProcessById((int)session.ProcessID);
                        testSelect.Items.Add(p.ProcessName);
                    }
            }
            SerialCom serialCom = new SerialCom();
        }

        private void Main(object sender, EventArgs e)
        {
            MasterVolume.Value = selectedSession.AudioMeterInformation.MasterPeakValue;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            selectedSession.SimpleAudioVolume.MasterVolume = (float) testSlider.Value;
        }
            
        private void testSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSession = Sessions[testSelect.SelectedIndex];
            testSlider.Value = selectedSession.SimpleAudioVolume.MasterVolume;
        }
    }
}