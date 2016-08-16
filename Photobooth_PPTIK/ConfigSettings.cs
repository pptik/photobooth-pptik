using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photobooth_PPTIK
{
    public class ConfigSettings
    {
        public Frames frames { get; set; }
        public Resolution resolution { get; set; }
    }

    public class Resolution
    {
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Frames
    {
        public string[] files { get; set; }
    }
}
