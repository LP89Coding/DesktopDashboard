using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIYoutubeDownloader
{
    public class Enums
    {
        public enum MediaType
        {
            Unknown = 0,
            MP3 = 1,
            MP4 = 2
        }
        public enum ThumbnailQuality
        {
            Unkown = 0,
            Low = 1,
            Standard = 2,
            Medium = 3,
            High = 4,
            Max = 5
        }
    }
}
