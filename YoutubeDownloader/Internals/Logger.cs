using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIYoutubeDownloader.Internal
{
    internal class Logger : ILogger
    {
        private NLog.Logger LoggerInstance = null;
        private bool InitializationFailed = false;
        private bool Closed = false;

        #region GetLogLevel
        private NLog.LogLevel GetLogLevel(Enums.LogLevel logLevel)
        {
            switch (logLevel)
            {
                case Enums.LogLevel.Trace:
                    return NLog.LogLevel.Trace;
                case Enums.LogLevel.Debug:
                    return NLog.LogLevel.Debug;
                case Enums.LogLevel.Info:
                    return NLog.LogLevel.Info;
                case Enums.LogLevel.Warn:
                    return NLog.LogLevel.Warn;
                case Enums.LogLevel.Error:
                    return NLog.LogLevel.Error;
                case Enums.LogLevel.Fatal:
                    return NLog.LogLevel.Fatal;
                default:
                    return NLog.LogLevel.Off;
            }
        }
        #endregion

        #region ILogger implementation

        #region Initialize
        public void Initialize()
        {
            try
            {
                LoggerInstance = NLog.LogManager.GetCurrentClassLogger();
                Closed = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                InitializationFailed = true;
            }
        }
        #endregion
        #region Close
        public void Close()
        {
            try
            {
                NLog.LogManager.Shutdown();
                Closed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion
        #region Log
        public void Log(LogData logData, params object[] args)
        {
            if (logData != null && !InitializationFailed && !Closed)
            {
                if (LoggerInstance == null)
                    Initialize();
                NLog.LogEventInfo lei = new NLog.LogEventInfo(this.GetLogLevel(logData.LogLevel), LoggerInstance.Name, null, logData.Message, args);
                // this data can be retrieved using ${event-context:EventID}
                lei.Properties["EventID"] = logData.EventID;
                LoggerInstance.Log(lei);
            }
        }
        #endregion

        #endregion
    }
}
