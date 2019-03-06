using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Common.Interfaces;

namespace WPF.Common.Logger
{
    public class Logger
    {
        private static readonly ILogger LoggerInstane = new NLogLogger();

        public static void Close()
        {
            LoggerInstane.Close();
        }

        public static void Initialize()
        {
            LoggerInstane.Initialize();
        }

        public static void Log(LogData logData, params object[] args)
        {
            LoggerInstane.Log(logData, args);
        }
    }
}
