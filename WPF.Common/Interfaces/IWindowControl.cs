using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Common.Interfaces
{
    public interface IWindowControl : IDisposable
    {
        void SubscribePropertyChangeNotification(System.ComponentModel.PropertyChangedEventHandler propertyChangedHandler);
        void UnsubscribePropertyChangeNotification(System.ComponentModel.PropertyChangedEventHandler propertyChangedHandler);
    }
}
