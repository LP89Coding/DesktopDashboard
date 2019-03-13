using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPF.Common.Common;
using WPF.Common.Interfaces;
using Logger = WPF.Common.Logger.Logger;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;
using WPFUtils = WPF.Common.Common.Utils;

namespace DIComputerPerformance.ViewModels
{
    public class CpuInfoViewModel : ObservableViewModel, IViewModel
    {
        private System.Diagnostics.PerformanceCounter cpuTotalCntr = null;

        private int gaugeValue;
        public int GaugeValue
        {
            get { return this.gaugeValue; }
            private set
            {
                this.gaugeValue = value;
                RaisePropertyChangedEvent(nameof(this.GaugeValue));
            }
        }

        #region Methods
        
        #region RefreshCpuInfo
        public void RefreshCpuInfo()
        {
            if(cpuTotalCntr != null)
                this.GaugeValue = Convert.ToInt32(Math.Round(cpuTotalCntr.NextValue(), 0));
        }
        #endregion

        #endregion

        #region IViewModel implementation

        public void Dispose()
        {
        }

        public object GetPropertyValue(string propertyName)
        {
            throw new NotImplementedException();
        }

        public void Initialize(ArgumentCollection args)
        {
            if (System.Diagnostics.PerformanceCounterCategory.Exists("Processor"))
                this.cpuTotalCntr = new System.Diagnostics.PerformanceCounter("Processor", "% Processor Time", "_Total", true);
            else
                this.cpuTotalCntr = new System.Diagnostics.PerformanceCounter("Processor Information", "% Processor Time", "_Total", true);
        }

        public bool NotifyPropertyChange(string propertyName, object propertyValue)
        {
            bool result = false;
            switch (propertyName)
            {
                case nameof(ArgumentCollection.ArgumentType.ForceRefresh):
                    this.RefreshCpuInfo();
                    result = true;
                    break;
                default:
                    break;
            }
            return result;
        }

        #endregion
    }
}
