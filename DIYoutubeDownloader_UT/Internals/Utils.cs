using System;
using System.Diagnostics;
using System.Threading;

using DIYoutubeDownloader.Internal;
using DIYoutubeDownloader.Models;
using DIYoutubeDownloader.ViewModels;

using DIYoutubeDownloader_UT.Mocks;

namespace DIYoutubeDownloader_UT.Internal
{
    public class Utils
    {
        public static YoutubeDownloaderViewModel InitilizeViewModel()
        {
            MockDownloader downloader = new MockDownloader();

            ArgumentCollection args = new ArgumentCollection();
            args.Set(ArgumentCollection.ArgumentType.Downloader, downloader);
            return Tools.CreateYoutubeDownloaderViewModel(args) as YoutubeDownloaderViewModel;
        }

        public static Media GetMediaInfo_GivenUrl_MediaInfo(YoutubeDownloaderViewModel youtubeDownloaderViewModel)
        {
            Media result = null;
            Stopwatch watch = new Stopwatch();
            try
            {
                youtubeDownloaderViewModel.FindMediaButtonCommand.Execute("http://testUrl.com?ID=test");
                ManualResetEvent stopEvent = new ManualResetEvent(false);
                TimeSpan maxWaitTime = TimeSpan.FromSeconds(2);
                watch.Start();
                while (!stopEvent.WaitOne(100, false))
                {
                    if (!youtubeDownloaderViewModel.IsDownloading ||
                        stopEvent.WaitOne(0, false) || maxWaitTime.TotalMilliseconds < watch.ElapsedMilliseconds)
                        break;
                }
                watch.Stop();
                result = youtubeDownloaderViewModel.Media;
            }
            catch (Exception ex)
            {
                result = null;
            }
            finally
            {
                watch = null;
            }
            return result;
        }
    }
}
