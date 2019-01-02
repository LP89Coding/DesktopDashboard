using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDashboard.Common
{
    public interface IObjectState
    {
        Enums.ObjectState State { get; set; }
    }
}
