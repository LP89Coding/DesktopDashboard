using System;
using System.Collections.Generic;
using System.Drawing;

using Logger = WPF.Common.Logger.Logger;

using DIYoutubeDownloader.Internal;

namespace DIYoutubeDownloader.Models
{
    public class Media : IDisposable
    {
        public string MediaId { get; private set; }
        public string Url { get; private set; }

        public string Author { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTimeOffset? UploadDate { get; set; }
        public TimeSpan? Duration { get; set; }

        public long LikesCount { get; set; }
        public long DislikesCount { get; set; }
        public long ViewsCount { get; set; }
        public double AverageRatings { get; set; }

        public Bitmap Thumbnail { get; set; }
        public List<MediaType> MediaTypes { get; private set; }

        #region Ctor

        public Media(string mediaId, string url)
        {
            this.MediaId = mediaId;
            this.Url = url;
            this.MediaTypes = new List<MediaType>();
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return $"{this.Title} ({this.Author})";
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            try
            {
                if (this.Thumbnail != null)
                    this.Thumbnail.Dispose();
            }
            catch(Exception ex)
            {
                Logger.Log(EventID.DIYoutubeDownloader.Media.DisposeException, ex);
            }
        }

        #endregion
    }
}
