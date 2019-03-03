using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArgumentCollection = DesktopDashboard.Common.ArgumentCollection;

namespace DesktopDashboard.Interfaces
{
    public interface IPlugin
    {
        void InitializePlugin(ArgumentCollection args);

        ArgumentCollection GetArgs();

        string GetPluginName();
        Icon GetPluginIcon();

        void ClosePlugin();
    }
}
