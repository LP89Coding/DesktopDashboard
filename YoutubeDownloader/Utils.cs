using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace DIYoutubeDownloader
{
    public class Utils
    {
        public static BitmapImage ToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            if (bitmap == null)
                return null;
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                memory.Close();

                return bitmapImage;
            }
        }
    }
}
