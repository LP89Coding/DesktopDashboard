using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDashboard.Interfaces
{
    public interface IWindowControl
    {
        void SubscribePropertyChangeNotification(System.ComponentModel.PropertyChangedEventHandler propertyChangedHandler);
        void UnsubscribePropertyChangeNotification(System.ComponentModel.PropertyChangedEventHandler propertyChangedHandler);
    }
}
