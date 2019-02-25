using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIYoutubeDownloader.Models
{
    public class MediaType
    {
        public enum ExtensionType
        {
            Unknown = 0,
            MP3 = 1,
            MP4 = 2
        }
        public enum SizeUnit
        {
            Unknown = 0,
            KB = 1,
            MB = 2,
            GB = 3
        }

        public ExtensionType Extension { get; private set; }
        public string Quality { get; private set; }
        public long Size { get; private set; }

        #region Ctor

        public MediaType(ExtensionType extension, string quality, long size)
        {
            this.Extension = extension;
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
            return $"{this.Extension.ToString().ToLower()} {this.Quality} ({Math.Round(this.GetSize(SizeUnit.MB),2)} MB)";
        }

        #endregion
    }
}
