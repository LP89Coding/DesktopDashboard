using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Win32;

using Syncfusion.SfSkinManager;

using DIYoutubeDownloader.Internal;
using EventID = DIYoutubeDownloader.Internal.EventID.DIYoutubeDowbloader;

namespace DIYoutubeDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string MainWindowTitle = "Youtube downloader"; 

        private Downloader downloader { get; }
        private Media downloadingMedia { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            this.downloader = new Downloader();
            this.InitializeControls();
            this.SetEvents();
        }

        #region Events

        #region Downloader

        #region Downloader_OnBeginLoadMediaInfo
        private void Downloader_OnBeginLoadMediaInfo()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    baFindMedia.IsEnabled = false;
                    ddbaDownloadMedia.IsEnabled = false;
                    biMediaLoader.Visibility = Visibility.Visible;
                });
            }
            catch(Exception ex)
            {
                //TODO Log
            }
        }
        #endregion
        #region Downloader_OnEndLoadMediaInfo
        private void Downloader_OnEndLoadMediaInfo(Media mediaInfo)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                   this.RefreshControls(mediaInfo);

                   this.baFindMedia.IsEnabled = true;
                   this.biMediaLoader.Visibility = Visibility.Hidden;
                });
            }
            catch (TaskCanceledException ex)
            {
                //TODO Log
                Downloader_OnEndLoadMediaInfo(null);
            }
        }
        #endregion
        #region Downloader_OnBeginDownload
        private void Downloader_OnBeginDownload()
        {
            try
            {
                this.Dispatcher.Invoke(() => {
                    this.tbeUrl.IsEnabled = false;
                    this.ddbaDownloadMedia.Visibility = Visibility.Hidden;
                    this.baDownloadMediaCancel.Visibility = Visibility.Visible;
                    this.biMediaLoader.Visibility = Visibility.Visible;

                    this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal;
                    this.TaskbarItemInfo.ProgressValue = 0;
                });
            }
            catch (Exception ex)
            {
                //TODO Log
            }
        }
        #endregion
        #region Downloader_OnEndDownload
        private void Downloader_OnEndDownload()
        {
            try
            {
                this.Dispatcher.Invoke(() => {
                    this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
                    this.TaskbarItemInfo.ProgressValue = 0;
                    this.Title = $"{MainWindowTitle}";

                    this.tbeUrl.IsEnabled = true;
                    this.ddbaDownloadMedia.Visibility = Visibility.Visible;
                    this.baDownloadMediaCancel.Visibility = Visibility.Hidden;
                    this.biMediaLoader.Visibility = Visibility.Hidden;
                });
            }
            catch (TaskCanceledException ex)
            {
                //TODO Log
            }
        }
        #endregion
        #region Downloader_OnProgress
        private void Downloader_OnProgress(double progress)
        {
            try
            {
                int progressRounded = (int)(progress * 100);
                wDIYoutubeDonwloader.Dispatcher.Invoke(() => {
                    this.Title = $"{MainWindowTitle} ({progressRounded}%)";
                    this.TaskbarItemInfo.ProgressValue = progress;
                });
            }
            catch (TaskCanceledException ex)
            {
                //TODO Log
            }
        }
        #endregion

        #endregion
        #region Form

        #region baFindMedia_Click
        private void baFindMedia_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.LoadMediaInfoAsync(tbeUrl.Text);
            }
            catch (Exception ex)
            {
                //TODO Log
                Downloader_OnEndLoadMediaInfo(null);
                this.downloadingMedia = null;
            }
        }
        #endregion
        #region ddbaDownloadMedia_DropDownMenuItemClick_
        private void ddbaDownloadMedia_DropDownMenuItemClick_(object sender, RoutedEventArgs e)
        {
            try
            {
                Syncfusion.Windows.Tools.Controls.DropDownMenuItem ddmiItem = sender as Syncfusion.Windows.Tools.Controls.DropDownMenuItem;
                if (ddmiItem != null)
                {
                    MediaType ytmtItem = ddmiItem.Tag as MediaType;
                    DownloadMediaAsync(this.downloader, this.downloadingMedia, ytmtItem);
                }
            }
            catch (Exception ex)
            {
                //TODO Log
                Downloader_OnEndDownload();
            }
        }
        #endregion
        #region baDownloadMediaCancel_Click
        private void baDownloadMediaCancel_Click(object sender, RoutedEventArgs e)
        {
            this.downloader?.Cancel();
        }
        #endregion

        #endregion
        #region Application

        #region UnhandledException_Raised

        private void UnhandledException_Raised(Exception exception, string source)
        {
            string message = null;
            try
            {
                message = $"Unhandled exception in {source}, exception: {exception?.ToString()}.";
                message += $"Exception in {Utils.GetAssemblyName()} v{Utils.GetAssemblyVersion()}";
            }
            catch (Exception ex)
            {
                Logger.Log(EventID.Application.UnhandledExceptionException, ex);
            }
            finally
            {
                Logger.Log(EventID.Application.UnhandledException, message);
            }
        }

        #endregion
        #region WDIYoutubeDonwloader_Closing

        private void WDIYoutubeDonwloader_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Logger.Log(EventID.Application.End);
                try { Logger.Close(); } catch (Exception ex) { Console.WriteLine(ex.ToString()); Logger.Log(EventID.Application.Exception, ex); }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion

        #endregion

#endregion

        #region Methods

        #region InitializeControls
        private void InitializeControls()
        {
            try
            {
                Logger.Initialize();
                Logger.Log(EventID.Application.Start);
                //TODO Create Own style
                SfSkinManager.ApplyStylesOnApplication = true;
                SfSkinManager.SetVisualStyle(this, VisualStyles.Office2013DarkGray);

                #region GlobalUnhandledExceptionEvents

                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                    UnhandledException_Raised((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

                Application.Current.DispatcherUnhandledException += (s, e) =>
                    UnhandledException_Raised(e.Exception, "Application.Current.DispatcherUnhandledException");

                TaskScheduler.UnobservedTaskException += (s, e) =>
                    UnhandledException_Raised(e.Exception, "TaskScheduler.UnobservedTaskException");

                #endregion
            }
            catch(Exception ex)
            {
                Logger.Log(EventID.Application.Exception, ex);
            }
        }
        #endregion
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
        #region RefreshControls
        private void RefreshControls(Media toBind)
        {
            baThumbnail.LargeIcon = null;
            GC.Collect();
            baThumbnail.LargeIcon = Utils.ToBitmapImage(toBind?.Thumbnail ?? ResourceImage128.YouTube);

            tbeTitleAuthor.Text = toBind?.ToString() ?? null;
            mtbDuration.Value = (toBind?.Duration ?? new TimeSpan()).ToString("c");
            rMediaRating.Value = toBind?.AverageRatings ?? 0.0;
            baMediaLikes.Label = toBind?.LikesCount.ToString() ?? 0.ToString();
            baMediaDislikes.Label = toBind?.DislikesCount.ToString() ?? 0.ToString();

            List<DownloadMenuItem> ddmiList = new List<DownloadMenuItem>();
            if (toBind != null)
            {
                foreach (MediaType ytmtItem in toBind.MediaTypes)
                {
                    ddmiList.Add(new DownloadMenuItem(ytmtItem.ToString(), "Images/32x32/Download.png", ytmtItem));
                }
            }

            ddbaDownloadMedia.DataContext = new DownloadMenuCollection(ddmiList);
            ddbaDownloadMedia.IsEnabled = toBind != null;
        }
        #endregion
        #region LoadMediaInfoAsync
        private void LoadMediaInfoAsync(string url)
        {
            new Task(() => {
                if (!String.IsNullOrWhiteSpace(url) && 
                   (this.downloadingMedia == null || (this.downloadingMedia != null && !String.Equals(this.downloadingMedia.Url, url))))
                {
                    Media ytMedia = this.downloader.GetMediaInfo(url);
                    this.downloadingMedia?.Dispose();
                    this.downloadingMedia = ytMedia;
                }
            }).Start();
        }
        #endregion
        #region DownloadMediaAsync
        private void DownloadMediaAsync(Downloader downloader, Media downloadingMedia, MediaType downloadinMediaType)
        {
            if (downloadinMediaType != null && downloadingMedia != null)
            {
                try
                {
                    new Task(() =>
                    {
                        using (MemoryStream downloadStream = this.downloader.Download(downloadingMedia.MediaId, downloadinMediaType))
                        {
                            if (downloadStream != null)
                            {
                                string fileName = downloadingMedia.Title;
                                string fileExtension = downloadinMediaType.Extension.ToString().ToLower();

                                System.IO.Path.GetInvalidFileNameChars().Select(c => fileName.Replace(c, ' '));

                                SaveFileDialog dialog = new SaveFileDialog()
                                {
                                    Filter = $"*{fileExtension} files |*.{fileExtension}",
                                    DefaultExt = $"*.{fileExtension}",
                                    InitialDirectory = Utils.GetDownloadFolderPath(),
                                    FileName = $"{fileName}.{fileExtension}"
                                };

                                if (dialog.ShowDialog() == true)
                                {
                                    File.WriteAllBytes(dialog.FileName, downloadStream.ToArray());
                                }
                                downloadStream.Close();
                                downloadStream.Dispose();
                            }
                        }
                    }).Start();
                }
                catch (Exception ex)
                {
                    //TODO Log
                    Downloader_OnEndDownload();
                }
            }
        }
        #endregion

        #endregion

    }
}
