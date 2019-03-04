using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using Logger = DesktopDashboard.Common.Logger;
using ILogger = DesktopDashboard.Interfaces.ILogger;

namespace DesktopDashboard.Common
{
    public class Utils
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


        public static BitmapImage GetBitmapImage(Enums.ImageName imageName, Enums.ImageSize imageSize,
            Enums.ImageExtension imageExtension = Enums.ImageExtension.PNG)
        {
            string str = String.Format("pack://application:,,,/Images/{0}/{1}.{2}",
                imageSize.ToString().Substring(1, imageSize.ToString().Length - 1),
                imageName.ToString(),
                imageExtension.ToString().ToLower());
            Uri uri = new Uri(str);
            return new BitmapImage(uri);
        }
    }
}
