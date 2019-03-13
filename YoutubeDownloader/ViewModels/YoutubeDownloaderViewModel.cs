using System;
using System.Windows;
using System.Windows.Input;
using DIYoutubeDownloader.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Shell;

using WPF.Common.Common;
using WPF.Common.Interfaces;
using Logger = WPF.Common.Logger.Logger;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;
using WPFUtils = WPF.Common.Common.Utils;

using EventID = DIYoutubeDownloader.Internal.EventID.DIYoutubeDownloader;

namespace DIYoutubeDownloader.ViewModels
{
    public class YoutubeDownloaderViewModel : ObservableViewModel, IViewModel, IWindowPropertyChangeNotifier
    {
        private IDownloader downloader { get; set; }

        private Media media;
        public Media Media
        {
            get
            {
                return this.media;
            }
            private set
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
                this.thumbnail = WPFUtils.ToBitmapImage(this.Media?.Thumbnail ?? ResourceImage.YouTube);
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
        
        public bool DownloadMediaIsEnabled { get { return !String.IsNullOrWhiteSpace(this.Media?.MediaId) && !this.IsDownloading; } }
        public bool DownloadMediaCancelIsEnabled { get { return this.IsDownloading; } }
        public bool FindMediaIsEnabled { get { return !this.IsDownloading; } }
        public Visibility MediaLoaderVisibility { get { return this.IsDownloading ? Visibility.Visible : Visibility.Hidden; } }
        public Visibility DownloadMedialVisibility { get { return !this.IsDownloading ? Visibility.Visible : Visibility.Hidden; } }
        public Visibility DownloadMediaCancelVisibility { get { return this.IsDownloading ? Visibility.Visible : Visibility.Hidden; } }
        
        private ICommand cancelButtonCommand;
        public ICommand CancelButtonCommand { get { return this.cancelButtonCommand; } private set { this.cancelButtonCommand = value; } }
        private ICommand findMediaButtonCommand;
        public ICommand FindMediaButtonCommand { get { return this.findMediaButtonCommand; } private set { this.findMediaButtonCommand = value; } }

        #region Ctor
        public YoutubeDownloaderViewModel()
        {

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
        private void SetDownloaderEvents()
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
        private void LoadMediaInfoAsync(string url)
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
                            Logger.Log(EventID.Application.Exception, ex);
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
                                Thumbnail = ResourceImage.YouTube
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
                Logger.Log(EventID.Application.Exception, ex);
            }
        }
        #endregion
        #region CancelDownload

        private void CancelDownload()
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
                Logger.Log(EventID.Application.Exception, ex);
            }
        }

        #endregion
        #region IViewModel implementation

        public void Initialize(ArgumentCollection args)
        {
            if (args == null || args.Length == 0)
                throw new Exception("Parameters cannot be empty");
            if(!args.Contains(ArgumentCollection.ArgumentType.Downloader) && !(args.Get(ArgumentCollection.ArgumentType.Downloader) is IDownloader))
                throw new Exception("First parametr needs to implement IDownloader interface");
            this.downloader = args.Get(ArgumentCollection.ArgumentType.Downloader) as IDownloader;
            
            this.FindMediaButtonCommand = new Command((object parameter) => { LoadMediaInfoAsync(parameter?.ToString()); }, param => FindMediaIsEnabled);
            this.CancelButtonCommand = new Command((object parameter) => { CancelDownload(); });
            this.SetDownloaderEvents();
        }

        public bool NotifyPropertyChange(string propertyName, object propertyValue)
        {
            return false;
        }

        public object GetPropertyValue(string propertyName)
        {
            switch(propertyName)
            {
                case nameof(this.TaskBarProgressState): return this.TaskBarProgressState;
                case nameof(this.TaskBarProgressValue): return this.TaskBarProgressValue;
                default: return null;
            }
        }

        #endregion
        #region IWindowPropertyNotifier implementation

        private string windowTitle;
        public string WindowTitle
        {
            get { return this.windowTitle; }
            set
            {
                this.windowTitle = value;
                RaisePropertyChangedEvent(nameof(this.WindowTitle));
            }
        }
        private double taskBarProgressValue;
        public double TaskBarProgressValue
        {
            get { return this.taskBarProgressValue; }
            set
            {
                this.taskBarProgressValue = value;
                RaisePropertyChangedEvent(nameof(this.TaskBarProgressValue));
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
            }
        }

        public double Height { get; set; }
        public double Width { get; set; }

        #endregion
    }
}
