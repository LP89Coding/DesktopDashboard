﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shell;

namespace WPF.Common.Interfaces
{
    public interface IWindowPropertyChangeNotifier
    {
        double TaskBarProgressValue { get; set; }
        TaskbarItemProgressState TaskBarProgressState { get; set; }

        double WindowHeight { get; set; }
        double WindowWidth { get; set; }

        string WindowTitle { get; set; }
    }
}
