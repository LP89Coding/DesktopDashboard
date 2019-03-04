using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;

using DesktopDashboard.Common;
using DesktopDashboard.Interfaces;
using DesktopDashboard.ViewModels;
using ArgumentCollection = DesktopDashboard.Common.ArgumentCollection;
using EventID = DesktopDashboard.Internals.EventID.DesktopDashboard;
using Utils = DesktopDashboard.Common.Utils;

namespace DesktopDashboard.Views
{
    /// <summary>
    /// Interaction logic for BaseWindow.xaml
    /// </summary>
    public partial class BaseWindow : Window
    {
        private enum PropertyChangeNotificationOperation
        {
            Subscribe,
            Unsubscribe
        }

        private IViewModel viewModel { get; set; }
        private PropertyChangedEventHandler propertyChangedEventHandler;

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

        private void RNavBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try { if(e.ButtonState == MouseButtonState.Pressed) this.DragMove();} catch { }
        }

        private void TbeTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try { if (e.ButtonState == MouseButtonState.Pressed) this.DragMove(); } catch { }
        }

        private void IWindowIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try { if (e.ButtonState == MouseButtonState.Pressed) this.DragMove(); } catch { }
        }

        #endregion  
        #region Methods

        #region Initialize
        private void Initialize(ArgumentCollection args)
        {
            try
            {
                this.propertyChangedEventHandler = new PropertyChangedEventHandler(this.ChangeListener);

                ViewModelFactory factory = new ViewModelFactory();
                this.viewModel = factory.CreateViewModel<BaseWindowViewModel>(args);
                this.DataContext = viewModel;
                
                this.ManagePropertyChangeNotificationSubscription(this.Content as Visual, PropertyChangeNotificationOperation.Subscribe);
            }
            catch (Exception ex)
            {
                Utils.Logger.Log(EventID.Application.Exception, ex);
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
        #region ManagePropertyChangeNotificationSubscription
        private void ManagePropertyChangeNotificationSubscription(Visual parent, PropertyChangeNotificationOperation operation)
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    Visual childVisual = (Visual)VisualTreeHelper.GetChild(parent, i);
                    if (childVisual is IWindowControl)
                    {
                        if(operation == PropertyChangeNotificationOperation.Subscribe)
                            (childVisual as IWindowControl).SubscribePropertyChangeNotification(this.propertyChangedEventHandler);
                        else
                            (childVisual as IWindowControl).UnsubscribePropertyChangeNotification(this.propertyChangedEventHandler);
                    }

                    ManagePropertyChangeNotificationSubscription(childVisual, operation);
                }
            }
        }
        #endregion
        #region SetContent

        public void SetContent(IWindowControl windowControl)
        {
            if (!(windowControl is UIElement))
                throw new ArgumentException("WindowControl has to be UIElement");
            this.ManagePropertyChangeNotificationSubscription(wpFillContent, PropertyChangeNotificationOperation.Unsubscribe);
            wpFillContent.Children.Clear();
            wpFillContent.Children.Add(windowControl as UIElement);
            this.ManagePropertyChangeNotificationSubscription(wpFillContent, PropertyChangeNotificationOperation.Subscribe);
        }

        #endregion

        #endregion

    }
}
