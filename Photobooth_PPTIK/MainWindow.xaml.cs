using System.Windows;
using MahApps.Metro.Controls;

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
