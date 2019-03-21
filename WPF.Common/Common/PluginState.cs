using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Common.Common
{
    public class PluginState
    {
        public string Name { get; private set; }
        public bool IsActive { get; private set; }

        public WindowState WindowState { get; set; }

        public PluginState(string name, bool isActive)
        {
            this.Name = name;
            this.IsActive = isActive;
        }
    }
}
