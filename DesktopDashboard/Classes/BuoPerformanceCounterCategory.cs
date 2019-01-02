using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDashboard
{
    public class BuoPerformanceCounterCategory
    {
        public PerformanceCounterCategory PerformanceCounterCategory { get; private set; }

        public BuoPerformanceCounterCategory(PerformanceCounterCategory PerformanceCounterCategory)
        {
            this.PerformanceCounterCategory = PerformanceCounterCategory;
        }

        public override string ToString()
        {
            if (this.PerformanceCounterCategory != null)
                return this.PerformanceCounterCategory.CategoryName;
            return "-";
        }
    }
}
