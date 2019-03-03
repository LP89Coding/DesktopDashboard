using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopDashboard.Common;

namespace DesktopDashboard.Interfaces
{
    public interface ILogger
    {
        void Initialize();
        void Close();
        void Log(LogData logData, params object[] args);
    }
}
