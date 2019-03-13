using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIComputerPerformance.Models
{
    public class PerformanceCounterCategory
    {
        public System.Diagnostics.PerformanceCounterCategory CounterCategory { get; private set; }

        public PerformanceCounterCategory(System.Diagnostics.PerformanceCounterCategory counterCategory)
        {
            this.CounterCategory = counterCategory;
        }

        public override string ToString()
        {
            if (this.CounterCategory != null)
                return this.CounterCategory.CategoryName;
            return "-";
        }
    }
}
