using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace LogMyTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer t;
        DateTime start;
        public MainWindow()
        {
            InitializeComponent();
            btnStop.Visibility = Visibility.Hidden;
            txtTimer.Visibility = Visibility.Hidden;
            t = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 50), DispatcherPriority.Background,t_Tick, Dispatcher.CurrentDispatcher); t.IsEnabled = true;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            start = DateTime.Now;
            btnStart.Visibility = Visibility.Hidden;
            btnStop.Visibility = Visibility.Visible;
            txtTimer.Visibility = Visibility.Visible;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            btnStop.Visibility=Visibility.Hidden;
            btnStart.Visibility = Visibility.Visible;
            txtTimer.Visibility = Visibility.Hidden;
        }

        private void t_Tick(object sender, EventArgs e)
        {
            txtTimer.Text = Convert.ToString(DateTime.Now - start);
        }
    }
}
