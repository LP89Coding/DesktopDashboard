using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.IO;

namespace DIYoutubeDownloader.Internal
{
    internal class Utils
    {
        #region ToBitmapImage
        public static BitmapImage ToBitmapImage(object toConvert)
        {
            return DesktopDashboard.Common.Utils.ToBitmapImage(toConvert);
        }

        #endregion
        #region GetDownloadFolderPath

        public static string GetDownloadFolderPath()
        {
            return Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads");
        }

        #endregion
        #region GetAssemblyName

        public static string GetAssemblyName()
        {
            return System.Reflection.Assembly.GetExecutingAssembly()?.GetName()?.Name;
        }

        #endregion
        #region GetAssemblyVersion

        public static Version GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly()?.GetName()?.Version;
        }

        #endregion
    }
}
