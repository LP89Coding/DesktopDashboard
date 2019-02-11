using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIYoutubeDownloader.Internal
{
    public class LogData
    {
        public long EventID { get; private set; }
        public Enums.LogLevel LogLevel { get; private set; }
        public string Message { get; private set; }

        public LogData(long EventID, Enums.LogLevel LogLevel, string Message)
        {
            this.EventID = EventID;
            this.LogLevel = LogLevel;
            this.Message = Message;
        }
    }
}
