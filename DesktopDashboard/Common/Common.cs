using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDashboard.Common
{
    public class Common
    {

        public static System.Windows.Media.Imaging.BitmapImage GetBitmapImage(Enums.ImageName imageName, Enums.ImageSize imageSize, 
            Enums.ImageExtension imageExtension = Enums.ImageExtension.PNG)
        {
            string str = String.Format("pack://application:,,,/Images/{0}/{1}.{2}", 
                imageSize.ToString().Substring(1, imageSize.ToString().Length-1), 
                imageName.ToString(),
                imageExtension.ToString().ToLower());
            Uri uri = new Uri(str);
            return new System.Windows.Media.Imaging.BitmapImage(uri);
        }
    }
}
