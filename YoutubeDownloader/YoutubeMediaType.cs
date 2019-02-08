using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIYoutubeDownloader
{
    public class YoutubeMediaType
    {
        public enum SizeUnit
        {
            Unknown = 0,
            KB = 1,
            MB = 2,
            GB = 3
        }

        public Enums.MediaType MediaType { get; private set; }
        public string Quality { get; private set; }
        public long Size { get; private set; }

        #region Ctor

        public YoutubeMediaType(Enums.MediaType mediaType, string quality, long size)
        {
            this.MediaType = mediaType;
            this.Quality = quality;
            this.Size = size;
        }

        #endregion

        #region GetSize

        public double GetSize(SizeUnit unit = SizeUnit.Unknown)
        {
            double result = this.Size;
            if(unit != SizeUnit.Unknown)
            {
                result = result / (Math.Pow(1024, (int)unit));
            }

            return result;
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return $"{this.MediaType.ToString().ToLower()} {this.Quality} ({Math.Round(this.GetSize(SizeUnit.MB),2)} MB)";
        }

        #endregion
    }
}
