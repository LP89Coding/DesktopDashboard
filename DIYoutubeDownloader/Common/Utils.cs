using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using DIYoutubeDownloader.Internal;

using WPF.Common.Interfaces;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;

namespace DIYoutubeDownloader.Common
{
    public class Utils
    {
        #region CreateYoutubeDownloaderViewModel

        public static IViewModel CreateYoutubeDownloaderViewModel(ArgumentCollection args = null)
        {
            return new YoutubeDownloaderViewModelFactory().CreateViewModel(args: args);
        }

        #endregion
    }
}
