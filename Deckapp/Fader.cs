using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using CoreAudio;

namespace Deckapp
{
    class Fader 
    {
        public static List<Fader> Faders = new List<Fader>();


        public Slider slider;
        public ComboBox comboBox;
        public ProgressBar progressBar;
        public Image image;
        public AudioSessionControl2 selectedSession { get; set; }
        int id;


        public Fader(Canvas canvas, int Faderid, AudioSessionControl2 session) 
        {
            id = Faderid;
            selectedSession = session;
            comboBox = createComboBox();
            canvas.Children.Add(comboBox);
            image = createImage();
            canvas.Children.Add(image);
            slider = createSlider();
            canvas.Children.Add(slider);
            progressBar = createProgressBar();
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
        public void setVolume(float volume)
        {
            selectedSession.SimpleAudioVolume.MasterVolume = volume;
        }
        public static void allFadersMain()
        {
            foreach(Fader fader in Faders)
            {
                fader.Main();
            }
        }
        public static void createFaders(Canvas canva,int count = 4)
        {
            string[] FaderValues = Properties.Settings.Default.FaderValues.Split('|', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < count; i++)
            {
                Fader fader = new Fader(canva, i, MainWindow.getSessionByName(FaderValues[i]));
                Faders.Add(fader);
            }
        }


        ComboBox createComboBox()
        {
            comboBox = new System.Windows.Controls.ComboBox();
            comboBox.ItemsSource = MainWindow.ProcessNames;
            comboBox.SelectedIndex = MainWindow.Sessions.IndexOf(selectedSession);
            comboBox.SelectionChanged += ComboBox_SelectionChanged;
            comboBox.Width = 95;
            Canvas.SetTop(comboBox, 20);
            Canvas.SetLeft(comboBox, 75 + id * 200);
            Canvas.SetTop(comboBox, 20);
            return comboBox;
        }

        Image createImage()
        {
            image = new Image();
            image.Source = ProcessIcons.getIconFromProsses((int)selectedSession.ProcessID);
            image.Height = 20;
            image.Width = 20;
            Canvas.SetLeft(image, 50 + id * 200);
            Canvas.SetTop(image, 20);
            return image;
        }
        Slider createSlider()
        {
            slider = new Slider();
            slider.Minimum = 0; slider.Maximum = 1;
            slider.ValueChanged += Slider_ValueChanged;
            slider.Width = 120;
            Canvas.SetLeft(slider, 50 + id * 200);
            Canvas.SetTop(slider, 60);
            return slider;
        }
        ProgressBar createProgressBar()
        {
            progressBar = new ProgressBar();
            progressBar.Minimum = 0; progressBar.Maximum = 1;
            progressBar.Height = 10;
            progressBar.Width = 120;
            Canvas.SetLeft(progressBar, 50 + id * 200);
            Canvas.SetTop(progressBar, 90);
            return progressBar;
        }


        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            selectedSession.SimpleAudioVolume.MasterVolume = (float) slider.Value;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSession =  MainWindow.Sessions[comboBox.SelectedIndex];
            slider.Value = selectedSession.SimpleAudioVolume.MasterVolume;
            image.Source = ProcessIcons.getIconFromProsses((int)selectedSession.ProcessID);
            MainWindow.saveFaders();
        }

    }
}
