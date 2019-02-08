using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIYoutubeDownloader
{
    class DropDownMenuItem
    {
        public string Name { get; private set; }
        public string Image { get; private set; }

        public object Source { get; private set; }

        public DropDownMenuItem(string name, string image, object source)
        {
            this.Name = name;
            this.Image = image;
            this.Source = source;
        }
    }
}
