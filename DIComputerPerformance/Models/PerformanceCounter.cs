using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DIComputerPerformance.Models
{
    public class PerformanceCounter
    {
        public System.Diagnostics.PerformanceCounter Counter { get; private set; }

        public PerformanceCounter(System.Diagnostics.PerformanceCounter counter)
        {
            this.Counter = counter;
        }

        public override string ToString()
        {
            if (this.Counter != null)
            {
                if(!String.IsNullOrEmpty(this.Counter.InstanceName))
                    return String.Format("{0} ({1})", this.Counter.CounterName, this.Counter.InstanceName);
                return this.Counter.CounterName;
            }
            return "-";
        }
    }
}
