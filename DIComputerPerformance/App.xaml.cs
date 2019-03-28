﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

using WPF.Common.Interfaces;
using WPF.Common.Logger;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;

using DIComputerPerformance.Common;
using DIComputerPerformance.Internals;

namespace DIComputerPerformance
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
            Stopwatch sw = new Stopwatch();
            try
            {
                sw.Start();
                Logger.Log(EventID.DIComputerPerformance.Application.StartupEnter);
                base.OnStartup(e);
                ArgumentCollection args = new ArgumentCollection();
                plugin.InitializePlugin(args);
                plugin.GetPluginWindow()?.Show();
            }
            catch(Exception ex)
            {
                Logger.Log(EventID.DIComputerPerformance.Application.Exception, nameof(OnStartup), ex);
            }
            finally
            {
                sw.Stop();
                Logger.Log(EventID.DIComputerPerformance.Application.StartupExit, sw.ElapsedMilliseconds);
            }
        }

        #endregion
        #region OnExit

        protected override void OnExit(ExitEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            try
            {
                sw.Start();
                Logger.Log(EventID.DIComputerPerformance.Application.EndEnter);
                base.OnExit(e);
            }
            catch (Exception ex)
            {
                Logger.Log(EventID.DIComputerPerformance.Application.Exception, nameof(OnStartup), ex);
            }
            finally
            {
                sw.Stop();
                Logger.Log(EventID.DIComputerPerformance.Application.EndExit, sw.ElapsedMilliseconds);
            }
        }

        #endregion

        #endregion
    }
}
