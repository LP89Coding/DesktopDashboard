using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using DIYoutubeDownloader.Internal;
using DIYoutubeDownloader.ViewModels;
using IntrnalUtils = DIYoutubeDownloader.Internal.Utils;

using DesktopDashboard.Common;
using DesktopDashboard.Interfaces;
using ArgumentCollection = DesktopDashboard.Common.ArgumentCollection;
using System.ComponentModel;

namespace DIYoutubeDownloader
{
    /// <summary>
    /// Interaction logic for BaseWindow.xaml
    /// </summary>
    public partial class BaseWindow : Window
    {
        private IViewModel viewModel { get; set; }

        #region Ctor

        public BaseWindow() : this(null)
        {
        }

        public BaseWindow(ArgumentCollection args)
        {
            InitializeComponent();
            this.Initialize(args);
        }

        #endregion

        #region Overrides

        #region OnClosing

        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                this.viewModel?.Dispose();
            }
            catch(Exception ex)
            {
                //TODO Logs
            }
            base.OnClosing(e);
        }

        #endregion

        #endregion
        #region Events

        private void RNavBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void TbeTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void IWindowIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        #endregion  
        #region Methods

        #region Initialize
        private void Initialize(ArgumentCollection args)
        {
            try
            {
                ViewModelFactory factory = new ViewModelFactory();
                this.viewModel = factory.CreateViewModel<BaseWindowViewModel>(args);
                this.DataContext = viewModel;
                
                this.SubscribePropertyChangeNotification(this.Content as Visual);
            }
            catch (Exception ex)
            {
                IntrnalUtils.Logger.Log(EventID.DIYoutubeDownloader.Application.Exception, ex);
            }
        }
        #endregion
        #region ChangeListener
        private void ChangeListener(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e != null && sender is IViewModel)
            {
                IViewModel childViewModel = sender as IViewModel;
                switch (e.PropertyName)
                {
                    case nameof(IWindowPropertyChangeNotifier.TaskBarProgressState):
                    case nameof(IWindowPropertyChangeNotifier.TaskBarProgressValue):
                        viewModel.NotifyPropertyChange(e.PropertyName, childViewModel.GetPropertyValue(e.PropertyName));
                        break;
                    default: break;
                }
            }
        }
        #endregion
        #region SubscribePropertyChangeNotification
        private void SubscribePropertyChangeNotification(Visual parent)
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    Visual childVisual = (Visual)VisualTreeHelper.GetChild(parent, i);
                    if (childVisual is IWindowControl)
                        (childVisual as IWindowControl).SubscribePropertyChangeNotification(new System.ComponentModel.PropertyChangedEventHandler(this.ChangeListener));

                    SubscribePropertyChangeNotification(childVisual);
                }
            }
        }
        #endregion

        #endregion

    }
}
