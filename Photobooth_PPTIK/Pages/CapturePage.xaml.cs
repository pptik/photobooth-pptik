﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.IO;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using Newtonsoft.Json;
using System.Windows.Input;

namespace Photobooth_PPTIK
{
    /// <summary>
    /// Interaction logic for CapturePage.xaml
    /// </summary>
    public partial class CapturePage : Page
    {
        public FilterInfoCollection CamsCollection;
        public VideoCaptureDevice Cam = null;

        private int currentElement = 0;
        private DispatcherTimer _timer;
        private TimeSpan _time;
        private int nFrame = 0;
        private int configWidth = 0;

        private Uri[] uriSource;
     
        //number
        private Uri[] numbers = {
           new Uri(@"/Photobooth_PPTIK;component/Resources/Icon/number-0.png", UriKind.Relative),
           new Uri(@"/Photobooth_PPTIK;component/Resources/Icon/number-1.png", UriKind.Relative),
           new Uri(@"/Photobooth_PPTIK;component/Resources/Icon/number-2.png", UriKind.Relative),
           new Uri(@"/Photobooth_PPTIK;component/Resources/Icon/number-3.png", UriKind.Relative),
           new Uri(@"/Photobooth_PPTIK;component/Resources/Icon/number-4.png", UriKind.Relative),
           new Uri(@"/Photobooth_PPTIK;component/Resources/Icon/number-5.png", UriKind.Relative),
           new Uri(@"/Photobooth_PPTIK;component/Resources/Icon/number-6.png", UriKind.Relative),
           new Uri(@"/Photobooth_PPTIK;component/Resources/Icon/number-7.png", UriKind.Relative),
           new Uri(@"/Photobooth_PPTIK;component/Resources/Icon/number-8.png", UriKind.Relative),
           new Uri(@"/Photobooth_PPTIK;component/Resources/Icon/number-9.png", UriKind.Relative),
           new Uri(@"/Photobooth_PPTIK;component/Resources/Icon/number-10.png", UriKind.Relative),
        };

        private bool Triger = true;


        public CapturePage()
        {
            
            this.InitializeComponent();
            this.Loaded += OnLoaded;
            _time = TimeSpan.FromSeconds(10);
            CaptureBtn.Focus();

            ConfigSettings config = new ConfigSettings();
            string configPath = @"" + Directory.GetCurrentDirectory() + "/config.json";
            using (StreamReader file = File.OpenText("config.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                config = serializer.Deserialize<ConfigSettings>(reader);
                configWidth = config.resolution.width;
                var frames = config.frames.files;
                nFrame = frames.Count();
                uriSource = new Uri[nFrame];
                for (int i = 0; i < frames.Count(); i++)
                {
                    uriSource[i] = new Uri(@"/Photobooth_PPTIK;component/Resources/Frame/"+frames[i], UriKind.Relative);
                    Console.WriteLine("path "+uriSource[i]);

                    System.Windows.Controls.Image frame = new System.Windows.Controls.Image();
                    frame.Source = new BitmapImage(uriSource[i]);
                    frame.Margin = new Thickness(0);
                    frame.Stretch = System.Windows.Media.Stretch.Fill;
                                        
                    var bindActualWidth = new System.Windows.Data.Binding("ActualWidth");
                    bindActualWidth.ElementName = "canvas";
                    bindActualWidth.Mode = System.Windows.Data.BindingMode.OneWay;
                    frame.SetBinding(System.Windows.Controls.Image.WidthProperty, bindActualWidth);

                    var bindActualHeight= new System.Windows.Data.Binding("ActualHeight");
                    bindActualHeight.ElementName = "canvas";
                    bindActualHeight.Mode = System.Windows.Data.BindingMode.OneWay;
                    frame.SetBinding(System.Windows.Controls.Image.HeightProperty, bindActualHeight);

                    Carousel.Children.Add(frame);
                }

                //var uriSource1 = new Uri(@"/Photobooth_PPTIK;component/Resources/Frame/frame_1.png", UriKind.Relative);
                //var uriSource2 = new Uri(@"/Photobooth_PPTIK;component/Resources/Frame/frame_2.png", UriKind.Relative);
                //var uriSource3 = new Uri(@"/Photobooth_PPTIK;component/Resources/Frame/frame_3.png", UriKind.Relative);
                //var uriSource4 = new Uri(@"/Photobooth_PPTIK;component/Resources/Frame/frame_4.png", UriKind.Relative);
                //var uriSource5 = new Uri(@"/Photobooth_PPTIK;component/Resources/Frame/frame_5.png", UriKind.Relative);
                
                //frame1.Source = new BitmapImage(uriSource[0]);
                //frame2.Source = new BitmapImage(uriSource[1]);
                //frame3.Source = new BitmapImage(uriSource[2]);
                //frame4.Source = new BitmapImage(uriSource[3]);
                //frame5.Source = new BitmapImage(uriSource[4]);
            }          

        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            CamsCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            Cam = new VideoCaptureDevice(CamsCollection[0].MonikerString);
            Cam.NewFrame += new NewFrameEventHandler(Cam_NewFrame);
            Cam.Start();
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
            var gapValue = this.configWidth;
            Canvas c = this.Resources["canvas"] as Canvas;
            Storyboard storyboard = (this.Resources["CarouselStoryboard"] as Storyboard);
            DoubleAnimation animation = storyboard.Children.First() as DoubleAnimation;
            Console.WriteLine("trans: " + (this.ActualWidth));
            animation.To = ((-(gapValue * 2) * currentElement) / 2);
            storyboard.Begin();
        }

        void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            System.Drawing.Image imgforms = (Bitmap)eventArgs.Frame.Clone();

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();

            MemoryStream ms = new MemoryStream();
            imgforms.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);

            bi.StreamSource = ms;
            bi.EndInit();

            
            bi.Freeze();

            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                viewer.Source = bi; 
            }));
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
                    _time = TimeSpan.FromSeconds(11);
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
            string appPathFB = @"" + Directory.GetCurrentDirectory() + "/images_fb";
            Directory.CreateDirectory(appPath);
            Directory.CreateDirectory(appPathFB);
            string path = Path.Combine(appPath, "photobooth_" + n + ext);
            string pathFB = Path.Combine(appPathFB, "photobooth_" + n + ext);
            Console.WriteLine("Path: "+path);

            util.SaveCanvas(this, this.canvas, 96, path);
            util.SaveCanvas(this, this.canvas, 96, pathFB);
            CaptureBtn.Visibility = Visibility.Visible;

            //go to result page
            var page = new ViewPage(path);
            this.NavigationService.Navigate(page);

        }

        private void page_Unloaded(object sender, RoutedEventArgs e)
        {
            Cam.Stop();
        }

        private void page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Right && Triger)
            {
                SwipeLeft();
                Triger = false;
            }
            else if (e.Key == Key.Left && Triger)
            {
                SwipeRight();
                Triger = false;
            }
            else if (e.Key == Key.Enter && Triger)
            {
                Capture();
                Triger = false;
            }
        }

        private void page_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Triger = true;
        }
    }
}
