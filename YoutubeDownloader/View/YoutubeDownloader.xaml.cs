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
        private IViewModel viewModel { get; set; }

        #region Ctor
        public YoutubeDownloader()
        {
            this.Initialize();
            InitializeComponent();
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

                ViewModelFactory factory = new ViewModelFactory();
                this.viewModel = factory.CreateViewModel<YoutubeDownloaderViewModel>(new Downloader());
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
