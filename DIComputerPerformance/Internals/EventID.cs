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
                public static LogData StartupEnter = new LogData(11000, LogLevel.Info, "DIComputerPerformance Start Enter");
                public static LogData EndEnter = new LogData(11001, LogLevel.Info, "DIComputerPerformance End Enter");
                public static LogData Exception = new LogData(11002, LogLevel.Error, "DIComputerPerformance. Code: {0}, Error: {1}");
                public static LogData UnhandledException = new LogData(11003, LogLevel.Fatal, "Unhandled error: {0}");
                public static LogData UnhandledExceptionException = new LogData(11004, LogLevel.Fatal, "Unhandled error error: {0}");
                public static LogData StartupExit = new LogData(11005, LogLevel.Info, "DIComputerPerformance Start Exit ({0}ms)");
                public static LogData EndExit = new LogData(11006, LogLevel.Info, "DIComputerPerformance End Exit ({0}ms)");
                public static LogData InitializeComputerPerformanceEnter = new LogData(11007, LogLevel.Info, "Start Initialize ComputerPerformance");
                public static LogData InitializeComputerPerformanceExit = new LogData(11008, LogLevel.Info, "End Initialize ComputerPerformance ({0}ms)");
                public static LogData PluginCloseEnter = new LogData(11009, LogLevel.Info, "Plugin Close Enter");
                public static LogData PluginCloseExit = new LogData(11010, LogLevel.Info, "Plugin Close Exit ({0}ms)");
            }
        }
    }
}
