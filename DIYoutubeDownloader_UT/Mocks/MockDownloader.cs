using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DIYoutubeDownloader.Internal;
using DIYoutubeDownloader;
using DIYoutubeDownloader.Models;
using Tools = DIYoutubeDownloader.Internal.Tools;

namespace DIYoutubeDownloader_UT.Mocks
{
    internal class MockDownloader : IDownloader, IProgress<double>
    {

        private const int VideoInfoLoadTimeout = 2000;
        private const int MediaStreamLoadTimeout = 30000;
        
        private CancellationTokenSource CancelOperation { get; set; }

        public bool InProgress { get; private set; }

        public event Progress OnProgress;
        public event BeginDownload OnBeginDownload;
        public event EndDownload OnEndDownload;
        public event BeginLoadMediaInfo OnBeginLoadMediaInfo;
        public event EndLoadMediaInfo OnEndLoadMediaInfo;

        #region Ctor

        public MockDownloader()
        {
            this.InProgress = false;
        }

        #endregion
        #region GetMediaTypeInfo

        private List<MediaType> GetMediaTypeInfo(string mediaId)
        {
            List<MediaType> result = new List<MediaType>();
            try
            {
                if (!String.IsNullOrWhiteSpace(mediaId))
                {
                    Random rand = new Random();
                    result.Add(new MediaType(MediaType.ExtensionType.MP3, "Ok", rand.Next(1, 1000000000)));
                    result.Add(new MediaType(MediaType.ExtensionType.MP4, "Crap", rand.Next(1, 1000000000)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CancelOperation?.Dispose();
                this.CancelOperation = null;
            }
            return result;
        }

        #endregion

        #region IDownloader implementation

        #region IsDownloading

        public bool IsDownloading()
        {
            return this.InProgress;
        }

        #endregion
        #region GetMediaInfo

        public Media GetMediaInfo(string url)
        {
            Media ymItem = null;
            try
            {
                this.InProgress = true;
                if (this.OnBeginLoadMediaInfo != null)
                    OnBeginLoadMediaInfo();
                if (!String.IsNullOrWhiteSpace(url))
                {
                    Random rand = new Random();
                    Thread.Sleep(rand.Next(VideoInfoLoadTimeout - 500, VideoInfoLoadTimeout));
                    ymItem = new Media(Guid.NewGuid().ToString(), url)
                    {
                        Author = "TestAuthor",
                        Description = "TestDescription",
                        Duration = new TimeSpan(0, 2, 21, 55, 0),
                        Title = "TestTitle",
                        UploadDate = DateTime.Now,
                        Thumbnail = this.GetThumbnail(url),
                        LikesCount = rand.Next(0, 60000000),
                        DislikesCount = rand.Next(0, 60000000),
                        ViewsCount = rand.Next(0, 600000000),
                        AverageRatings = rand.NextDouble() * 5.0
                    };
                    ymItem.MediaTypes.AddRange(this.GetMediaTypeInfo(ymItem.MediaId));
                }
            }
            catch (Exception ex)
            {
                ymItem = null;
            }
            finally
            {
                if (this.OnEndLoadMediaInfo != null)
                    OnEndLoadMediaInfo(ymItem);
                this.CancelOperation?.Dispose();
                this.CancelOperation = null;
                this.InProgress = false;
            }
            return ymItem;
        }

        #endregion
        #region GetThumbnailUrl

        public string GetThumbnailUrl(string mediaUrl)
        {
            return String.Empty;
        }

        #endregion
        #region GetThumbnail

        public Bitmap GetThumbnail(string mediaUrl)
        {
            return ResourceImage.ThumbnailTest;
        }

        #endregion
        #region Download

        public MemoryStream Download(string mediaId, MediaType mediaType)
        {
            MemoryStream downlaodStream = null;
            Task videoDownloader = null;
            try
            {
                this.InProgress = true;
                if (this.OnBeginDownload != null)
                    this.OnBeginDownload();
                if (mediaType == null || mediaType.Extension == MediaType.ExtensionType.Unknown)
                    throw new Exception("Media type not given");
                if (String.IsNullOrWhiteSpace(mediaId))
                    throw new Exception("Media ID not given");

                this.CancelOperation = new CancellationTokenSource();
                for (int i = 0; i < 20; i++)
                {
                    if (this.CancelOperation == null || this.CancelOperation.IsCancellationRequested)
                    {
                        downlaodStream = null;
                        break;
                    }
                    Thread.Sleep(100);
                }
                if(!(this.CancelOperation == null || this.CancelOperation.IsCancellationRequested))
                {
                    downlaodStream = new MemoryStream();
                }
            }
            catch (Exception ex)
            {
                downlaodStream?.Close();
                downlaodStream?.Dispose();
                downlaodStream = null;
            }
            finally
            {
                if (this.OnEndDownload != null)
                    this.OnEndDownload();
                this.CancelOperation?.Dispose();
                this.CancelOperation = null;
                videoDownloader?.Dispose();
                this.InProgress = false;
            }
            return downlaodStream;
        }

        #endregion
        #region Cancel

        public void Cancel()
        {
            try
            {
                if (this.CancelOperation != null && !this.CancelOperation.IsCancellationRequested)
                {
                    this.CancelOperation.Cancel();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                this.CancelOperation?.Dispose();
                this.CancelOperation = null;
            }
        }

        #endregion

        #endregion
        #region IDisposable implementation

        public void Dispose()
        {
            try
            {
                this.CancelOperation?.Dispose();
                this.CancelOperation = null;
            }
            catch (Exception ex)
            {
            }
        }

        #endregion
        #region IProgress implementation

        public void Report(double value)
        {
            if (this.OnProgress != null)
                OnProgress(value);
        }

        #endregion
    }
}
