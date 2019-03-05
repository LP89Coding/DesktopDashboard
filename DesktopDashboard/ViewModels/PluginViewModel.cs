﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DesktopDashboard.Common;
using DesktopDashboard.Interfaces;
using DesktopDashboard.ViewModels;
using ArgumentCollection = DesktopDashboard.Common.ArgumentCollection;
using EventID = DesktopDashboard.Internals.EventID.DesktopDashboard;
using Utils = DesktopDashboard.Common.Utils;

namespace DesktopDashboard.ViewModels
{
    public class PluginViewModel : ObservableViewModel, IViewModel
    {
        private IPlugin plugin;
        public IPlugin Plugin
        {
            get
            {
                return this.plugin;
            }
            private set
            {
                this.plugin = value;
                RaisePropertyChangedEvent(nameof(this.Plugin));
            }
        }

        public string Name { get { return this.Plugin?.GetPluginName() ?? null; } }

        private System.Windows.Controls.Image icon;
        public System.Windows.Controls.Image Icon
        {
            get
            {
                if (this.icon == null)
                    this.icon = new System.Windows.Controls.Image() { Source = Utils.ToBitmapImage(this.Plugin?.GetPluginIcon()) };
                return icon;
            }
        }

        private Command initializePluginCommand;
        public Command InitializePluginCommand { get { return this.initializePluginCommand; } private set { this.initializePluginCommand = value; } }

        #region Overrides

        public override string ToString()
        {
            return this.Name;
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
        }

        #endregion
        #region IViewModel implementation

        public object GetPropertyValue(string propertyValue)
        {
            return null;
        }

        public bool NotifyPropertyChange(string propertyName, object propertyValue)
        {
            bool result = true;
            switch (propertyName)
            {
                default:
                    result = false;
                    break;
            }
            return result;
        }

        public void Initialize(ArgumentCollection args)
        {
            if (args != null)
            {
                if (args.Contains(ArgumentCollection.ArgumentType.Plugin))
                    this.Plugin = args.Get<IPlugin>(ArgumentCollection.ArgumentType.Plugin);
            }
            if (this.Plugin == null)
            {
                this.InitializePluginCommand = new Command((object parameter) => { });
            }
            else
            {
                this.InitializePluginCommand = new Command((object parameter) => { this.Plugin?.InitializePlugin(this.Plugin?.GetArgs()); });
            }
        }

        #endregion
    }
}