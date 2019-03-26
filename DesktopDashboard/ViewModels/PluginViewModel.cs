using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using WPF.Common.Common;
using WPF.Common.Interfaces;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;

using WPFUtils = WPF.Common.Common.Utils;

using DesktopDashboard.Internals;

namespace DesktopDashboard.ViewModels
{
    public class PluginViewModel : ObservableViewModel, IViewModel
    {
        private ArgumentCollection pluginInitArgs { get; set; }

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
        public double Size { get; private set; }

        private System.Windows.Controls.Image icon;
        public System.Windows.Controls.Image Icon
        {
            get
            {
                if (this.icon == null)
                    this.icon = new System.Windows.Controls.Image() { Source = WPFUtils.ToBitmapImage(this.Plugin?.GetPluginIcon()) };
                return icon;
            }
        }
        public ImageSource IconSource
        {
            get
            {
                return Icon.Source;
            }
        }


        private ImageSource largeImage;
        public ImageSource LargeImage
        {
            get
            {
                if (this.largeImage == null)
                    this.largeImage = WPFUtils.ToBitmapImage(this.Plugin?.GetLargeImage());
                return largeImage;
            }
        }

        private Command initializePluginCommand;
        public Command InitializePluginCommand { get { return this.initializePluginCommand; } private set { this.initializePluginCommand = value; } }


        #region Ctor

        public PluginViewModel()
        {
            this.pluginInitArgs = new ArgumentCollection();
        }

        #endregion

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
            bool restorePlugin = false;
            if (args != null)
            {
                if (args.Contains(ArgumentCollection.ArgumentType.Plugin))
                    this.Plugin = args.Get<IPlugin>(ArgumentCollection.ArgumentType.Plugin);
                if (args.Contains(ArgumentCollection.ArgumentType.PluginArgs))
                    this.pluginInitArgs = args.Get<ArgumentCollection>(ArgumentCollection.ArgumentType.PluginArgs);
                if (args.Contains(ArgumentCollection.ArgumentType.RestorePlugin))
                    restorePlugin = args.Get<bool>(ArgumentCollection.ArgumentType.RestorePlugin);
            }
            this.Size = Consts.NormalTileSize;
            if (this.Plugin == null)
                this.InitializePluginCommand = new Command((object parameter) => { });
            else
            {
                this.InitializePluginCommand = new Command((object parameter) => 
                    {
                        if(!(this.Plugin?.IsPluginInitialized() ?? false))
                            this.Plugin?.InitializePlugin(this.pluginInitArgs);
                        this.Plugin?.GetPluginWindow()?.Show();
                    });
            }
            if(restorePlugin)
            {
                this.InitializePluginCommand.Execute(null);
            }
        }

        #endregion
    }
}
