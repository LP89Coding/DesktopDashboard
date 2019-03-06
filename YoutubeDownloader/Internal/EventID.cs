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
                public static LogData Start = new LogData(10000, LogLevel.Info, "DIYoutubeDowbloader start");
                public static LogData End = new LogData(10001, LogLevel.Info, "DIYoutubeDowbloader end");
                public static LogData Exception = new LogData(10002, LogLevel.Error, "DIYoutubeDowbloader Error: {0}");
                public static LogData UnhandledException = new LogData(10003, LogLevel.Fatal, "Unhandled error: {0}");
                public static LogData UnhandledExceptionException = new LogData(10004, LogLevel.Fatal, "Unhandled error error: {0}");
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
