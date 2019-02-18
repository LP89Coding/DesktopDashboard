using System;
using System.Threading.Tasks;
using System.Windows;

using Syncfusion.SfSkinManager;

using DIYoutubeDownloader.Internal;
using DIYoutubeDownloader.ViewModels;
using EventID = DIYoutubeDownloader.Internal.EventID.DIYoutubeDownloader;
using System.ComponentModel;

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
            this.Initialize();
        }
        #endregion

        #region Overrides

        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                this.viewModel?.Dispose();
            }
            catch (Exception ex)
            {
                Utils.Logger.Log(EventID.Application.Exception, ex);
            }
            finally
            {
                base.OnClosing(e);
            }
        }

        #endregion
        #region Methods

        #region Initialize
        private void Initialize()
        {
            try
            {
                //TODO Create Own style
                SfSkinManager.SetVisualStyle(this, VisualStyles.Office2013DarkGray);
                this.DataContext = viewModel;
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
