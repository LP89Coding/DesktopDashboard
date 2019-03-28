using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPF.Common.Common;
using WPF.Common.Factories;
using WPF.Common.Interfaces;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;
using WPFUtils = WPF.Common.Common.Utils;

using DesktopDashboard.Internals;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WPF.Common.Logger;

namespace DesktopDashboard.ViewModels
{
    public class DesktopDashboardViewModel : ObservableViewModel, IViewModel
    {
        private readonly PluginManager pluginManager;
        
        private string title;
        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                RaisePropertyChangedEvent(nameof(this.Title));
            }
        }

        private List<PluginViewModel> availablePlugins;
        public List<PluginViewModel> AvailablePlugins
        {
            get
            {
                return this.availablePlugins;
            }
            set
            {
                this.availablePlugins = value;
                RaisePropertyChangedEvent(nameof(this.AvailablePlugins));
            }
        }
        
        public DesktopDashboardViewModel()
        {
            this.AvailablePlugins = new List<PluginViewModel>();
            this.pluginManager = new PluginManager();
        }

        #region Methods

        #region GetAvailablePlugins

        private List<PluginViewModel> GetAvailablePlugins()
        {
            List<PluginViewModel> result = new List<PluginViewModel>();
            try
            {
                IViewModelFactory factory = new ViewModelFactory();
                PluginState[] pluginStates = null;

                pluginStates = UserSettings.LoadSetting<PluginState[]>(UserSettings.SettingType.PluginState);

                result = pluginManager.GetPlugins()?.Select(p =>
                {
                    ArgumentCollection viewModelArgs = new ArgumentCollection();
                    ArgumentCollection pluginInitArgs = new ArgumentCollection();

                    pluginInitArgs.Set(ArgumentCollection.ArgumentType.IsPluginMode, true);

                    if (pluginStates != null)
                    {
                        PluginState pluginState = pluginStates.FirstOrDefault(ps => String.Equals(ps.Name, p.GetPluginName()));
                        if (pluginState == null)
                            pluginState = new PluginState(p.GetPluginName(), false) { WindowState = p.GetDefaultWindowState() };
                        if (pluginState.WindowState == null)
                            pluginState.WindowState = p.GetDefaultWindowState();
                        if (pluginState != null)
                        {
                            pluginInitArgs.Set(ArgumentCollection.ArgumentType.PluginState, pluginState);
                            viewModelArgs.Set(ArgumentCollection.ArgumentType.RestorePlugin, pluginState.IsActive);
                        }
                    }

                    viewModelArgs.Set(ArgumentCollection.ArgumentType.Plugin, p);
                    viewModelArgs.Set(ArgumentCollection.ArgumentType.PluginArgs, pluginInitArgs);

                    PluginViewModel pluginViewModel = factory.CreateViewModel<PluginViewModel>(viewModelArgs);
                    return pluginViewModel;
                }).ToList() ?? new List<PluginViewModel>();
            }
            catch(Exception ex)
            {
                Logger.Log(EventID.DesktopDashboard.Application.Exception, nameof(GetAvailablePlugins), ex);
            }
            finally
            {
                Logger.Log(EventID.DesktopDashboard.Application.FoundedAvailablePlugins, result.Count, String.Join(",", result.Select(p => p.Name)));
            }
            return result;
        }

        #endregion
        #region CloseAvailablePlugins

        public void CloseAvailablePlugins()
        {
            try
            {
                PluginState[] currentState = this.AvailablePlugins?.Select(d => d.Plugin).Select(p => p.GetPluginCurrentState()).ToArray() ?? new PluginState[] { };
                UserSettings.SaveSetting(UserSettings.SettingType.PluginState, currentState);
                if (this.AvailablePlugins.Count > 0)
                {
                    foreach (IPlugin plugin in this.AvailablePlugins.Select(p => p.Plugin))
                    {
                        try
                        {
                            plugin?.ClosePlugin();
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(EventID.DesktopDashboard.Application.Exception, $"Plugin {plugin.GetPluginName()} closing", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(EventID.DesktopDashboard.Application.Exception, nameof(CloseAvailablePlugins), ex);
            }
        }

        #endregion
        
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
            }
            this.AvailablePlugins = this.GetAvailablePlugins();
            this.Title = Consts.WindowTitle;
        }

        #endregion
    }
}
