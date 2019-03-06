using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WPF.Common.Common
{
    public class Utils
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

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
        #region CloseActiveWindow

        public static void CloseActiveWindow()
        {
            IntPtr active = GetActiveWindow();
            System.Windows.Window activeWindow = null;
            if (active != null)
            {
                activeWindow = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>()
                    .SingleOrDefault(window => new System.Windows.Interop.WindowInteropHelper(window).Handle == active);
            }
            if (activeWindow != null)
                activeWindow.Close();
            else
                System.Windows.Application.Current.Shutdown();
        }

        #endregion
    }
}
