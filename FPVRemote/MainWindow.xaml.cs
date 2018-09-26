using System;
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

using FPVRemote.Joyinput;


namespace FPVRemote
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _inputCheckTimer;

        IJoyInput ji;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }


        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("Loaded");

            var Parser = new FileIniDataParser();
            IniData data = Parser.ReadFile("config.ini");

            // ji = new CountJoyInput().InitFromConfig(data, "COUNTJOY2")
            ji = new GamePadInput()
                .Chain(new LimitJoyInput(3500));





            StartNewInputCheckTimer();
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
            ji.Poll();
            XAxisTextBox.Text = ji.Values.x.ToString();
        }


        private void XAxisTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void XAxisTextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
