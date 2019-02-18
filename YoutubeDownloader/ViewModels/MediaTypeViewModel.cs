using DIYoutubeDownloader.Internal;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DIYoutubeDownloader.ViewModels
{
    public class MediaTypeViewModel : ObservableViewModel
    {
        private IDownloader downloader;
        private Media downloadingMedia;

        private MediaType mediaType;
        public MediaType MediaType {  get { return this.mediaType; } private set { this.mediaType = value; } }

        private ICommand downloadMediaButtonCommand;
        public ICommand DownloadMediaButtonCommand { get { return this.downloadMediaButtonCommand; } private set { this.downloadMediaButtonCommand = value; } }

        public MediaTypeViewModel(MediaType mediaType, IDownloader downloader, Media downloadingMedia)
        {
            if (mediaType == null)
                throw new Exception("MediaTypes cannot be empty");
            if (downloader == null)
                throw new Exception("Downloader cannot be empty");
            if (downloadingMedia == null)
                throw new Exception("Downloading media cannot be empty");
            this.MediaType = mediaType;
            this.downloader = downloader;
            this.downloadingMedia = downloadingMedia;
            this.DownloadMediaButtonCommand = new Command((object parameter) => { DownloadMediaAsync(); });
        }

        #region DownloadMediaAsync
        public void DownloadMediaAsync()
        {
            MediaType downloadingMediaType = this.MediaType;
            if (downloadingMediaType != null && downloadingMediaType != null)
            {
                try
                {
                    new Task(() =>
                    {
                        using (MemoryStream downloadStream = this.downloader.Download(downloadingMedia.MediaId, downloadingMediaType))
                        {
                            if (downloadStream != null)
                            {
                                string fileName = downloadingMedia.Title;
                                string fileExtension = downloadingMediaType.Extension.ToString().ToLower();

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
                    Utils.Logger.Log(EventID.DIYoutubeDownloader.Application.Exception, ex);
                }
            }
        }
        #endregion

        public override string ToString()
        {
            return this.mediaType?.ToString();
        }
    }
}
