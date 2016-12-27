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
        public Smtp smtp { get; set; }
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

    public class Smtp
    {
        public string email { get; set; }
        public string password { get; set; }

        public string msgBody { get; set; }
        public string emailSubject { get; set; }
    }
}
