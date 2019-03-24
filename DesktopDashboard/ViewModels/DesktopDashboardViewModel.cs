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

namespace DesktopDashboard.ViewModels
{
    public class DesktopDashboardViewModel : ObservableViewModel, IViewModel
    {
        private readonly PluginManager pluginManager;

        private double width;
        public double Width
        {
            get { return this.width; }
            set
            {
                this.width = value;
                RaisePropertyChangedEvent(nameof(this.Width));
            }
        }
        private double height;
        public double Height
        {
            get { return this.height; }
            set
            {
                this.height = value;
                RaisePropertyChangedEvent(nameof(this.Height));
            }
        }
        private double top;
        public double Top
        {
            get { return this.top; }
            set
            {
                this.top = value;
                RaisePropertyChangedEvent(nameof(this.Top));
            }
        }
        private double left;
        public double Left
        {
            get { return this.left; }
            set
            {
                this.left = value;
                RaisePropertyChangedEvent(nameof(this.Left));
            }
        }
        private bool topMost;
        public bool TopMost
        {
            get { return this.topMost; }
            set
            {
                bool valueChanged = this.topMost != value;
                this.topMost = value;
                RaisePropertyChangedEvent(nameof(this.TopMost));
            }
        }
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

        private ICommand closeWindowButtonCommand;
        public ICommand CloseWindowButtonCommand { get { return this.closeWindowButtonCommand; } set { this.closeWindowButtonCommand = value; } }
        private ICommand topMostButtonCommand;
        public ICommand TopMostButtonCommand { get { return this.topMostButtonCommand; } set { this.topMostButtonCommand = value; } }
        private ICommand closeWindowOverrideButtonCommand;
        public ICommand CloseWindowOverrideButtonCommand { get { return this.closeWindowOverrideButtonCommand; } set { this.closeWindowOverrideButtonCommand = value; } }

        public DesktopDashboardViewModel()
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
                viewModelArgs.Set(ArgumentCollection.ArgumentType.ParentWidth, this.Width);

                PluginViewModel pluginViewModel = factory.CreateViewModel<PluginViewModel>(viewModelArgs);
                return pluginViewModel;
            }).ToList() ?? new List<PluginViewModel>();
        }

        #endregion
        #region CloseWindowOverride

        private void CloseWindowOverride(object parameter)
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
                            //ToDo Log
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ToDo Log
            }
            Internals.WindowState windowState = new Internals.WindowState()
            {
                Width = this.Width,
                Height = this.Height,
                Top = this.Top,
                Left = this.Left,
                TopMost = this.TopMost
            };
            UserSettings.SaveSetting(UserSettings.SettingType.WindowState, windowState);
            this.CloseWindowButtonCommand?.Execute(parameter);
        }

        #endregion
        #region CloseWindowOverride

        private void TopMostToogle(object parameter)
        {
            this.TopMost = !this.TopMost;
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
            Internals.WindowState windowState = UserSettings.LoadSetting<Internals.WindowState>(UserSettings.SettingType.WindowState);

            this.Height = windowState?.Height ?? 200;
            this.Width = windowState?.Width ?? SystemParameters.PrimaryScreenWidth * 0.1;
            this.Top = windowState?.Top ?? 0;
            this.Left = windowState?.Left ?? SystemParameters.PrimaryScreenWidth - this.Width;
            this.TopMost = windowState?.TopMost ?? false;

            this.Title = Consts.WindowTitle;
            this.CloseWindowOverrideButtonCommand = new Command((object parameter) => { this.CloseWindowOverride(parameter); });
            this.TopMostButtonCommand = new Command((object parameter) => { this.TopMostToogle(parameter); });
            this.AvailablePlugins = this.GetAvailablePlugins();

            //Syncfusion.Windows.Controls.Notification.SfHubTile htItem = args.Get<Syncfusion.Windows.Controls.Notification.SfHubTile>(ArgumentCollection.ArgumentType.DockingManager);
            
            //htItem.Title = this.AvailablePlugins[1].Plugin.GetPluginName();
            //htItem.ImageSource = WPFUtils.ToBitmapImage(this.AvailablePlugins[1].Plugin.GetPluginIcon());
            //htItem.SecondaryContent = new System.Windows.Controls.Image() { Source = WPFUtils.ToBitmapImage(Resources.ResourceImage256.YouTube), Stretch = Stretch.Fill };
            //htItem.Width = this.Width / 2;
            //htItem.Height = this.Width / 2;

            //<syncfusion:SfHubTile x:Name="htPluginTile" HorizontalAlignment="Left"  Grid.Row="1"  VerticalAlignment="Top"  Interval="0:0:3">

            //    <syncfusion:SfHubTile.HubTileTransitions>
            //        <Controls:FadeTransition/>
            //    </syncfusion:SfHubTile.HubTileTransitions>

            //</syncfusion:SfHubTile>
        }

        #endregion
    }
}
