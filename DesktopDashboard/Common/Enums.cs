using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDashboard.Common
{
    public class Enums
    {
        public enum ImageExtension
        {
            Uknown,
            PNG
        }
        public enum ImageName
        {
            Unknown,
            Close,
            Lock,
            LockOpen,
        }
        public enum ImageSize
        {
            _48x48
        }
        public enum LogLevel
        {
            Trace = 1,
            Debug = 2,
            Info = 3,
            Warn = 4,
            Error = 5,
            Fatal = 6,
            Off = 7,
        }
        public enum ObjectState
        {
            Unknown = 0,
            New = 1,
            Modified = 2,
            Loaded = 3,
            Delete = 4,
            Incorect = 5,
        }
    }
}
