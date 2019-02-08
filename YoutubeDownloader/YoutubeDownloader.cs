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
using YoutubeExplode;
using YoutubeExplode.Models;
using YoutubeExplode.Models.MediaStreams;

namespace DIYoutubeDownloader
{
    public class YoutubeDownloader : IDisposable, IProgress<double>
    {
        /*
         * https://github.com/Tyrrrz/YoutubeExplode
        */
        private const int VideoInfoLoadTimeout = 30000;
        private const int MediaStreamLoadTimeout = 30000;

        private IYoutubeClient youtubeClient { get; set; }

        private Task youtubeOperationTask { get; set; }

        private CancellationTokenSource CancelOperation { get; set; }
        
        public delegate void Progress(double progress);
        public delegate void BeginDownload();
        public delegate void EndDownload();
        public delegate void BeginLoadMediaInfo();
        public delegate void EndLoadMediaInfo(YoutubeMedia mediaInfo);

        public event Progress OnProgress;
        public event BeginDownload OnBeginDownload;
        public event EndDownload OnEndDownload;
        public event BeginLoadMediaInfo OnBeginLoadMediaInfo;
        public event EndLoadMediaInfo OnEndLoadMediaInfo;

        public bool IsDownloading { get; private set; }

        #region Ctor

        public YoutubeDownloader()
        {
            this.youtubeClient = new YoutubeClient();
            this.IsDownloading = false;
        }

        #endregion

        #region GetVideoInfo

        private Video GetVideoInfo(string url)
        {
            try
            {
                this.CancelOperation = new CancellationTokenSource();
                string videoId = YoutubeClient.ParseVideoId(url);
                Task<Video> videoInfoLoader = this.youtubeClient.GetVideoAsync(videoId);
                videoInfoLoader.Wait(VideoInfoLoadTimeout, CancelOperation.Token);
                return videoInfoLoader.Result as Video;
            }
            catch (Exception ex)
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

        private MediaStreamInfoSet GetMediaStreamInfo(string mediaId)
        {
            try
            {
                this.CancelOperation = new CancellationTokenSource();
                Task<MediaStreamInfoSet> streamInfoSetLoader = this.youtubeClient.GetVideoMediaStreamInfosAsync(mediaId);
                streamInfoSetLoader.Wait(MediaStreamLoadTimeout, this.CancelOperation.Token);
                return streamInfoSetLoader.Result as MediaStreamInfoSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CancelOperation?.Dispose();
            }
        }

        #endregion

        #region GetMediaInfo

        public YoutubeMedia GetMediaInfo(string url)
        {
            YoutubeMedia ymItem = null;
            try
            {
                if (this.OnBeginLoadMediaInfo != null)
                    OnBeginLoadMediaInfo();
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
            catch (Exception ex)
            {
                //TODO Log
                ymItem = null;
            }
            finally
            {
                if (this.OnEndLoadMediaInfo != null)
                    OnEndLoadMediaInfo(ymItem);
                this.CancelOperation?.Dispose();
            }
            return ymItem;
        }

        #endregion
        #region GetMediaTypeInfo

        private List<YoutubeMediaType> GetMediaTypeInfo(string mediaId)
        {
            List<YoutubeMediaType> result = new List<YoutubeMediaType>();
            try
            {
                if (!String.IsNullOrWhiteSpace(mediaId))
                {
                    MediaStreamInfoSet streamInfoSet = this.GetMediaStreamInfo(mediaId);

                    if (streamInfoSet.Audio != null)
                    {
                        AudioStreamInfo asiItem = streamInfoSet.Audio.FirstOrDefault(s => s.Container.GetFileExtension().ToLower().Equals("mp4"));
                        if (asiItem != null)
                        {
                            result.Add(new YoutubeMediaType(Enums.MediaType.MP3, null, asiItem.Size));
                        }
                    }
                    //Nie uzywamy Video, bo obsługuje tylko obraz, aby go połączyć z dźwiękiem potrzeba dodatkowo Youtube.Converter + FFMPEG
                    if (streamInfoSet.Muxed != null)
                    {
                        foreach (MuxedStreamInfo vsiItem in streamInfoSet.Muxed.Where(s => s.Container.GetFileExtension().ToLower().Equals("mp4")))
                        {
                            result.Add(new YoutubeMediaType(Enums.MediaType.MP4, vsiItem.VideoQualityLabel, vsiItem.Size));
                        }
                    }
                }
            }
            catch (Exception ex)
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

        #region GetThumbnailUrl

        public string GetThumbnailUrl(string url, Enums.ThumbnailQuality quality = Enums.ThumbnailQuality.Standard)
        {
            return this.GetThumbnailUrl(this.GetVideoInfo(url), quality: quality);
        }

        private string GetThumbnailUrl(Video videoInfo, Enums.ThumbnailQuality quality = Enums.ThumbnailQuality.Standard)
        {
            string url = null;
            if (videoInfo != null)
            {
                switch (quality)
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

            if (!String.IsNullOrWhiteSpace(url))
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
                catch (Exception ex)
                {
                    //TODO Log
                }
            }

            return result;
        }

        #endregion

        #region Download

        public MemoryStream Download(string mediaId, YoutubeMediaType mediaType)
        {
            MemoryStream downlaodStream = null;
            try
            {
                this.IsDownloading = true;
                if (this.OnBeginDownload != null)
                    this.OnBeginDownload();
                if (mediaType == null || mediaType.MediaType == Enums.MediaType.Unknown)
                    throw new Exception("Media type not given");
                if (String.IsNullOrWhiteSpace(mediaId))
                    throw new Exception("Media ID not given");

                MediaStreamInfoSet mediaStreamInfoSet = this.GetMediaStreamInfo(mediaId);
                MediaStreamInfo mediaStreamInfo = null;

                if (mediaType.MediaType == Enums.MediaType.MP3)
                {
                    mediaStreamInfo = mediaStreamInfoSet?.Audio?.FirstOrDefault(s => s.Container.GetFileExtension().ToLower().Equals("mp4"));
                }
                else if (mediaType.MediaType == Enums.MediaType.MP4)
                {
                    mediaStreamInfo = mediaStreamInfoSet?.Muxed?.FirstOrDefault(s => s.Container.GetFileExtension().ToLower().Equals("mp4") &&
                                                                                     s.VideoQualityLabel.Equals(mediaType.Quality) &&
                                                                                     s.Size == mediaType.Size);
                }
                else
                    throw new Exception($"Unsupported media type: {mediaType.MediaType.ToString()}");
                if (mediaStreamInfo == null)
                    throw new Exception("Media stream not found");


                this.CancelOperation = new CancellationTokenSource();

                downlaodStream = new MemoryStream();
                Task videoDownloader = this.youtubeClient.DownloadMediaStreamAsync(mediaStreamInfo, downlaodStream, 
                                                                                   progress: this,
                                                                                   cancellationToken: this.CancelOperation.Token);
                videoDownloader.Wait();
                if (this.CancelOperation == null || this.CancelOperation.IsCancellationRequested)
                    downlaodStream = null;
            }
            catch (Exception ex)
            {
                //TODO Log
                downlaodStream = null;
            }
            finally
            {
                this.IsDownloading = false;
                if (this.OnEndDownload != null)
                    this.OnEndDownload();
                this.CancelOperation?.Dispose();
            }
            return downlaodStream;
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

        #region IProgress implementation

        public void Report(double value)
        {
            if (this.OnProgress != null)
                OnProgress(value);
        }

        #endregion
    }
}
