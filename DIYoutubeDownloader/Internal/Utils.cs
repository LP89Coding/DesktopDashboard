﻿using System;
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
        #region GetDownloadFolderPath

        public static string GetDownloadFolderPath()
        {
            return Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads");
        }

        #endregion

    }
}
