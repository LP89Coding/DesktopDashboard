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
                case nameof(IWindowPropertyChangeNotifier.Height):
                    this.Height = (double)(propertyValue ?? 0);
                    break;
                case nameof(IWindowPropertyChangeNotifier.TaskBarProgressState):
                    this.TaskBarProgressState = (TaskbarItemProgressState)(propertyValue ?? TaskbarItemProgressState.None);
                    break;
                case nameof(IWindowPropertyChangeNotifier.TaskBarProgressValue):
                    this.TaskBarProgressValue = (double)(propertyValue ?? 0);
                    break;
                case nameof(IWindowPropertyChangeNotifier.Width):
                    this.Width = (double)(propertyValue ?? 0);
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
                if (args.Contains(ArgumentCollection.ArgumentType.Width))
                    this.Width = args.Get<double>(ArgumentCollection.ArgumentType.Width);
                if (args.Contains(ArgumentCollection.ArgumentType.Height))
                    this.Height = args.Get<double>(ArgumentCollection.ArgumentType.Height);
            }
            if (this.Width == 0 && !args.Contains(ArgumentCollection.ArgumentType.Width))
                this.width = 800;
            if (this.Height == 0 && !args.Contains(ArgumentCollection.ArgumentType.Height))
                this.Height = 600;
        }

        #endregion
    }
}
