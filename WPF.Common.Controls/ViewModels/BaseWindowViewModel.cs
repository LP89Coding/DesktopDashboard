using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using System.Windows;
using System.Windows.Interop;

using WPF.Common.Common;
using WPF.Common.Interfaces;
using WPFUtils = WPF.Common.Common.Utils;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;

namespace WPF.Common.Controls.ViewModels
{
    public class BaseWindowViewModel : ObservableViewModel, IViewModel
    {

        private string windowTitle;
        public string WindowTitle
        {
            get { return this.CombineWindowTitle(this.windowTitle); }
            set
            {
                this.windowTitle = value;
                RaisePropertyChangedEvent(nameof(this.WindowTitle));
            }
        }
        private BitmapImage windowIcon;
        public BitmapImage WindowIcon
        {
            get { return this.windowIcon; }
            set
            {
                this.windowIcon = value;
                RaisePropertyChangedEvent(nameof(this.WindowIcon));
            }
        }
        private double taskBarProgressValue;
        public double TaskBarProgressValue
        {
            get { return this.taskBarProgressValue; }
            set
            {
                this.taskBarProgressValue = value;
                RaisePropertyChangedEvent(nameof(this.TaskBarProgressValue));
                RaisePropertyChangedEvent(nameof(this.WindowTitle));
            }
        }
        private TaskbarItemProgressState taskBarProgressState;
        public TaskbarItemProgressState TaskBarProgressState
        {

            get { return this.taskBarProgressState; }
            set
            {
                this.taskBarProgressState = value;
                RaisePropertyChangedEvent(nameof(this.TaskBarProgressState));
                RaisePropertyChangedEvent(nameof(this.WindowTitle));
            }
        }

        private double windowHeight;
        public double WindowHeight
        {
            get { return this.windowHeight; }
            set
            {
                this.windowHeight = value;
                RaisePropertyChangedEvent(nameof(this.WindowHeight));
            }
        }
        private double windowWidth;
        public double WindowWidth
        {
            get { return this.windowWidth; }
            set
            {
                this.windowWidth = value;
                RaisePropertyChangedEvent(nameof(this.WindowWidth));
            }
        }

        private bool windowTopMost;
        public bool WindowTopMost
        {
            get { return this.windowTopMost; }
            set
            {
                bool valueChanged = this.windowTopMost != value;
                this.windowTopMost = value;
                RaisePropertyChangedEvent(nameof(this.WindowTopMost));
            }
        }
        private double windowTop;
        public double WindowTop
        {
            get { return this.windowTop; }
            set
            {
                this.windowTop = value;
                RaisePropertyChangedEvent(nameof(this.WindowTop));
            }
        }
        private double windowLeft;
        public double WindowLeft
        {
            get { return this.windowLeft; }
            set
            {
                this.windowLeft = value;
                RaisePropertyChangedEvent(nameof(this.WindowLeft));
            }
        }


        private ICommand closeButtonCommand;
        public ICommand CloseButtonCommand { get { return this.closeButtonCommand; } private set { this.closeButtonCommand = value; } }
        private ICommand topMostButtonCommand;
        public ICommand TopMostButtonCommand { get { return this.topMostButtonCommand; } set { this.topMostButtonCommand = value; } }

        public BaseWindowViewModel()
        {
        }

        #region Methods

        #region CombineWindowTitle

        private string CombineWindowTitle(string title)
        {
            return this.TaskBarProgressState == TaskbarItemProgressState.Normal ?
                        $"{title} ({Math.Round(this.TaskBarProgressValue * 100.0, 0)}%)" :
                        title;
        }

        #endregion
        #region TopMostToogle

        private void TopMostToogle(object parameter)
        {
            this.WindowTopMost = !this.WindowTopMost;
        }

        #endregion

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            this.WindowIcon = null;
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
                case nameof(IWindowPropertyChangeNotifier.WindowHeight):
                    this.WindowHeight = (double)(propertyValue ?? 0);
                    break;
                case nameof(IWindowPropertyChangeNotifier.TaskBarProgressState):
                    this.TaskBarProgressState = (TaskbarItemProgressState)(propertyValue ?? TaskbarItemProgressState.None);
                    break;
                case nameof(IWindowPropertyChangeNotifier.TaskBarProgressValue):
                    this.TaskBarProgressValue = (double)(propertyValue ?? 0);
                    break;
                case nameof(IWindowPropertyChangeNotifier.WindowWidth):
                    this.WindowWidth = (double)(propertyValue ?? 0);
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }

        public void Initialize(ArgumentCollection args)
        {
            double? windowWidth = null;
            double? windowHeight = null;
            if (args != null)
            {
                if (args.Contains(ArgumentCollection.ArgumentType.WindowTitle))
                    this.WindowTitle = args.Get(ArgumentCollection.ArgumentType.WindowTitle)?.ToString();
                if (args.Contains(ArgumentCollection.ArgumentType.WindowIcon))
                    this.WindowIcon = WPFUtils.ToBitmapImage(args.Get(ArgumentCollection.ArgumentType.WindowIcon));
                if (args.Contains(ArgumentCollection.ArgumentType.WindowCloseCommand))
                    this.CloseButtonCommand = args.Get<Command>(ArgumentCollection.ArgumentType.WindowCloseCommand);
                if (args.Contains(ArgumentCollection.ArgumentType.WindowTitle))
                    this.WindowTitle = args.Get<string>(ArgumentCollection.ArgumentType.WindowTitle);

                Common.WindowState windowState = null;
                
                if (args.Contains(ArgumentCollection.ArgumentType.PluginState))
                    windowState = args.Get<PluginState>(ArgumentCollection.ArgumentType.PluginState)?.WindowState;
                if (args.Contains(ArgumentCollection.ArgumentType.WindowState))
                    windowState = args.Get<Common.WindowState>(ArgumentCollection.ArgumentType.WindowState);

                windowWidth = windowState?.Width;
                windowHeight = windowState?.Height;
                
                if (args.Contains(ArgumentCollection.ArgumentType.WindowWidth) && !windowWidth.HasValue)
                    windowWidth = args.Get<double>(ArgumentCollection.ArgumentType.WindowWidth);
                if (args.Contains(ArgumentCollection.ArgumentType.WindowHeight) && !windowHeight.HasValue)
                    windowHeight = args.Get<double>(ArgumentCollection.ArgumentType.WindowHeight);

                double? windowTop = windowState?.PositionTop;
                double? windowLeft = windowState?.PositionLeft;
                bool? windowTopMost = windowState?.TopMost;

                if (windowTop.HasValue)
                    this.WindowTop = windowTop.Value;
                if (windowLeft.HasValue)
                    this.WindowLeft = windowLeft.Value;
                if (windowTopMost.HasValue)
                    this.WindowTopMost = windowTopMost.Value;
            }
            if (!windowWidth.HasValue)
                windowWidth = 800;
            if (!windowHeight.HasValue)
                windowHeight = 600;

            this.WindowWidth = windowWidth.Value;
            this.WindowHeight = windowHeight.Value;

            if (String.IsNullOrWhiteSpace(this.WindowTitle) && !args.Contains(ArgumentCollection.ArgumentType.WindowTitle))
                this.WindowTitle = "Base Window";

            this.TopMostButtonCommand = new Command((object parameter) => { this.TopMostToogle(parameter); });
        }

        #endregion
    }
}
