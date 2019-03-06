using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Common
{
    public class Enums
    {
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
