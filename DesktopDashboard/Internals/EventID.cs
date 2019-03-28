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
                public static LogData StartupEnter = new LogData(9000, LogLevel.Info, "DesktopDashboard Start Enter");
                public static LogData EndEnter = new LogData(9001, LogLevel.Info, "DesktopDashboard End Enter");
                public static LogData Exception = new LogData(9002, LogLevel.Error, "DesktopDashboard. Code: {0}, Error: {1}");
                public static LogData UnhandledException = new LogData(9003, LogLevel.Fatal, "Unhandled error: {0}");
                public static LogData UnhandledExceptionException = new LogData(9004, LogLevel.Fatal, "Unhandled error error: {0}");
                public static LogData StartupExit = new LogData(9005, LogLevel.Info, "DesktopDashboard Start Exit ({0}ms)");
                public static LogData EndExit = new LogData(9006, LogLevel.Info, "DesktopDashboard End Exit ({0}ms)");
                public static LogData FoundedPlugins = new LogData(9007, LogLevel.Trace, "Founded plugins (Count: {0}): {1}");
                public static LogData FoundedAvailablePlugins = new LogData(9008, LogLevel.Trace, "Founded available plugins (Count: {0}): {1}");
                public static LogData InitializeDesktopDashboardEnter = new LogData(9009, LogLevel.Info, "Start Initialize DesktopDashboard");
                public static LogData InitializeDesktopDashboardExit = new LogData(9010, LogLevel.Info, "End Initialize DesktopDashboard ({0}ms)");
            }
        }
    }
}
