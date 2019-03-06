using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Logger = WPF.Common.Logger.Logger;

using DesktopDashboard.Interfaces;
using DesktopDashboard.ViewModels;
using BaseWindow = DesktopDashboard.Views.BaseWindow;
using ArgumentCollection = DesktopDashboard.Common.ArgumentCollection;

using DIYoutubeDownloader.Internal;
using EventID = DIYoutubeDownloader.Internal.EventID.DIYoutubeDownloader;
using IntrnalUtils = DIYoutubeDownloader.Internal.Utils;

namespace DIYoutubeDownloader.Common
{
    public class Plugin : IPlugin
    {
        private ArgumentCollection args { get; set; }
        BaseWindow mainWindow { get; set; }
        #region Events

        #region UnhandledException_Raised

        private void UnhandledException_Raised(Exception exception, string source)
        {
            string message = null;
            try
            {
                message = $"Unhandled exception in {source}, exception: {exception?.ToString()}.";
                message += $"Exception in {IntrnalUtils.GetAssemblyName()} v{IntrnalUtils.GetAssemblyVersion()}";
            }
            catch (Exception ex)
            {
                Logger.Log(EventID.Application.UnhandledExceptionException, ex);
            }
            finally
            {
                Logger.Log(EventID.Application.UnhandledException, message);
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
                Logger.Log(EventID.Application.Start);
                #region GlobalUnhandledExceptionEvents

                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                    UnhandledException_Raised((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

                System.Windows.Application.Current.DispatcherUnhandledException += (s, e) =>
                    UnhandledException_Raised(e.Exception, "Application.Current.DispatcherUnhandledException");

                TaskScheduler.UnobservedTaskException += (s, e) =>
                    UnhandledException_Raised(e.Exception, "TaskScheduler.UnobservedTaskException");

                #endregion
            }
            catch (Exception ex)
            {
                Logger.Log(EventID.Application.Exception, ex);
            }
        }
        #endregion
        #region InitializeWindow
        private void InitializeWindow()
        {
            ArgumentCollection args = new ArgumentCollection();
            args.Set(ArgumentCollection.ArgumentType.WindowIcon, ResourceImage.YouTubeIcon);
            args.Set(ArgumentCollection.ArgumentType.WindowTitle, Consts.YoutubeDownloaderTitle);
            args.Set(ArgumentCollection.ArgumentType.WindowCloseCommand, new Command((object parametrer) => { this.ClosePlugin(); }));

            mainWindow = new BaseWindow(args);
            ucYoutubeDownloader youtubeDownloader = new ucYoutubeDownloader();
            mainWindow.SetContent(youtubeDownloader);
            mainWindow.Show();
        }
        #endregion
        #region Close

        private void Close()
        {
            try
            {
                Logger.Log(EventID.Application.End);
                try { Logger.Close(); } catch (Exception ex) { Console.WriteLine(ex.ToString()); Logger.Log(EventID.Application.Exception, ex); }
                mainWindow?.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion

        #endregion
        #region IPlugin implementation

        public void InitializePlugin(ArgumentCollection args)
        {
            this.args = args ?? new ArgumentCollection();
            this.Initialize();
            this.InitializeWindow();
        }

        public ArgumentCollection GetArgs()
        {
            return this.args;
        }

        public string GetPluginName()
        {
            return Consts.YoutubeDownloaderTitle;
        }

        public System.Drawing.Icon GetPluginIcon()
        {
            return ResourceImage.YouTubeIcon;
        }

        public void ClosePlugin()
        {
            this.Close();
        }

        #endregion
    }
}
