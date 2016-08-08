using System;
using System.Linq;
using System.Windows;
using LightBuzz.Vitruvius;
using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.IO;
using System.Diagnostics;
using Photobooth_PPTIK.Helper;

namespace Photobooth_PPTIK
{
    /// <summary>
    /// Interaction logic for CapturePage.xaml
    /// </summary>
    public partial class CapturePage : Page
    {
        private KinectSensor _sensor;
        private MultiSourceFrameReader _reader;

        private int currentElement = 0;
        private DispatcherTimer _timer;
        private TimeSpan _time;
        private int nFrame = 0;

        //number
        private Uri[] numbers = {
           new Uri(@"/Photobooth_PPTIK;component/Resources/Icon/number-0.png", UriKind.Relative),
           new Uri(@"/Photobooth_PPTIK;component/Resources/Icon/number-1.png", UriKind.Relative),
           new Uri(@"/Photobooth_PPTIK;component/Resources/Icon/number-2.png", UriKind.Relative),
           new Uri(@"/Photobooth_PPTIK;component/Resources/Icon/number-3.png", UriKind.Relative),
           new Uri(@"/Photobooth_PPTIK;component/Resources/Icon/number-4.png", UriKind.Relative),
           new Uri(@"/Photobooth_PPTIK;component/Resources/Icon/number-5.png", UriKind.Relative)
        };
       

        public CapturePage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
            _time = TimeSpan.FromSeconds(5);
       
            var uriSource1 = new Uri(@"/Photobooth_PPTIK;component/Resources/Frame/frame_1.png", UriKind.Relative);
            var uriSource2 = new Uri(@"/Photobooth_PPTIK;component/Resources/Frame/frame_2.png", UriKind.Relative);

            nFrame = 4; //jumlah frame
            frame1.Source = new BitmapImage(uriSource1);
            frame2.Source = new BitmapImage(uriSource2);
            frame3.Source = new BitmapImage(uriSource1);
            frame4.Source = new BitmapImage(uriSource2);

        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();
                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body | FrameSourceTypes.Depth);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            }
        }

        private void SwipeLeft()
        {
            if (currentElement < nFrame - 1)
            {
                currentElement++;
                AnimateCarousel();
            }
        }

        private void SwipeRight()
        {
            if (currentElement > 0)
            {
                currentElement--;
                AnimateCarousel();
            }
        }

        private void AnimateCarousel()
        {
            //var gapValue = 1024; //sesuaikan dengan lebar/width aplikasi
            var gapValue = 600;
            Canvas c = this.Resources["canvas"] as Canvas;
            Storyboard storyboard = (this.Resources["CarouselStoryboard"] as Storyboard);
            DoubleAnimation animation = storyboard.Children.First() as DoubleAnimation;
            Console.WriteLine("trans: " + (this.ActualWidth));
            animation.To = ((-(gapValue * 2) * currentElement) / 2);
            storyboard.Begin();
        }

        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Color
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (viewer.Visualization == Visualization.Color)
                    {
                        viewer.Image = frame.ToBitmap();
                    }
                }
            }

        }

        private void CaptureBtn_Click(object sender, RoutedEventArgs e)
        {
            CaptureBtn.Visibility = Visibility.Hidden;
            //for (int cptr = 0; cptr < 4; cptr++) {
                Capture();
            //} 
        }

        private void Capture()
        {

            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {      
                if (_time.Seconds == 0)
                {
                    _timer.Stop();
                    _time = TimeSpan.FromSeconds(6);
                    countImage.Visibility = Visibility.Hidden;
                    SavePhoto();
                }
                else
                {
                    countImage.Visibility = Visibility.Visible;
                    countImage.BeginInit();
                    countImage.Source = new BitmapImage(numbers[_time.Seconds]);
                    countImage.EndInit();
                }

                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);

            _timer.Start();
                        
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            //SwipeLeft();
            SwipeRight();
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            //SwipeRight();
            SwipeLeft();
        }

        private void SavePhoto()
        {
            string ext = ".jpg";
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsEpoch = (int)t.TotalSeconds;
            string n = secondsEpoch.ToString();

            //string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "photoboothbsg_" + n + ext);
          
            string appPath = @"" + Directory.GetCurrentDirectory() + "/images";
            Directory.CreateDirectory(appPath);
            string path = Path.Combine(appPath, "photobooth_" + n + ext);
            Console.WriteLine("Path: "+path);

            util.SaveCanvas(this, this.canvas, 96, path);
            CaptureBtn.Visibility = Visibility.Visible;

            //go to result page
            var page = new ViewPage(path);
            this.NavigationService.Navigate(page);

        }

        private void page_Unloaded_1(object sender, RoutedEventArgs e)
        {

            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }
    }
}
