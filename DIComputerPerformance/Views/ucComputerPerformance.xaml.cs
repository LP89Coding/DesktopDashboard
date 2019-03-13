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

using DIComputerPerformance.Internals;
using EventID = DIComputerPerformance.Internals.EventID.DIComputerPerformance;

namespace DIComputerPerformance.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ucComputerPerformance : UserControl, IWindowControl
    {
        private IViewModel viewModel;

        #region Ctor
        public ucComputerPerformance()
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
                this.DataContext = this.viewModel = new ComputerPerformanceViewModelFactory().CreateViewModel(args: null);
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
            this.viewModel.PropertyChanged += propertyChangedHandler;
        }
        public void UnsubscribePropertyChangeNotification(System.ComponentModel.PropertyChangedEventHandler propertyChangedHandler)
        {
            this.viewModel.PropertyChanged -= propertyChangedHandler;
        }

        #endregion

        #endregion
    }
}
