using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Controls.Input;

namespace DIYoutubeDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string MainWindowTitle = "Youtube downloader"; 

        private YoutubeDownloader downloader { get; }
        private YoutubeMedia downloadingMedia { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeControls();

            this.downloader = new YoutubeDownloader();
            this.downloader.OnProgress += Downloader_OnProgress;
            this.downloader.OnBeginDownload += Downloader_OnBeginDownload;
            this.downloader.OnEndDownload += Downloader_OnEndDownload;
            this.downloader.OnBeginLoadMediaInfo += Downloader_OnBeginLoadMediaInfo;
            this.downloader.OnEndLoadMediaInfo += Downloader_OnEndLoadMediaInfo;
        }

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

        private void Downloader_OnEndLoadMediaInfo(YoutubeMedia mediaInfo)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.RefreshControls(mediaInfo);

                    ddbaDownloadMedia.IsEnabled = mediaInfo != null;
                    baFindMedia.IsEnabled = true;
                    biMediaLoader.Visibility = Visibility.Hidden;
                });
            }
            catch (TaskCanceledException ex)
            {
                //TODO Log
                Downloader_OnEndLoadMediaInfo(null);
            }
        }

        private void Downloader_OnBeginDownload()
        {
            try
            {
                this.Dispatcher.Invoke(() => {
                    this.tbeUrl.IsEnabled = false;
                    this.ddbaDownloadMedia.Visibility = Visibility.Hidden;
                    this.baDownloadMediaCancel.Visibility = Visibility.Visible;

                    this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal;
                    this.TaskbarItemInfo.ProgressValue = 0;
                });
            }
            catch (Exception ex)
            {
                //TODO Log
            }
        }

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
                });
            }
            catch (TaskCanceledException ex)
            {
                //TODO Log
            }
        }

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

        private void InitializeControls()
        {
            //TODO Create Own style
            SfSkinManager.ApplyStylesOnApplication = true;
            SfSkinManager.SetVisualStyle(this, VisualStyles.Office2013DarkGray);
        }

        private void RefreshControls(YoutubeMedia toBind)
        {
            baThumbnail.LargeIcon = Utils.ToBitmapImage(toBind?.Thumbnail ?? ResourceImage128.YouTube);
            tbeTitleAuthor.Text = toBind?.ToString() ?? null;
            mtbDuration.Value = (toBind?.Duration ?? new TimeSpan()).ToString("c");
            rMediaRating.Value = toBind?.AverageRatings ?? 0.0;
            baMediaLikes.Label = toBind?.LikesCount.ToString() ?? 0.ToString();
            baMediaDislikes.Label = toBind?.DislikesCount.ToString() ?? 0.ToString();

            List<DropDownMenuItem> ddmiList = new List<DropDownMenuItem>();
            if(toBind != null)
            {
                foreach(YoutubeMediaType ytmtItem in toBind.MediaTypes)
                {
                    ddmiList.Add(new DropDownMenuItem(ytmtItem.ToString(), "Images/32x32/Download.png", ytmtItem));
                }
            }

            ddbaDownloadMedia.DataContext = new DropDownMenuCollection(ddmiList);
        }
        
        private void DownloadMediaAsync(YoutubeDownloader downloader, YoutubeMedia downloadingMedia, YoutubeMediaType downloadinMediaType)
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
                                string fileExtension = downloadinMediaType.MediaType.ToString().ToLower();

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
                            }
                        }
                    }).Start();
                }
                catch(Exception ex)
                {
                    //TODO Log
                    Downloader_OnEndDownload();
                }
            }
        }

        private void LoadMediaInfoAsync(string url)
        {
            new Task(() => {
                if (!String.IsNullOrWhiteSpace(url))
                {
                    this.downloadingMedia = this.downloader.GetMediaInfo(url);
                }
            }).Start();
        }

        private void ddbaDownloadMedia_DropDownMenuItemClick_(object sender, RoutedEventArgs e)
        {
            try
            {
                Syncfusion.Windows.Tools.Controls.DropDownMenuItem ddmiItem = sender as Syncfusion.Windows.Tools.Controls.DropDownMenuItem;
                if (ddmiItem != null)
                {
                    YoutubeMediaType ytmtItem = ddmiItem.Tag as YoutubeMediaType;
                    DownloadMediaAsync(this.downloader, this.downloadingMedia, ytmtItem);
                }
            }
            catch(Exception ex)
            {
                //TODO Log
                Downloader_OnEndDownload();
            }
        }

        private void baDownloadMediaCancel_Click(object sender, RoutedEventArgs e)
        {
            this.downloader?.Cancel();
        }

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
    }
}
