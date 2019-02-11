using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogData = DIYoutubeDownloader.Internal.LogData;
using LogLevel = DIYoutubeDownloader.Internal.Enums.LogLevel;

namespace DIYoutubeDownloader.Internal
{
    internal class EventID
    { 
        /// <summary>
        /// Reserved EventID Range <10 000; 10 999>
        /// </summary>
        internal class DIYoutubeDowbloader
        {
            /// <summary>
            /// Reserved EventID Range <10 000; 10 029>
            /// </summary>
            internal class Application
            {
                public static LogData Start = new LogData(10000, LogLevel.Info, "DIYoutubeDowbloader start");
                public static LogData End = new LogData(10001, LogLevel.Info, "DIYoutubeDowbloader end");
                public static LogData Exception = new LogData(10002, LogLevel.Error, "KeyPresser Error: {0}");
                public static LogData UnhandledException = new LogData(10003, LogLevel.Fatal, "Unhandled error: {0}");
                public static LogData UnhandledExceptionException = new LogData(10004, LogLevel.Fatal, "Unhandled error error: {0}");
            }
        }
    }
}
