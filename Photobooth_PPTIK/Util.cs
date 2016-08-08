using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Photobooth_PPTIK
{
    public class util
    {
        public static void SaveWindow(Page page, int dpi, string filename)
        {

            var rtb = new RenderTargetBitmap(
                (int)page.Width, //width 
                (int)page.Width, //height 
                dpi, //dpi x 
                dpi, //dpi y 
                PixelFormats.Pbgra32 // pixelformat 
                );
            rtb.Render(page);

            SaveRTBAsPNG(rtb, filename);

        }

        public static void SaveCanvas(Page page, Canvas canvas, int dpi, string filename)
        {
            Size size = new Size(canvas.ActualWidth, canvas.ActualHeight);
            canvas.Measure(size);
            canvas.Arrange(new Rect(size));

            var rtb = new RenderTargetBitmap(
                (int)canvas.ActualWidth, //width 
                (int)canvas.ActualHeight, //height 
                dpi, //dpi x 
                dpi, //dpi y 
                PixelFormats.Pbgra32 // pixelformat 
                );
            rtb.Render(canvas);

            SaveRTBAsPNG(rtb, filename);
        }

        private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
        {
            var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);
            }
        }
    }
}
