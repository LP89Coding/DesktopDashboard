using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogData = WPF.Common.Logger.LogData;
using LogLevel = WPF.Common.Enums.LogLevel;

namespace DesktopDashboard.Internals
{
    internal class EventID
    { 
        /// <summary>
        /// Reserved EventID Range <9 000; 9 999>
        /// </summary>
        internal class DesktopDashboard
        {
            /// <summary>
            /// Reserved EventID Range <9 000; 9 029>
            /// </summary>
            internal class Application
            {
                public static LogData Start = new LogData(9000, LogLevel.Info, "Start");
                public static LogData End = new LogData(9001, LogLevel.Info, "End");
                public static LogData Exception = new LogData(9002, LogLevel.Error, "Error: {0}");
                public static LogData UnhandledException = new LogData(9003, LogLevel.Fatal, "Unhandled error: {0}");
                public static LogData UnhandledExceptionException = new LogData(9004, LogLevel.Fatal, "Unhandled error error: {0}");
            }
        }
    }
}
