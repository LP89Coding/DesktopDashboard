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

using WPF.Common.Common;
using WPF.Common.Factories;
using WPF.Common.Interfaces;
using WPF.Common.Logger;

using DIComputerPerformance.Interfaces;
using DIComputerPerformance.Internals;
using DIComputerPerformance.ViewModels;

namespace DIComputerPerformance.Views
{
    /// <summary>
    /// Interaction logic for ucRamInfo.xaml
    /// </summary>
    public partial class ucRamInfo : UserControl, IDashboardControl
    {
        private IViewModel viewModel;

        public ucRamInfo() : this(null)
        {
        }
        public ucRamInfo(ArgumentCollection args)
        {
            Initialize(args);
            InitializeComponent();
        }

        #region Methods

        #region Initialize

        private void Initialize(ArgumentCollection args)
        {
            try
            {
                this.DataContext = this.viewModel = new ViewModelFactory().CreateViewModel<RamInfoViewModel>(args);
            }
            catch (Exception ex)
            {
                Logger.Log(EventID.DIComputerPerformance.Application.Exception, "InitializingRamInfo", ex);
            }
        }

        #endregion

        #endregion

        #region IDashboardControl

        public void Refresh()
        {
            this.viewModel?.NotifyPropertyChange(ArgumentCollection.ArgumentType.ForceRefresh.ToString(), null);
        }

        public void Dispose()
        {
            this.viewModel?.Dispose();
        }

        #endregion
    }
}
