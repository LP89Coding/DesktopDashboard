using System;
using System.Threading;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DIYoutubeDownloader.ViewModels;
using DIYoutubeDownloader.Models;

using DIYoutubeDownloader_UT.Internal;

namespace DIYoutubeDownloader_UT
{
    [TestClass]
    public class UnitTest
    {
        const int MAX_WAIT_TIME_MS = 2000;

        [TestInitialize]
        public void Initilize()
        {
        }

        [TestCleanup]
        public void Cleanup()
        {
        }


        [TestMethod]
        public void GetMediaInfo_GivenUrl_MediaInfo()
        {
            bool result = false;
            Stopwatch watch = new Stopwatch();
            try
            {
                YoutubeDownloaderViewModel youtubeDownloaderViewModel = Utils.InitilizeViewModel();
                Media mediaResult = Utils.GetMediaInfo_GivenUrl_MediaInfo(youtubeDownloaderViewModel);
                result = (mediaResult?.Author.Equals("TestAuthor") ?? false) &&
                            (youtubeDownloaderViewModel.DownloadMediaTypes?.Count == 2);
            }catch(Exception ex)
            {
                result = false;
            }
            finally
            {
                watch = null;
            }
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void GetMediaInfo_EmptyUrl_EmtyMediaInfo()
        {
            bool result = false;
            Stopwatch watch = new Stopwatch();
            try
            {
                YoutubeDownloaderViewModel youtubeDownloaderViewModel = Utils.InitilizeViewModel();
                youtubeDownloaderViewModel.FindMediaButtonCommand.Execute(null);
                ManualResetEvent stopEvent = new ManualResetEvent(false);
                TimeSpan maxWaitTime = TimeSpan.FromMilliseconds(MAX_WAIT_TIME_MS);
                watch.Start();
                while (!stopEvent.WaitOne(100, false))
                {
                    if (!youtubeDownloaderViewModel.IsDownloading || stopEvent.WaitOne(0, false) || maxWaitTime.TotalMilliseconds < watch.ElapsedMilliseconds)
                        break;
                }
                result = youtubeDownloaderViewModel.Media == null;
                watch.Stop();
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                watch = null;
            }
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void Download_AllowComplete_DownloadProceeded()
        {
            bool result = false;
            Stopwatch watch = new Stopwatch();
            try
            {
                ManualResetEvent stopEvent = new ManualResetEvent(false);
                YoutubeDownloaderViewModel youtubeDownloaderViewModel = Utils.InitilizeViewModel();
                Media mediaResult = Utils.GetMediaInfo_GivenUrl_MediaInfo(youtubeDownloaderViewModel);
                if (mediaResult == null)
                    throw new Exception("MediaResult not loaded");
                MediaType mediaType = youtubeDownloaderViewModel.DownloadMediaTypes[0].MediaType;
                if (!youtubeDownloaderViewModel.DownloadMediaTypes[0].DownloadMediaButtonCommand.CanExecute(mediaType))
                    throw new Exception("Media not allowed to download");
                youtubeDownloaderViewModel.DownloadMediaTypes[0].DownloadMediaButtonCommand.Execute(mediaType);
                stopEvent.WaitOne(100);
                if (!youtubeDownloaderViewModel.IsDownloading)
                    throw new Exception("Media not downloading");

                TimeSpan maxWaitTime = TimeSpan.FromMilliseconds(MAX_WAIT_TIME_MS);
                watch.Start();
                while (!stopEvent.WaitOne(50, false))
                {
                    if (!youtubeDownloaderViewModel.IsDownloading ||
                        stopEvent.WaitOne(0, false) || maxWaitTime.TotalMilliseconds < watch.ElapsedMilliseconds)
                        break;
                }
                result = true;
                watch.Stop();
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                watch = null;
            }
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void Download_CancelDownload_StopDownload()
        {
            bool result = false;
            try
            {
                ManualResetEvent stopEvent = new ManualResetEvent(false);
                YoutubeDownloaderViewModel youtubeDownloaderViewModel = Utils.InitilizeViewModel();
                Media mediaResult = Utils.GetMediaInfo_GivenUrl_MediaInfo(youtubeDownloaderViewModel);
                if (mediaResult == null)
                    throw new Exception("MediaResult not loaded");
                MediaType mediaType = youtubeDownloaderViewModel.DownloadMediaTypes[0].MediaType;
                if (!youtubeDownloaderViewModel.DownloadMediaTypes[0].DownloadMediaButtonCommand.CanExecute(mediaType))
                    throw new Exception("Media not allowed to download");
                youtubeDownloaderViewModel.DownloadMediaTypes[0].DownloadMediaButtonCommand.Execute(mediaType);
                stopEvent.WaitOne(100);
                if (!youtubeDownloaderViewModel.IsDownloading)
                    throw new Exception("Media not downloading");

                youtubeDownloaderViewModel.CancelButtonCommand.Execute(null);
                stopEvent.WaitOne(200);
                if (!youtubeDownloaderViewModel.IsDownloading)
                    result = true;
                else
                    throw new Exception("Cancel download not working");
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
            }
            Assert.IsTrue(result);
        }
    }
}
