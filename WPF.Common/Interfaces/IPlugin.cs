using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArgumentCollection = WPF.Common.Common.ArgumentCollection;

namespace WPF.Common.Interfaces
{
    public interface IPlugin
    {
        void InitializePlugin(ArgumentCollection args);

        ArgumentCollection GetArgs();

        string GetPluginName();
        System.Drawing.Icon GetPluginIcon();

        void ClosePlugin();
    }
}
