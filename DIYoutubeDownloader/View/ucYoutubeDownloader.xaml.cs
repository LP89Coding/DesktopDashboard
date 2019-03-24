using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPF.Common.Interfaces;
using Logger = WPF.Common.Logger.Logger;

using EventID = DIYoutubeDownloader.Internal.EventID.DIYoutubeDownloader;

namespace DIYoutubeDownloader
{
    public partial class ucYoutubeDownloader : UserControl, IWindowControl
    {
        private IViewModel viewModel { get; set; }

        #region Ctor
        public ucYoutubeDownloader()
        {
            this.Initialize();
            InitializeComponent();
        }
        #endregion
        
        #region Methods

        #region Initialize
        private void Initialize()
        {
            try
            {
                this.DataContext = this.viewModel = Common.Utils.CreateYoutubeDownloaderViewModel();
            }
            catch (Exception ex)
            {
                Logger.Log(EventID.Application.Exception, ex);
            }
        }
        #endregion

        #endregion

        #region IWindowControl implementation

        #region SubscribePropertyChangeNotification

        public void SubscribePropertyChangeNotification(System.ComponentModel.PropertyChangedEventHandler propertyChangedHandler)
        {
            if(this.viewModel != null)
                this.viewModel.PropertyChanged += propertyChangedHandler;
        }

        #endregion
        #region UnsubscribePropertyChangeNotification

        public void UnsubscribePropertyChangeNotification(System.ComponentModel.PropertyChangedEventHandler propertyChangedHandler)
        {
            if (this.viewModel != null)
                this.viewModel.PropertyChanged -= propertyChangedHandler;
        }

        #endregion

        #endregion
        #region IDisposable implementation

        public void Dispose()
        {
            try
            {
                this.viewModel?.Dispose();
            }
            catch (Exception ex)
            {
                //ToDo Log
            }
        }

        #endregion
    }
}
