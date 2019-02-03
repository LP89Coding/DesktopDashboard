using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIYoutubeDownloader
{
    public class YoutubeMediaType
    {
        public Enums.MediaType MediaType { get; private set; }
        public string Quality { get; private set; }

        #region Ctor

        public YoutubeMediaType(Enums.MediaType mediaType, string quality)
        {
            this.MediaType = mediaType;
            this.Quality = quality;
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return this.MediaType.ToString().ToLower() + (String.IsNullOrWhiteSpace(this.Quality) ? "" : $" ({this.Quality})");
        }

        #endregion
    }
}
