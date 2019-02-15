using System;
using System.Threading.Tasks;
using System.Windows;

using Syncfusion.SfSkinManager;

using DIYoutubeDownloader.Internal;
using DIYoutubeDownloader.ViewModels;
using EventID = DIYoutubeDownloader.Internal.EventID.DIYoutubeDownloader;

namespace DIYoutubeDownloader
{
    /// <summary>
    /// Interaction logic for YoutubeDownloader.xaml
    /// </summary>
    public partial class YoutubeDownloader : Window
    {
        private YoutubeDownloaderViewModel viewModel { get; }

        #region Ctor
        public YoutubeDownloader()
        {
            InitializeComponent();

            this.viewModel = new YoutubeDownloaderViewModel();
            this.DataContext = viewModel;
            this.InitializeControls();
        }
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
        #region WDIYoutubeDonwloader_Closing

        private void WDIYoutubeDonwloader_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                this.viewModel?.Dispose();
                Utils.Logger.Log(EventID.Application.End);
                try { Utils.Logger.Close(); } catch (Exception ex) { Console.WriteLine(ex.ToString()); Utils.Logger.Log(EventID.Application.Exception, ex); }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion

        #endregion

        #region Methods

        #region InitializeControls
        private void InitializeControls()
        {
            try
            {
                Utils.Logger.Log(EventID.Application.Start);
                //TODO Create Own style
                SfSkinManager.ApplyStylesOnApplication = true;
                SfSkinManager.SetVisualStyle(this, VisualStyles.Office2013DarkGray);

                #region GlobalUnhandledExceptionEvents

                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                    UnhandledException_Raised((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

                Application.Current.DispatcherUnhandledException += (s, e) =>
                    UnhandledException_Raised(e.Exception, "Application.Current.DispatcherUnhandledException");

                TaskScheduler.UnobservedTaskException += (s, e) =>
                    UnhandledException_Raised(e.Exception, "TaskScheduler.UnobservedTaskException");

                #endregion
            }
            catch(Exception ex)
            {
                Utils.Logger.Log(EventID.Application.Exception, ex);
            }
        }
        #endregion

        #endregion
    }
}
