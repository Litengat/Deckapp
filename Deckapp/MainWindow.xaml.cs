using System;
using System.Windows;
using System.Windows.Media;
using CoreAudio;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Numerics;
using System.Configuration;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Sources;
using System.Drawing;
using Microsoft.VisualBasic.ApplicationServices;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Windows.Media.Imaging;


namespace Deckapp
{
    public partial class MainWindow : Window
    {
        NotifyIcon nIcon = new NotifyIcon();

        public static List<AudioSessionControl2> Sessions = new List<AudioSessionControl2>();
        bool CancelExit = true;
        public static List<string> ProcessNames = new List<string>();
        MMDeviceEnumerator DevEnum;

        static string registryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath, true);

        string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public MainWindow()
        {
            InitializeComponent();
            CompositionTarget.Rendering += Main;
            Closing += Window_Closing;

            this.Visibility = Visibility.Hidden;
            createNIcon();
            //Icon = BitmapFrame.Create(new Uri(path + "/Icons/icon.ico", UriKind.RelativeOrAbsolute));

            DevEnum = new MMDeviceEnumerator(Guid.NewGuid());
            Timer updateSessionTimer = new Timer();
            updateSessionTimer.Tick += new EventHandler(delegate (Object o, EventArgs a)
            {
                updateSessions(false);
            });
            updateSessionTimer.Interval = 1000; // in miliseconds
            updateSessionTimer.Start();
            updateSessions(true);

            Fader.createFaders(MainCanvas);
            saveFaders();
            DeckButton.createButtons(MainCanvas);
            saveButtons();
            SerialCom.StartSerialCom();
        }


        private void NIcon_Click(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                if(Visibility == Visibility.Hidden)
                {
                    Visibility = Visibility.Visible;
                }else
                {
                    Visibility = Visibility.Hidden;
                }
            }
        }
        private void createNIcon()
        {
            nIcon.Icon = new Icon(path + "/Icons/icon.ico"); ;
            nIcon.Visible = true;
            nIcon.MouseClick += NIcon_Click;

            nIcon.ContextMenuStrip = new ContextMenuStrip();

            nIcon.ContextMenuStrip.Items.Add("Exit", null, (object sender, EventArgs e) =>
            {
                CancelExit = false;
                Close();
            });
            nIcon.ContextMenuStrip.Items.Add("StartUP", null, (object sender, EventArgs e) =>
            {
                if (nIcon.ContextMenuStrip.Items[1].Image is null)
                {
                    nIcon.ContextMenuStrip.Items[1].Image = new Icon(path + "/Icons/check.ico").ToBitmap();
                    key.SetValue("DeckApp", System.Windows.Forms.Application.ExecutablePath.ToString());
                }
                else
                {
                    nIcon.ContextMenuStrip.Items[1].Image = null;
                    key.DeleteValue("DeckApp",false);
                }
            });

        }





        private void Main(object sender, EventArgs e)
        {

            if(Visibility == Visibility.Visible) {
                Fader.allFadersMain();
            }
        }

        private void Window_Closing(object sender,CancelEventArgs e)
        {
            e.Cancel = CancelExit;
            Visibility = Visibility.Hidden;
        }



        public static void saveFaders()
        {
            string FaderValues = "";
            foreach (var fader in Fader.Faders)
            {
                FaderValues += Process.GetProcessById((int)fader.selectedSession.ProcessID).ProcessName + "|";
            }
            Properties.Settings.Default.FaderValues = FaderValues;
            Properties.Settings.Default.Save();
        }

        public static void saveButtons()
        {
            string ButtonsPaths = "";
            foreach (var button in DeckButton.Buttons)
            {
                ButtonsPaths += button.path + "|";
            }
            Properties.Settings.Default.ButtonsPaths = ButtonsPaths;
            Properties.Settings.Default.Save();
        }
        public static AudioSessionControl2 getSessionByName(string name)
        {
            for (int j = 0; j < Sessions.Count; j++)
            {
                AudioSessionControl2 session = Sessions[j];
                if (Process.GetProcessById((int)session.ProcessID).ProcessName == name)
                {
                    return session;
                }
            }
            return Sessions[0];
        }

        private void updateSessions(bool skip)
        {
            if(Visibility == Visibility.Visible | skip)
            {
                MMDevice device = DevEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                Sessions.Clear();
                ProcessNames.Clear();
                foreach (var session in device.AudioSessionManager2.Sessions)
                {
                    if (!session.IsSystemSoundsSession)
                    {
                        Sessions.Add(session);
                        Process p = Process.GetProcessById((int)session.ProcessID);
                        ProcessNames.Add(p.ProcessName);
                    }
                }
            }
        
        }

    }
}