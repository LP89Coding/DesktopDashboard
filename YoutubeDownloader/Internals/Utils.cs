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
        public static ILogger Logger { get; } = new Logger();

        #region ToBitmapImage
        public static BitmapImage ToBitmapImage(object toConvert)
        {
            if (toConvert == null)
                return null;
            if (toConvert is System.Drawing.Bitmap)
                return ToBitmapImage(toConvert as System.Drawing.Bitmap);
            else if (toConvert is System.Drawing.Icon)
                return ToBitmapImage(toConvert as System.Drawing.Icon);
            else
                return null;
        }
        public static BitmapImage ToBitmapImage(System.Drawing.Bitmap toConvert)
        {
            if (toConvert == null)
                return null;
            BitmapImage bitmapImage = new BitmapImage();
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                toConvert.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                memory.Close();
                memory.Dispose();
            }
            bitmapImage.Freeze();
            return bitmapImage;
        }
        public static BitmapImage ToBitmapImage(System.Drawing.Icon toConvert)
        {
            if (toConvert == null)
                return null;
            BitmapImage bitmapImage = new BitmapImage();

            using (System.Drawing.Bitmap bitmap = toConvert.ToBitmap())
            {
                bitmapImage = ToBitmapImage(bitmap);
            }
            return bitmapImage;
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
