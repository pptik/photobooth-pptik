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
using MahApps.Metro.Controls;
using System.Windows.Media.Animation;
using Microsoft.Kinect.Wpf.Controls;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using System.Windows.Resources;
using MahApps.Metro.Controls.Dialogs;
using Photobooth_PPTIK.Pages;

namespace Photobooth_PPTIK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new CapturePage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            frame.GoBack();
        }
    }

}
