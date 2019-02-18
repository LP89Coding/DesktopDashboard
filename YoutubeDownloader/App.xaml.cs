using DIYoutubeDownloader.Internal;
using Syncfusion.SfSkinManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EventID = DIYoutubeDownloader.Internal.EventID.DIYoutubeDownloader;

namespace DIYoutubeDownloader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Overrides

        #region OnStartup

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.Initialize();
        }

        #endregion
        #region OnExit

        protected override void OnExit(ExitEventArgs e)
        {
            this.Close();
            base.OnExit(e);
        }

        #endregion

        #endregion
        #region Events

        #region UnhandledException_Raised

        private void UnhandledException_Raised(Exception exception, string source)
        {
            string message = null;
            try
            {
                message = $"Unhandled exception in {source}, exception: {exception?.ToString()}.";
                message += $"Exception in {Utils.GetAssemblyName()} v{Utils.GetAssemblyVersion()}";
            }
            catch (Exception ex)
            {
                Utils.Logger.Log(EventID.Application.UnhandledExceptionException, ex);
            }
            finally
            {
                Utils.Logger.Log(EventID.Application.UnhandledException, message);
            }
        }

        #endregion

        #endregion
        #region Methods

        #region Initialize
        private void Initialize()
        {
            try
            {
                Utils.Logger.Log(EventID.Application.Start);
                SfSkinManager.ApplyStylesOnApplication = true;
                #region GlobalUnhandledExceptionEvents

                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                    UnhandledException_Raised((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

                Application.Current.DispatcherUnhandledException += (s, e) =>
                    UnhandledException_Raised(e.Exception, "Application.Current.DispatcherUnhandledException");

                TaskScheduler.UnobservedTaskException += (s, e) =>
                    UnhandledException_Raised(e.Exception, "TaskScheduler.UnobservedTaskException");

                #endregion
            }
            catch (Exception ex)
            {
                Utils.Logger.Log(EventID.Application.Exception, ex);
            }
        }
        #endregion
        #region Close

        private void Close()
        {
            try
            {
                Utils.Logger.Log(EventID.Application.End);
                try { Utils.Logger.Close(); } catch (Exception ex) { Console.WriteLine(ex.ToString()); Utils.Logger.Log(EventID.Application.Exception, ex); }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion

        #endregion
    }
}
