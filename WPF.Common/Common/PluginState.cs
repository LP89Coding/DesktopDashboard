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

        public double Height { get; set; }
        public double Width { get; set; }
        public double PositionLeft { get; set; }
        public double PositionTop { get; set; }
        public System.Windows.WindowState WindowState { get; set; }
        public object SerilizedCustomControlContent { get; set; }

        public PluginState(string name, bool isActive)
        {
            this.Name = name;
            this.IsActive = isActive;
        }
    }
}
