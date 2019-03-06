﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using DesktopDashboard.Interfaces;
using ArgumentCollection = DesktopDashboard.Common.ArgumentCollection;

using DIYoutubeDownloader.Internal;
using DIYoutubeDownloader.Common;
using EventID = DIYoutubeDownloader.Internal.EventID.DIYoutubeDownloader;

namespace DIYoutubeDownloader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IPlugin plugin = new Plugin();
        #region Overrides

        #region OnStartup

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ArgumentCollection args = new ArgumentCollection();
            plugin.InitializePlugin(null);
        }

        #endregion
        #region OnExit

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        #endregion

        #endregion
    }
}
