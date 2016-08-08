using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
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

namespace Photobooth_PPTIK.Pages
{
    /// <summary>
    /// Interaction logic for menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        public Menu()
        {
            InitializeComponent();   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var wnd = (MainWindow)Window.GetWindow(this);
            wnd.frame.Navigate(new CapturePage());
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var wnd = (MainWindow)Window.GetWindow(this);
            await wnd.ShowMessageAsync("This is the title", "Some message");
        }
    }
}
