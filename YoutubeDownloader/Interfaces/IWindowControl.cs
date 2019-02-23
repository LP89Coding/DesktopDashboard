using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIYoutubeDownloader
{
    public interface IWindowControl
    {
        void SubscribePropertyChangeNotification(System.ComponentModel.PropertyChangedEventHandler propertyChangedHandler);
    }
}
