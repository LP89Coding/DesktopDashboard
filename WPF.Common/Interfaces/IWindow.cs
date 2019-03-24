using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Common.Common;

namespace WPF.Common.Interfaces
{
    public interface IWindow
    {
        void Show();
        void Close();
        void SetContent(IWindowControl windowControl);
        WindowState GetWindowState();
    }
}
