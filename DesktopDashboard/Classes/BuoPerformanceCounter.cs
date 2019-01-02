using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DesktopDashboard
{
    public class BuoPerformanceCounter
    {
        public PerformanceCounter PerformanceCounter { get; private set; }

        public BuoPerformanceCounter(PerformanceCounter PerformanceCounter)
        {
            this.PerformanceCounter = PerformanceCounter;
        }

        public override string ToString()
        {
            if (this.PerformanceCounter != null)
            {
                if(!String.IsNullOrEmpty(this.PerformanceCounter.InstanceName))
                    return String.Format("{0} ({1})", this.PerformanceCounter.CounterName, this.PerformanceCounter.InstanceName);
                return this.PerformanceCounter.CounterName;
            }
            return "-";
        }
    }
}
