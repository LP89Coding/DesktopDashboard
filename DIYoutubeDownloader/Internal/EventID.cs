using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogData = WPF.Common.Logger.LogData;
using LogLevel = WPF.Common.Enums.LogLevel;

namespace DIYoutubeDownloader.Internal
{
    internal class EventID
    { 
        /// <summary>
        /// Reserved EventID Range <10 000; 10 999>
        /// </summary>
        internal class DIYoutubeDownloader
        {
            /// <summary>
            /// Reserved EventID Range <10 000; 10 029>
            /// </summary>
            internal class Application
            {
                public static LogData StartupEnter = new LogData(10000, LogLevel.Info, "DIYoutubeDowbloader Start Enter");
                public static LogData EndEnter = new LogData(10001, LogLevel.Info, "DIYoutubeDowbloader End Enter");
                public static LogData Exception = new LogData(10002, LogLevel.Error, "DIYoutubeDowbloader. Code: {0}, Error: {1}");
                public static LogData UnhandledException = new LogData(10003, LogLevel.Fatal, "Unhandled error: {0}");
                public static LogData UnhandledExceptionException = new LogData(10004, LogLevel.Fatal, "Unhandled error error: {0}");
                public static LogData StartupExit = new LogData(10005, LogLevel.Info, "DIYoutubeDowbloader Start Exit ({0}ms)");
                public static LogData EndExit = new LogData(10006, LogLevel.Info, "DIYoutubeDowbloader End Exit ({0}ms)");
                public static LogData InitializeComputerPerformanceEnter = new LogData(10007, LogLevel.Info, "Start Initialize YoutubeDownloader");
                public static LogData InitializeComputerPerformanceExit = new LogData(10008, LogLevel.Info, "End Initialize YoutubeDownloader ({0}ms)");
                public static LogData PluginCloseEnter = new LogData(10009, LogLevel.Info, "YoutubeDownloader Plugin Close Enter");
                public static LogData PluginCloseExit = new LogData(10010, LogLevel.Info, "YoutubeDownloader Plugin Close Exit ({0}ms)");
            }
            /// <summary>
            /// Reserved EventID Range <10 100; 10 129>
            /// </summary>
            internal class Media
            {
                public static LogData DisposeException = new LogData(10100, LogLevel.Error, "Dispose Error: {0}");
                public static LogData Exception = new LogData(10101, LogLevel.Error, "Error: {0}");
            }
            /// <summary>
            /// Reserved EventID Range <10 130; 10 159>
            /// </summary>
            internal class Downloader
            {
                public static LogData DownloadException = new LogData(10130, LogLevel.Error, "Download Error: {0}"); 
                public static LogData GetMediaInfoException = new LogData(10131, LogLevel.Error, "GetMediaInfo Error: {0}");
                public static LogData GetThumbnailException = new LogData(10132, LogLevel.Error, "GetThumbnail Error: {0}");
                public static LogData CancelException = new LogData(10133, LogLevel.Error, "Cancel Error: {0}");
                public static LogData DisposeException = new LogData(10134, LogLevel.Error, "Dispose Error: {0}");
                public static LogData GetVideoInfoException = new LogData(10135, LogLevel.Error, "GetVideoInfo Error: {0}");
            }
        }
    }
}
