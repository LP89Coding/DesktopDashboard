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

            return pluginManager.GetPlugins()?.Select(p =>
            {
                ArgumentCollection args = new ArgumentCollection();
                args.Set(ArgumentCollection.ArgumentType.Plugin, p);
                return factory.CreateViewModel<PluginViewModel>(args);
            }).ToList() ?? new List<PluginViewModel>();
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
            try
            {
                Syncfusion.Windows.Tools.Controls.DockingManager dmItem = args.Get<Syncfusion.Windows.Tools.Controls.DockingManager>(ArgumentCollection.ArgumentType.DockingManager);
                System.Collections.ObjectModel.ObservableCollection<Syncfusion.Windows.Tools.Controls.DockItem> dockItems = new System.Collections.ObjectModel.ObservableCollection<Syncfusion.Windows.Tools.Controls.DockItem>();
                foreach (Plugin pItem in this.AvailablePlugins.Select(p => p.Plugin))
                {
                    pItem.InitializePlugin(null);
                    IWindowControl control = pItem.GetPluginControl();

                    //Brush b = new System.Windows.Media.ImageBrush(Utils.ToImageSource(pItem.GetPluginIcon()));
                    Syncfusion.Windows.Tools.Controls.DockItem diItem = new Syncfusion.Windows.Tools.Controls.DockItem()
                    {
                        Content = control as FrameworkElement,
                        Header = pItem.GetPluginName(),
                        Icon = new System.Windows.Media.ImageBrush(Utils.ToImageSource(pItem.GetPluginIcon()))//Nie działa
                    };
                    dockItems.Add(diItem);
                    dmItem.Children.Add(control as FrameworkElement);//TO sprawia że menu kontekstowe się dobrze wyświetla - ostylowane

                }
                dmItem.ItemsSource = dockItems;
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}
