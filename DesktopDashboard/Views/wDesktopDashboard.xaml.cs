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
using System.Diagnostics;
using System.Threading;

using WPF.Common.Factories;
using WPF.Common.Interfaces;
using Logger = WPF.Common.Logger.Logger;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;
using WindowState = WPF.Common.Common.WindowState;
using WPFUtils = WPF.Common.Common.Utils;

using DesktopDashboard.Resources;
using DesktopDashboard.Internals;
using DesktopDashboard.Common;
using DesktopDashboard.ViewModels;
using EventID = DesktopDashboard.Internals.EventID.DesktopDashboard;
using WPF.Common.Common;
using System.ComponentModel;

namespace DesktopDashboard
{
    public partial class wDesktopDashboard : UserControl, IWindowControl
    {
        private DesktopDashboardViewModel viewModel;

        #region Ctor

        public wDesktopDashboard()
        {
            try
            {
                InitializeComponent();
                Initialize(null);
                Logger.Log(EventID.Application.Start);

                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-us");
                
            }
            catch(Exception ex)
            {
                //ToDo Log
            }
        }

        #endregion

        #region Methods

        #region Initialize
        private void Initialize(ArgumentCollection args)
        {
            try
            {
                if (args == null)
                    args = new ArgumentCollection();

                ViewModelFactory factory = new ViewModelFactory();
                this.viewModel = factory.CreateViewModel<DesktopDashboardViewModel>(args);
                this.DataContext = viewModel;
            }
            catch (Exception ex)
            {
                Logger.Log(EventID.Application.Exception, ex);
            }
        }
        #endregion
        #region GetAvailablePluginsCount

        public int GetAvailablePluginsCount()
        {
            return this.viewModel?.AvailablePlugins?.Count ?? 0;
        }

        #endregion
        #region Close

        public void Close()
        {
            try
            {
                this.viewModel?.CloseAvailablePlugins();
            }
            catch (Exception ex)
            {
                //ToDo Log
            }
        }

        #endregion

        #endregion


        #region IWindowControl implementation

        public void SubscribePropertyChangeNotification(PropertyChangedEventHandler propertyChangedHandler)
        {
            if (this.viewModel != null)
                this.viewModel.PropertyChanged += propertyChangedHandler;
        }

        public void UnsubscribePropertyChangeNotification(PropertyChangedEventHandler propertyChangedHandler)
        {
            if (this.viewModel != null)
                this.viewModel.PropertyChanged -= propertyChangedHandler;
        }

        public void Dispose()
        {
            try
            {
                this.viewModel?.Dispose();
            }
            catch(Exception ex)
            {
                //ToDo Log
            }
        }

        #endregion

    }
}
