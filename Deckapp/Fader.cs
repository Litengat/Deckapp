using CoreAudio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Deckapp
{
    class Fader
    {
        public static List<Fader> Faders = new List<Fader>();

        public Slider slider;
        public ComboBox comboBox;
        public ProgressBar progressBar;
        public Image image;
        public AudioSessionControl2 selectedSession;
        int id;

        public Fader(Canvas canvas, int Faderid) 
        {
            id = Faderid;
            selectedSession = MainWindow.Sessions[id];

            createComboBox();
            canvas.Children.Add(comboBox);
            createImage();
            canvas.Children.Add(image);
            createSlider();
            canvas.Children.Add(slider);
            createProgressBar();
            canvas.Children.Add(progressBar);
        }
        void Main()
        {
            progressBar.Value = selectedSession.AudioMeterInformation.MasterPeakValue;
            if(slider.Value != selectedSession.SimpleAudioVolume.MasterVolume)
            {
                slider.Value = selectedSession.SimpleAudioVolume.MasterVolume;
            }
        }
        
        public static void allFadersMain()
        {
            foreach(Fader fader in Faders)
            {
                fader.Main();
            }
        }


        void createComboBox()
        {
            comboBox = new ComboBox();
            foreach (var session in MainWindow.Sessions)
            {
                Process p = Process.GetProcessById((int)session.ProcessID);
                comboBox.Items.Add(p.ProcessName);
            }
            comboBox.SelectedIndex = id;
            comboBox.SelectionChanged += ComboBox_SelectionChanged;
            comboBox.Width = 95;
            Canvas.SetTop(comboBox, 20);
            Canvas.SetLeft(comboBox, 75 + id * 200);
            Canvas.SetTop(comboBox, 20);
        }
        void createImage()
        {
            image = new Image();
            image.Source = ProcessIcons.getIconFromProsses((int)selectedSession.ProcessID);
            image.Height = 20;
            image.Width = 20;
            Canvas.SetLeft(image, 50 + id * 200);
            Canvas.SetTop(image, 20);
        }
        void createSlider()
        {
            slider = new Slider();
            slider.Minimum = 0; slider.Maximum = 1;
            slider.ValueChanged += Slider_ValueChanged;
            slider.Width = 120;
            Canvas.SetLeft(slider, 50 + id * 200);
            Canvas.SetTop(slider, 60);
        }
        void createProgressBar()
        {
            progressBar = new ProgressBar();
            progressBar.Minimum = 0; progressBar.Maximum = 1;
            progressBar.Height = 10;
            progressBar.Width = 120;
            Canvas.SetLeft(progressBar, 50 + id * 200);
            Canvas.SetTop(progressBar, 90);
        }


        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            selectedSession.SimpleAudioVolume.MasterVolume = (float) slider.Value;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSession = MainWindow.Sessions[comboBox.SelectedIndex];
            slider.Value = selectedSession.SimpleAudioVolume.MasterVolume;
            image.Source = ProcessIcons.getIconFromProsses((int)selectedSession.ProcessID);
        }

    }
}
