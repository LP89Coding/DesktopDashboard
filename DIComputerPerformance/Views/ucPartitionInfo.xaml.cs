﻿using System;
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

using DIComputerPerformance.ViewModels;
using DIComputerPerformance.Interfaces;
using DIComputerPerformance.Internals;

namespace DIComputerPerformance.Views
{
    /// <summary>
    /// Interaction logic for PartitionInfo.xaml
    /// </summary>
    public partial class ucPartitionInfo : UserControl, IDashboardControl
    {
        private IViewModel viewModel { get; set;}

        public ucPartitionInfo() : this(null)
        {
        }
        public ucPartitionInfo(ArgumentCollection args)
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
                this.DataContext = this.viewModel = new ViewModelFactory().CreateViewModel<PartitionInfoViewModel>(args);
            }
            catch (Exception ex)
            {
                Logger.Log(EventID.DIComputerPerformance.Application.Exception, "InitializingPartitionInfo", ex);
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
