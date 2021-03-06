﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArgumentCollection = WPF.Common.Common.ArgumentCollection;

namespace WPF.Common.Interfaces
{
    public delegate void ChangeListenerDelegate(object sender, System.ComponentModel.PropertyChangedEventArgs e);
    public interface IViewModel : IDisposable
    {
        event PropertyChangedEventHandler PropertyChanged;

        void Initialize(ArgumentCollection args);
        object GetPropertyValue(string propertyName);
        bool NotifyPropertyChange(string propertyName, object propertyValue);
    }
}
