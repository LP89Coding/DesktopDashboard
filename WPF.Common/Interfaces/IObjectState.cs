﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WpfCommonEnums = WPF.Common.Enums;

namespace WPF.Common.Interfaces
{
    public interface IObjectState
    {
        WpfCommonEnums.ObjectState State { get; set; }
    }
}
