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
        public static BitmapImage ToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            if (bitmap == null)
                return null;
            BitmapImage bitmapImage = new BitmapImage();
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
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

        public static string GetDownloadFolderPath()
        {
            return Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads");
        }
    }
}
