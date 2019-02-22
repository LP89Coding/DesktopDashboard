using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shell;

namespace DIYoutubeDownloader
{
    public interface IWindowPropertyChangeNotifier
    {
        double TaskBarProgressValue { get; set; }
        TaskbarItemProgressState TaskBarProgressState { get; set; }
    }
}
