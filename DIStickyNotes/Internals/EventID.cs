using LogData = WPF.Common.Logger.LogData;
using LogLevel = WPF.Common.Enums.LogLevel;

namespace DIStickyNotes.Internals
{
    internal class EventID
    {/// <summary>
     /// Reserved EventID Range <12 000; 12 999>
     /// </summary>
        internal class DIStickyNotes
        {
            /// <summary>
            /// Reserved EventID Range <12 000; 12 029>
            /// </summary>
            internal class Application
            {
                public static LogData StartupEnter = new LogData(12000, LogLevel.Info, "DIStickyNotes Start Enter");
                public static LogData EndEnter = new LogData(12001, LogLevel.Info, "DIStickyNotes End Enter");
                public static LogData Exception = new LogData(12002, LogLevel.Error, "DIStickyNotes. Code: {0}, Error: {1}");
                public static LogData UnhandledException = new LogData(12003, LogLevel.Fatal, "Unhandled error: {0}");
                public static LogData UnhandledExceptionException = new LogData(12004, LogLevel.Fatal, "Unhandled error error: {0}");
                public static LogData StartupExit = new LogData(12005, LogLevel.Info, "DIStickyNotes Start Exit ({0}ms)");
                public static LogData EndExit = new LogData(12006, LogLevel.Info, "DIStickyNotes End Exit ({0}ms)");
                public static LogData PluginCloseEnter = new LogData(12007, LogLevel.Info, "StickyNotes Plugin Close Enter");
                public static LogData PluginCloseExit = new LogData(12008, LogLevel.Info, "StickyNotes Plugin Close Exit ({0}ms)");
                public static LogData InitializeStickyNotesEnter = new LogData(12009, LogLevel.Info, "Start Initialize StickyNotes");
                public static LogData InitializeStickyNotesExit = new LogData(12010, LogLevel.Info, "End Initialize StickyNotes ({0}ms)");
            }
        }
    }
}
