using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Common.Interfaces
{
    public interface ILogger
    {
        void Initialize();
        void Close();
        void Log(WPF.Common.Logger.LogData logData, params object[] args);
    }
}
