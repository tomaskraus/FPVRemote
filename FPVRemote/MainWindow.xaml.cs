﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;
using IniParser;
using IniParser.Model;

using FPVRemote.RCSender;

using WPFMediaKit.DirectShow.Controls;

namespace FPVRemote
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int NUM_OF_CHANNELS = 4;

        private DispatcherTimer _inputCheckTimer;
        private VideoCaptureElement _frontView;

        private short[] inputResults;

        SerialRCSender rcSender;

        public MainWindow()
        {
            

            Loaded += MainWindow_Loaded;

            InitializeComponent();
        }


        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("Loaded");

            try
            {

                var Parser = new FileIniDataParser();
                IniData data = Parser.ReadFile("config.ini");

                rcSender = new SerialRCSender(NUM_OF_CHANNELS).InitFromConfig(data, "RC");
                inputResults = new short[rcSender.NumOfChannels];
                initInputControls(data, ref inputResults);                              

                bordr.Width = bordrR.w;
                bordr.Height = bordrR.h;
                Canvas.SetLeft(bordr, this.bordrR.x);
                Canvas.SetTop(bordr, this.bordrR.y);

                centr.Width = centrR.w;
                centr.Height = centrR.h;
                Canvas.SetLeft(centr, this.bordrR.x + (this.bordrR.w - this.centrR.w) / 2);
                Canvas.SetTop(centr, this.bordrR.y + (this.bordrR.h - this.centrR.h) / 2);

                deadzone.Width = deadZoneR.w;
                deadzone.Height = deadZoneR.h;
                Canvas.SetLeft(deadzone, this.bordrR.x + (this.bordrR.w - this.deadZoneR.w) / 2);
                Canvas.SetTop(deadzone, this.bordrR.y + (this.bordrR.h - this.deadZoneR.h) / 2);


                StartNewInputCheckTimer();

                // Start the camera feeds
                StartAllCameras(data, "FPV");

            } catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Application.Current.Shutdown();
            }
        }

        private void StartNewInputCheckTimer()
        {
            _inputCheckTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 50),
                IsEnabled = true
            };

            _inputCheckTimer.Tick += InputCheckTimerOnTick;
            _inputCheckTimer.Start();
        }

        private void InputCheckTimerOnTick(object sender, EventArgs eventArgs)
        {
            loopInputControls(ref inputResults);    
            rcSender.sendValues(inputResults);
        }


        private void XAxisTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void XAxisTextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }


        // -- FPV ----------------------------------------------------------------------------

        public void StartAllCameras(IniData configData, string key)
        {
            int cameraIndex = int.Parse(configData[key]["cameraId"]);
            StartCamera(ref _frontView, 480, 240, 15, cameraIndex);
        }

        private void StartCamera(ref VideoCaptureElement camera, int width, int height, int fps, int deviceIndex)
        {
            if (deviceIndex >= MultimediaUtil.VideoInputDevices.Length || deviceIndex < 0)
            {
                // Invalid device index should be ignored
                return;
            }

            // Initialize the element
            camera = new VideoCaptureElement
            {
                DesiredPixelWidth = width,
                DesiredPixelHeight = height,
                FPS = fps,
                VideoCaptureDevice = MultimediaUtil.VideoInputDevices[deviceIndex]
            };

            camera.BeginInit();
            camera.EndInit();

            // Add the control to layout
            camera.Width = CameraCanvas.Width;
            camera.Height = CameraCanvas.Height;
            CameraCanvas.Children.Add(camera);

            // start the camera stream
            camera.Play();
        }

    }
}
