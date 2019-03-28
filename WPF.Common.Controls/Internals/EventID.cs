using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogData = WPF.Common.Logger.LogData;
using LogLevel = WPF.Common.Enums.LogLevel;

namespace WPF.Common.Controls.Internals
{
    internal class EventID
    {
        /// <summary>
        /// Reserved EventID Range <12 000; 12 999>
        /// </summary>
        internal class WPFCommonControls
        {
            /// <summary>
            /// Reserved EventID Range <12 000; 12 029>
            /// </summary>
            internal class Application
            {
                public static LogData Exception = new LogData(11002, LogLevel.Error, "WPF.Common.Controls. Code: {0}, Error: {1}");
            }
        }
    }
}
