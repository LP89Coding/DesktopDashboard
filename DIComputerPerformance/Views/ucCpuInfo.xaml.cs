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

using DIComputerPerformance.ViewModels;
using DIComputerPerformance.Interfaces;

namespace DIComputerPerformance.Views
{
    /// <summary>
    /// Interaction logic for ucCpuInfo.xaml
    /// </summary>
    public partial class ucCpuInfo : UserControl, IDashboardControl
    {
        private IViewModel viewModel;

        public ucCpuInfo() : this(null)
        {
        }
        public ucCpuInfo(ArgumentCollection args)
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
                this.DataContext = this.viewModel = new ViewModelFactory().CreateViewModel<CpuInfoViewModel>(args);
            }
            catch (Exception ex)
            {
                //ToDo Log
            }
        }

        #endregion

        #endregion

        public void Refresh()
        {
            this.viewModel?.NotifyPropertyChange(ArgumentCollection.ArgumentType.ForceRefresh.ToString(), null);
        }
    }
}
