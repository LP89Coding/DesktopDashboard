using System;
using System.Threading.Tasks;
using System.Windows;

using Syncfusion.SfSkinManager;

using DIYoutubeDownloader.Internal;
using DIYoutubeDownloader.ViewModels;
using EventID = DIYoutubeDownloader.Internal.EventID.DIYoutubeDowbloader;

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

        #region Form

        #region baFindMedia_Click
        private void baFindMedia_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.LoadMediaInfoAsync(tbeUrl.Text);
        }
        #endregion
        #region ddbaDownloadMedia_DropDownMenuItemClick_
        private void ddbaDownloadMedia_DropDownMenuItemClick_(object sender, RoutedEventArgs e)
        {
            try
            {
                Syncfusion.Windows.Tools.Controls.DropDownMenuItem ddmiItem = sender as Syncfusion.Windows.Tools.Controls.DropDownMenuItem;
                if (ddmiItem != null)
                {
                    MediaType ytmtItem = ddmiItem.Tag as MediaType;
                    this.viewModel.DownloadMediaAsync(ytmtItem);
                }
            }
            catch (Exception ex)
            {
                Utils.Logger.Log(EventID.DownloadMediaClick.Exception, ex);
                this.viewModel.DownloadMediaAsync(null);
            }
        }
        #endregion
        #region baDownloadMediaCancel_Click
        private void baDownloadMediaCancel_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel?.CancelDownload();
        }
        #endregion

        #endregion
        #region Application

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

        #endregion

        #region Methods

        #region InitializeControls
        private void InitializeControls()
        {
            try
            {
                Utils.Logger.Initialize();
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
