﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIComputerPerformance.Interfaces
{
    public interface IDashboardControl : IDisposable
    {
        void Refresh();
    }
}
