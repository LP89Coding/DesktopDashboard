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

using DIComputerPerformance.Internals;

namespace DIComputerPerformance.ViewModels
{
    public class RamInfoViewModel : ObservableViewModel, IViewModel
    {

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
        private string gaugeHeader;
        public string GaugeHeader
        {
            get { return this.gaugeHeader; }
            private set
            {
                this.gaugeHeader = value;
                RaisePropertyChangedEvent(nameof(this.GaugeHeader));
            }
        }

        #region Methods

        #region RefreshCpuInfo
        public void RefreshRamInfo()
        {
            try
            {
                long valRamTotalMemory = PerformanceInfo.GetTotalMemoryInMiB();
                long valRamTotal = PerformanceInfo.GetTotalMemoryInMiB() - PerformanceInfo.GetPhysicalAvailableMemoryInMiB();
                int valRamTakenPrc = Convert.ToInt32(((double)valRamTotal / (double)valRamTotalMemory) * 100.0);

                this.GaugeValue = valRamTakenPrc;
                this.GaugeHeader = String.Format("RAM %{0}({1:0.00}{2})", Environment.NewLine,
                                        valRamTotal > 1024 ? Math.Round(valRamTotal / 1024.0, 2) : valRamTotal,
                                        valRamTotal > 1024 ? "GB" : "MB");
            }
            catch (Exception ex)
            {
                Logger.Log(EventID.DIComputerPerformance.Application.Exception, "RefreshRamInfo", ex);
            }
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
        }

        public bool NotifyPropertyChange(string propertyName, object propertyValue)
        {
            bool result = false;
            switch (propertyName)
            {
                case nameof(ArgumentCollection.ArgumentType.ForceRefresh):
                    this.RefreshRamInfo();
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
