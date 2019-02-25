using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DIYoutubeDownloader.Internal
{
    public class Tools
    {
        #region CreateYoutubeDownloaderViewModel

        public static IViewModel CreateYoutubeDownloaderViewModel(ArgumentCollection args = null)
        {
            return new YoutubeDownloaderViewModelFactory().CreateViewModel(args: args);
        }

        #endregion

        #region ToBitmapImage

        public static BitmapImage ToBitmapImage(object toConvert)
        {
            return Utils.ToBitmapImage(toConvert);
        }

        #endregion
    }
}
