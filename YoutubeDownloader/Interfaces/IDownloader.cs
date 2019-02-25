using DIYoutubeDownloader.Models;
using System;
using System.Drawing;
using System.IO;

namespace DIYoutubeDownloader
{
    public delegate void Progress(double progress);
    public delegate void BeginDownload();
    public delegate void EndDownload();
    public delegate void BeginLoadMediaInfo();
    public delegate void EndLoadMediaInfo(Media mediaInfo);

    public interface IDownloader : IDisposable
    {
        bool IsDownloading();
        Media GetMediaInfo(string url);
        string GetThumbnailUrl(string mediaUrl);
        Bitmap GetThumbnail(string mediaUrl);
        MemoryStream Download(string mediaId, MediaType mediaType);
        void Cancel();

        event Progress OnProgress;
        event BeginDownload OnBeginDownload;
        event EndDownload OnEndDownload;
        event BeginLoadMediaInfo OnBeginLoadMediaInfo;
        event EndLoadMediaInfo OnEndLoadMediaInfo;
    }
}
