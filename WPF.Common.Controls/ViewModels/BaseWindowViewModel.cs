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


        private ICommand closeButtonCommand;
        public ICommand CloseButtonCommand { get { return this.closeButtonCommand; } private set { this.closeButtonCommand = value; } }

        public BaseWindowViewModel()
        {
        }

        #region CombineWindowTitle

        private string CombineWindowTitle(string title)
        {
            return this.TaskBarProgressState == TaskbarItemProgressState.Normal ?
                        $"{title} ({Math.Round(this.TaskBarProgressValue * 100.0, 0)}%)" :
                        title;
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
            if(args != null)
            {
                if (args.Contains(ArgumentCollection.ArgumentType.WindowTitle))
                    this.WindowTitle = args.Get(ArgumentCollection.ArgumentType.WindowTitle)?.ToString();
                if (args.Contains(ArgumentCollection.ArgumentType.WindowIcon))
                    this.WindowIcon = WPFUtils.ToBitmapImage(args.Get(ArgumentCollection.ArgumentType.WindowIcon));
                if (args.Contains(ArgumentCollection.ArgumentType.WindowCloseCommand))
                    this.CloseButtonCommand = args.Get<Command>(ArgumentCollection.ArgumentType.WindowCloseCommand);
                if (args.Contains(ArgumentCollection.ArgumentType.WindowWidth))
                    this.WindowWidth = args.Get<double>(ArgumentCollection.ArgumentType.WindowWidth);
                if (args.Contains(ArgumentCollection.ArgumentType.WindowHeight))
                    this.WindowHeight = args.Get<double>(ArgumentCollection.ArgumentType.WindowHeight);
                if (args.Contains(ArgumentCollection.ArgumentType.WindowTitle))
                    this.WindowTitle = args.Get<string>(ArgumentCollection.ArgumentType.WindowTitle);
            }
            if (this.WindowWidth == 0 && !args.Contains(ArgumentCollection.ArgumentType.WindowWidth))
                this.WindowWidth = 800;
            if (this.WindowHeight == 0 && !args.Contains(ArgumentCollection.ArgumentType.WindowHeight))
                this.WindowHeight = 600;
            if (String.IsNullOrWhiteSpace(this.WindowTitle) && !args.Contains(ArgumentCollection.ArgumentType.WindowTitle))
                this.WindowTitle = "Base Window";
        }

        #endregion
    }
}
