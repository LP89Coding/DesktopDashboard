using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPF.Common.Common;
using WPF.Common.Factories;
using WPF.Common.Interfaces;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;

using DesktopDashboard.Internals;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;

namespace DesktopDashboard.ViewModels
{
    public class MainWindowViewModel : ObservableViewModel, IViewModel
    {
        private readonly PluginManager pluginManager;

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

        private ICommand closeWindowButtonCommand;
        public ICommand CloseWindowButtonCommand { get { return this.closeWindowButtonCommand; } set { this.closeWindowButtonCommand = value; } }
        private ICommand closeWindowOverrideButtonCommand;
        public ICommand CloseWindowOverrideButtonCommand { get { return this.closeWindowOverrideButtonCommand; } set { this.closeWindowOverrideButtonCommand = value; } }

        public MainWindowViewModel()
        {
            this.AvailablePlugins = new List<PluginViewModel>();
            this.pluginManager = new PluginManager();
        }

        #region Methods

        #region GetAvailablePlugins

        private List<PluginViewModel> GetAvailablePlugins()
        {
            IViewModelFactory factory = new ViewModelFactory();
            PluginState[] pluginStates = null;
            try
            {
                pluginStates = UserSettings.LoadSetting<PluginState[]>(UserSettings.SettingType.PluginState);
            }
            catch(Exception ex)
            {
                //ToDo Log
            }

            return pluginManager.GetPlugins()?.Select(p =>
            {
                ArgumentCollection viewModelArgs = new ArgumentCollection();
                ArgumentCollection pluginInitArgs = new ArgumentCollection();

                pluginInitArgs.Set(ArgumentCollection.ArgumentType.IsPluginMode, true);

                if (pluginStates != null)
                {
                    PluginState pluginState = pluginStates.FirstOrDefault(ps => String.Equals(ps.Name, p.GetPluginName()));
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

        #endregion
        #region CloseWindowOverride

        private void CloseWindowOverride(object parameter)
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
                        //ToDo Log
                    }
                }
            }
            this.CloseWindowButtonCommand?.Execute(parameter);
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
                if(args.Contains(ArgumentCollection.ArgumentType.WindowCloseCommand))
                    this.CloseWindowButtonCommand = args.Get<Command>(ArgumentCollection.ArgumentType.WindowCloseCommand);
            }
            this.CloseWindowOverrideButtonCommand = new Command((object parameter) => { this.CloseWindowOverride(parameter); });
            this.AvailablePlugins = this.GetAvailablePlugins();
        }

        #endregion
    }
}
