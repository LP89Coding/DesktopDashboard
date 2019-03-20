﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArgumentCollection = WPF.Common.Common.ArgumentCollection;
using PluginState = WPF.Common.Common.PluginState;


namespace WPF.Common.Interfaces
{
    public interface IPlugin
    {
        void InitializePlugin(ArgumentCollection args);

        ArgumentCollection GetArgs();

        string GetPluginName();
        System.Drawing.Icon GetPluginIcon();
        PluginState GetPluginCurrentState();

        bool IsPluginInitialized();

        string GetPluginAssemblyName();
        Version GetPluginAssemblyVersion();

        IWindowControl GetPluginControl();
        IWindow GetPluginWindow();

        void ClosePlugin();
    }
}
