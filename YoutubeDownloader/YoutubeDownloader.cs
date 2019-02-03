using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Models;
using YoutubeExplode.Models.MediaStreams;

namespace DIYoutubeDownloader
{
    public class YoutubeDownloader : IDisposable
    {
        private const int VideoInfoLoadTimeout = 30000;
        private const int MediaStreamLoadTimeout = 30000;

        private CancellationTokenSource CancelOperation { get; set; }
        
        #region Ctor

        public YoutubeDownloader()
        { 
        }

        #endregion

        #region GetVideoInfo

        private Video GetVideoInfo(string url)
        {
            try
            {
                this.CancelOperation = new CancellationTokenSource();
                YoutubeClient client = new YoutubeClient();
                string videoId = YoutubeClient.ParseVideoId(url);
                Task<Video> videoInfoLoader = client.GetVideoAsync(videoId);
                videoInfoLoader.Wait(VideoInfoLoadTimeout, CancelOperation.Token);
                return videoInfoLoader.Result as Video;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CancelOperation?.Dispose();
            }
        }

        #endregion
        #region GetMediaStreamInfo

        private List<YoutubeMediaType> GetMediaTypeInfo(string mediaId)
        {
            List<YoutubeMediaType> result = new List<YoutubeMediaType>();
            try
            {
                if (!String.IsNullOrWhiteSpace(mediaId))
                {
                    this.CancelOperation = new CancellationTokenSource();
                    YoutubeClient client = new YoutubeClient();
                    Task<MediaStreamInfoSet> streamInfoSetLoader = client.GetVideoMediaStreamInfosAsync(mediaId);
                    streamInfoSetLoader.Wait(MediaStreamLoadTimeout, this.CancelOperation.Token);
                    MediaStreamInfoSet streamInfoSet = streamInfoSetLoader.Result as MediaStreamInfoSet;

                    if (streamInfoSet.Audio != null)
                    {
                        foreach (AudioStreamInfo asiItem in streamInfoSet.Audio.Where(s => s.Container.GetFileExtension().ToLower().Equals("mp4")))
                        {
                            result.Add(new YoutubeMediaType(Enums.MediaType.MP3, null));
                        }
                    }
                    if (streamInfoSet.Video != null)
                    {
                        foreach (VideoStreamInfo vsiItem in streamInfoSet.Video.Where(s => s.Container.GetFileExtension().ToLower().Equals("mp4")))
                        {
                            result.Add(new YoutubeMediaType(Enums.MediaType.MP4, vsiItem.VideoQualityLabel));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CancelOperation?.Dispose();
            }
            return result;
        }

        #endregion

        #region GetMediaInfo

        public YoutubeMedia GetMediaInfo(string url)
        {
            YoutubeMedia ymItem = null;
            try
            {
                if (!String.IsNullOrWhiteSpace(url))
                {
                    Video videoInfo = this.GetVideoInfo(url);

                    ymItem = new YoutubeMedia(videoInfo.Id, url)
                    {
                        Author = videoInfo.Author,
                        Description = videoInfo.Description,
                        Duration = videoInfo.Duration,
                        Title = videoInfo.Title,
                        UploadDate = videoInfo.UploadDate,
                        Thumbnail = this.GetThumbnail(videoInfo),
                        LikesCount = videoInfo?.Statistics.LikeCount ?? 0,
                        DislikesCount = videoInfo?.Statistics.DislikeCount ?? 0,
                        ViewsCount = videoInfo?.Statistics.ViewCount ?? 0,
                        AverageRatings = videoInfo?.Statistics.AverageRating ?? 0.0
                    };
                    ymItem.MediaTypes.AddRange(this.GetMediaTypeInfo(ymItem.MediaId));
                }
            }
            catch(Exception ex)
            {
                //TODO Log
                ymItem = null;
            }
            finally
            {
                this.CancelOperation?.Dispose();
            }
            return ymItem;
        }

        #endregion

        #region GetThumbnailUrl

        public string GetThumbnailUrl(string url, Enums.ThumbnailQuality quality = Enums.ThumbnailQuality.Standard)
        {
            return this.GetThumbnailUrl(this.GetVideoInfo(url), quality: quality);
        }

        private string GetThumbnailUrl(Video videoInfo, Enums.ThumbnailQuality quality = Enums.ThumbnailQuality.Standard)
        {
            string url = null;
            if(videoInfo != null)
            {
                switch(quality)
                {
                    case Enums.ThumbnailQuality.Low:
                        url = videoInfo.Thumbnails.LowResUrl;
                        break;
                    case Enums.ThumbnailQuality.Standard:
                        url = videoInfo.Thumbnails.StandardResUrl;
                        break;
                    case Enums.ThumbnailQuality.Medium:
                        url = videoInfo.Thumbnails.MediumResUrl;
                        break;
                    case Enums.ThumbnailQuality.High:
                        url = videoInfo.Thumbnails.HighResUrl;
                        break;
                    case Enums.ThumbnailQuality.Max:
                        url = videoInfo.Thumbnails.MaxResUrl;
                        break;
                    default:
                        url = videoInfo.Thumbnails.MediumResUrl;
                        break;
                }
            }
            return url;
        }

        #endregion
        #region GetThumbnail

        private Bitmap GetThumbnail(string url, Enums.ThumbnailQuality quality = Enums.ThumbnailQuality.Standard)
        {
            return this.GetThumbnail(this.GetVideoInfo(url), quality: quality);
        }

        private Bitmap GetThumbnail(Video videoInfo, Enums.ThumbnailQuality quality = Enums.ThumbnailQuality.Standard)
        {
            Bitmap result = null;
            string url = GetThumbnailUrl(videoInfo, quality: quality);

            if(!String.IsNullOrWhiteSpace(url))
            {
                try
                {
                    using (WebClient webClient = new WebClient())
                    {
                        using (Stream stream = webClient.OpenRead(url))
                        {
                            result = new Bitmap(stream);
                        }
                    }
                }
                catch(Exception ex)
                {
                    //TODO Log
                }
            }

            return result;
        }

        #endregion

        #region Download

        public byte[] Download(string mediaId, string fileName)
        {
            byte[] result = null;
            try
            {

                //this.CancelDownload = new CancellationTokenSource();
                //MediaStreamInfoSet streamInfoSet = streamInfoSetLoader.Result as MediaStreamInfoSet;
                //MuxedStreamInfo streamInfo = streamInfoSet.Muxed.WithHighestVideoQuality();
                //string fileExtension = streamInfo.Container.GetFileExtension();
                //Path.GetInvalidFileNameChars().Select(c => fileName.Replace(c, ' '));
                //Task videoDownloader = client.DownloadMediaStreamAsync(streamInfo, $"{fileName}.{fileExtension}");
                //videoDownloader.Wait(this.CancelDownload.Token);
            }
            catch(Exception ex)
            {
                //TODO Log
            }
            finally
            {
                this.CancelOperation?.Dispose();
            }
            return result;
        }

        #endregion

        #region Cancel

        public void Cancel()
        {
            try
            {
                if(this.CancelOperation != null && !this.CancelOperation.IsCancellationRequested)
                {
                    this.CancelOperation.Cancel();
                }
            }
            catch (Exception ex)
            {
                //TODO Log
            }
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            try
            {
                this.CancelOperation?.Dispose();
            }
            catch(Exception ex)
            {
                //TODO log
            }
        }

        #endregion
    }
}
