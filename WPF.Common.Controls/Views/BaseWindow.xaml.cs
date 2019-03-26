using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;

using WPF.Common.Factories;
using WPF.Common.Interfaces;
using Logger = WPF.Common.Logger.Logger;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;
using WindowState = WPF.Common.Common.WindowState;

using WPF.Common.Controls.ViewModels;

//using EventID = DesktopDashboard.Internals.EventID.DesktopDashboard;

namespace WPF.Common.Controls.Views
{
    /// <summary>
    /// Interaction logic for BaseWindow.xaml
    /// </summary>
    public partial class BaseWindow : Window, IWindow
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
                //TODO Log
                //Logger.Log(EventID.Application.Exception, ex);
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
                    case nameof(IWindowPropertyChangeNotifier.WindowWidth):
                    case nameof(IWindowPropertyChangeNotifier.WindowHeight):
                    case nameof(IWindowPropertyChangeNotifier.WindowTitle):
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
        #region ClearContent

        private void ClearContent()
        {
            if(wpFillContent.Children?.Count > 0)
            {
                this.ManagePropertyChangeNotificationSubscription(wpFillContent, PropertyChangeNotificationOperation.Unsubscribe);
                this.GetContent()?.Dispose();
                wpFillContent.Children.Clear();
            }
        }

        #endregion
        #region GetContent

        public IWindowControl GetContent()
        {
            if (wpFillContent?.Children?.Count > 0)
                return wpFillContent.Children[0] as IWindowControl;
            return null;
        }

        #endregion

        #endregion

        #region IWindow implementation

        #region Close

        public new void Close()
        {
            try
            {
                this.viewModel?.Dispose();
            }
            catch (Exception ex)
            {
                //ToDo Log
            }
            try
            {
                base.Close();
            }
            catch(Exception ex)
            {
                //ToDo Log
            }
            try
            {
                this.ClearContent();
            }
            catch(Exception ex)
            {
                //ToDo Log
            }
        }

        #endregion
        #region Show

        public new void Show()
        {
            if(this.Visibility != Visibility.Visible)
                base.Show();
            if (base.WindowState == System.Windows.WindowState.Minimized)
            {
                base.WindowState = System.Windows.WindowState.Normal;
            }

            base.Activate();
            //Window.Topmost = true;  // important
            //Window.Topmost = false; // important
            base.Focus();         // important
        }

        #endregion

        #region GetWindowState

        public WindowState GetWindowState()
        {
            WindowState currentState = new WindowState();

            currentState.PositionLeft = this.Left;
            currentState.PositionTop = this.Top;

            currentState.Height = this.Height;
            currentState.Width = this.Width;

            currentState.State = this.WindowState;

            currentState.TopMost = this.Topmost;

            return currentState;
        }

        #endregion
        #region SetContent

        public void SetContent(IWindowControl windowControl)
        {
            if (!(windowControl is UIElement))
                throw new ArgumentException("WindowControl has to be UIElement");
            ClearContent();
            wpFillContent.Children.Add(windowControl as UIElement);
            this.ManagePropertyChangeNotificationSubscription(wpFillContent, PropertyChangeNotificationOperation.Subscribe);
        }

        #endregion

        #endregion

    }
}
