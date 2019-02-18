using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using Microsoft.Win32;

using DIYoutubeDownloader.Internal;
using EventID = DIYoutubeDownloader.Internal.EventID.DIYoutubeDownloader;
using System.Windows;
using DIYoutubeDownloader.Internals;
using System.Windows.Input;

namespace DIYoutubeDownloader.ViewModels
{
    public class YoutubeDownloaderViewModel : ObservableViewModel, IDisposable
    {
        private IDownloader downloader { get; } = new Downloader();

        private Media media;
        private Media Media
        {
            get
            {
                return this.media;
            }
            set
            {
                this.media = value;
                RaisePropertyChangedEvent(nameof(this.AverageRatings));
                RaisePropertyChangedEvent(nameof(this.LikesCount));
                RaisePropertyChangedEvent(nameof(this.DislikesCount));
                RaisePropertyChangedEvent(nameof(this.Duration));
                RaisePropertyChangedEvent(nameof(this.Thumbnail));

                RaisePropertyChangedEvent(nameof(this.MediaDescription));
                RaisePropertyChangedEvent(nameof(this.DownloadMediaTypes));
                RaisePropertyChangedEvent(nameof(this.DownloadMediaIsEnabled));
            }
        }

        public string Title { get { return this.Media?.Title; } }
        public double AverageRatings { get { return this.Media?.AverageRatings ?? 0.0; } }
        public string LikesCount { get { return this.GetStatisticsShortcutString(this.Media?.LikesCount); } }
        public string DislikesCount { get { return this.GetStatisticsShortcutString(this.Media?.DislikesCount); } }
        public string Duration { get { return (this.Media?.Duration ?? new TimeSpan()).ToString("c"); } }
        private BitmapImage thumbnail;
        public BitmapImage Thumbnail
        {
            get
            {
                this.thumbnail = null;
                GC.Collect();
                this.thumbnail = Utils.ToBitmapImage(this.Media?.Thumbnail ?? ResourceImage128.YouTube);
                return this.thumbnail;
            }
        }

        public List<MediaTypeViewModel> DownloadMediaTypes { get { return this.Media?.MediaTypes.Select(mt => new MediaTypeViewModel(mt, this.downloader, this.media)).ToList(); } }
        public string MediaDescription { get { return this.Media?.ToString(); } }

        private bool isDownloading;
        public bool IsDownloading
        {
            get { return this.isDownloading; }
            set
            {
                this.isDownloading = value;
                RaisePropertyChangedEvent(nameof(this.DownloadMediaIsEnabled));
                RaisePropertyChangedEvent(nameof(this.FindMediaIsEnabled));
                RaisePropertyChangedEvent(nameof(this.MediaLoaderVisibility));
                RaisePropertyChangedEvent(nameof(this.DownloadMediaCancelIsEnabled));
            }
        }
        public bool IsIdle { get { return !this.IsDownloading; } }

        public string WindowTitle { get { return this.IsDownloading && this.TaskBarProgressState == TaskbarItemProgressState.Normal ? $"{Consts.YoutubeDownloaderTitle} ({Math.Round(this.TaskBarProgressValue * 100.0, 0)}%)" : Consts.YoutubeDownloaderTitle; } }
        public bool DownloadMediaIsEnabled { get { return !String.IsNullOrWhiteSpace(this.Media?.MediaId) && !this.IsDownloading; } }
        public bool DownloadMediaCancelIsEnabled { get { return this.IsDownloading; } }
        public bool FindMediaIsEnabled { get { return !this.IsDownloading; } }
        public Visibility MediaLoaderVisibility { get { return this.IsDownloading ? Visibility.Visible : Visibility.Hidden; } }
        public Visibility DownloadMedialVisibility { get { return !this.IsDownloading ? Visibility.Visible : Visibility.Hidden; } }
        public Visibility DownloadMediaCancelVisibility { get { return this.IsDownloading ? Visibility.Visible : Visibility.Hidden; } }
        private double taskBarProgressValue;
        public double TaskBarProgressValue
        {
            get { return this.taskBarProgressValue; }
            set
            {
                this.taskBarProgressValue = value;
                RaisePropertyChangedEvent(nameof(this.TaskBarProgressValue));
                RaisePropertyChangedEvent(nameof(this.WindowTitle));
            }
        }
        private TaskbarItemProgressState taskBarProgressState;
        public TaskbarItemProgressState TaskBarProgressState
        {

            get { return this.taskBarProgressState; }
            set
            {
                this.taskBarProgressState = value;
                RaisePropertyChangedEvent(nameof(this.TaskBarProgressState));
                RaisePropertyChangedEvent(nameof(this.WindowTitle));
            }
        }
        
        private ICommand cancelButtonCommand;
        public ICommand CancelButtonCommand { get { return this.cancelButtonCommand; } private set { this.cancelButtonCommand = value; } }
        private ICommand findMediaButtonCommand;
        public ICommand FindMediaButtonCommand { get { return this.findMediaButtonCommand; } private set { this.findMediaButtonCommand = value; } }

        #region Ctor
        public YoutubeDownloaderViewModel()
        {
            this.FindMediaButtonCommand = new Command((object parameter) => { LoadMediaInfoAsync(parameter?.ToString()); }, param => FindMediaIsEnabled);
            this.CancelButtonCommand = new Command((object parameter) => { CancelDownload(); });
            this.SetEvents();

        }
        #endregion

        #region Events

        #region Downloader

        #region Downloader_OnBeginLoadMediaInfo
        private void Downloader_OnBeginLoadMediaInfo()
        {
            this.IsDownloading = true;
        }
        #endregion
        #region Downloader_OnEndLoadMediaInfo
        private void Downloader_OnEndLoadMediaInfo(Media mediaInfo)
        {
            this.IsDownloading = false;
        }
        #endregion
        #region Downloader_OnBeginDownload
        private void Downloader_OnBeginDownload()
        {
            this.IsDownloading = true;
            this.TaskBarProgressState = TaskbarItemProgressState.Normal;
            RaisePropertyChangedEvent(nameof(this.DownloadMediaCancelVisibility));
            RaisePropertyChangedEvent(nameof(this.DownloadMedialVisibility));
        }
        #endregion
        #region Downloader_OnEndDownload
        private void Downloader_OnEndDownload()
        {
            this.IsDownloading = false;
            this.TaskBarProgressState = TaskbarItemProgressState.None;
            RaisePropertyChangedEvent(nameof(this.DownloadMediaCancelVisibility));
            RaisePropertyChangedEvent(nameof(this.DownloadMedialVisibility));
        }
        #endregion
        #region Downloader_OnProgress
        private void Downloader_OnProgress(double progress)
        {
            this.TaskBarProgressValue = progress;
        }
        #endregion

        #endregion

        #endregion

        #region Methods

        #region SetEvents
        private void SetEvents()
        {
            if (this.downloader != null)
            {
                this.downloader.OnProgress += Downloader_OnProgress;
                this.downloader.OnBeginDownload += Downloader_OnBeginDownload;
                this.downloader.OnEndDownload += Downloader_OnEndDownload;
                this.downloader.OnBeginLoadMediaInfo += Downloader_OnBeginLoadMediaInfo;
                this.downloader.OnEndLoadMediaInfo += Downloader_OnEndLoadMediaInfo;
            }
        }
        #endregion
        #region LoadMediaInfoAsync
        public void LoadMediaInfoAsync(string url)
        {
            try
            {
                new Task(() =>
                {
                    Media ytMedia = null;
                    if (!String.IsNullOrWhiteSpace(url) &&
                       (this.Media == null || (this.Media != null && !String.Equals(this.Media.Url, url))))
                    {
                        try
                        {
                            ytMedia = this.downloader.GetMediaInfo(url);
                        }
                        catch(Exception ex)
                        {
                            Utils.Logger.Log(EventID.FindMediaClick.Exception, ex);
                            ytMedia = null;
                        }

                        if (ytMedia == null && (this.Media == null || !String.IsNullOrWhiteSpace(this.Media.MediaId)))
                        {
                            ytMedia = new Media(null, null)
                            {
                                Title = "Title",
                                Author = "Author",
                                Duration = new TimeSpan(),
                                AverageRatings = 0.0,
                                LikesCount = 0,
                                DislikesCount = 0,
                                Thumbnail = ResourceImage128.YouTube
                            };
                        }
                        if (ytMedia != null)
                        {
                            this.Media?.Dispose();
                            this.Media = ytMedia;
                        }
                    }
                }).Start();
            }
            catch(Exception ex)
            {
                Utils.Logger.Log(EventID.LoadMediaInfoAsync.Exception, ex);
            }
        }
        #endregion
        #region CancelDownload

        public void CancelDownload()
        {
            this.downloader?.Cancel();
        }

        #endregion
        #region GetStatisticsShortcutString

        private string GetStatisticsShortcutString(long? statistics)
        {
            string result = "-";
            const double m = 1000000;
            const double k = 1000;

            if (statistics.HasValue)
            {
                if (statistics >= m)
                    result = $"{Math.Round((statistics.Value / m), 1)}M";
                else if (statistics >= k)
                    result = $"{Math.Round((statistics.Value / k), 1)}K";
                else
                    result = statistics.Value.ToString();
            }
            return result;
        }

        #endregion

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            try
            {
                this.Media?.Dispose();
                this.downloader?.Dispose();
            }
            catch (Exception ex)
            {
                //TODO Log
                //Utils.Logger.Log(EventID.DIYoutubeDowbloader.Downloader.DisposeException, ex);
            }
        }

        #endregion

    }
}
