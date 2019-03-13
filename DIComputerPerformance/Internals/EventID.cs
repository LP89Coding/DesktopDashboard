using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogData = WPF.Common.Logger.LogData;
using LogLevel = WPF.Common.Enums.LogLevel;

namespace DIComputerPerformance.Internals
{
    internal class EventID
    {
        /// <summary>
        /// Reserved EventID Range <11 000; 11 999>
        /// </summary>
        internal class DIComputerPerformance
        {
            /// <summary>
            /// Reserved EventID Range <11 000; 11 029>
            /// </summary>
            internal class Application
            {
                public static LogData Start = new LogData(11000, LogLevel.Info, "DIComputerPerformance start");
                public static LogData End = new LogData(11001, LogLevel.Info, "DIComputerPerformance end");
                public static LogData Exception = new LogData(11002, LogLevel.Error, "DIComputerPerformance Error: {0}");
                public static LogData UnhandledException = new LogData(11003, LogLevel.Fatal, "Unhandled error: {0}");
                public static LogData UnhandledExceptionException = new LogData(11004, LogLevel.Fatal, "Unhandled error error: {0}");
            }
        }
    }
}
