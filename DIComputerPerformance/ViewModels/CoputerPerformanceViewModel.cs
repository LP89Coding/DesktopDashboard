using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Threading;

using Syncfusion.UI.Xaml.Gauges;

using WPF.Common.Common;
using WPF.Common.Interfaces;
using Logger = WPF.Common.Logger.Logger;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;
using WPFUtils = WPF.Common.Common.Utils;

using DIComputerPerformance.Models;
using DIComputerPerformance.Internals;
using DIComputerPerformance.Interfaces;
using DIComputerPerformance.Views;

namespace DIComputerPerformance.ViewModels
{
    public class ComputerPerformanceViewModel : ObservableViewModel, IViewModel, IWindowPropertyChangeNotifier
    {
        public List<IDashboardControl> Controls { get; private set; }
        

        private readonly ManualResetEvent DashboardUpdateWaitEvent = new ManualResetEvent(false);
        private Task DashboardUpdateTask = null;

        private delegate void ProceedDashboardUpdateDelegate();

        #region Ctor

        public ComputerPerformanceViewModel()
        {
            this.Controls = new List<IDashboardControl>();
        }

        #endregion

        #region Methods

        #region DashboardUpdater

        private void DashboardUpdater()
        {
            while (!DashboardUpdateWaitEvent.WaitOne(1000, false))
            {
                try
                {
                    if (DashboardUpdateWaitEvent.WaitOne(0, false))
                        break;
                    if (this.Controls != null)
                    {
                        this.Controls.ForEach(c => c.Refresh());
                    }
                }
                catch (Exception ex)
                {
                    //TODO Log
                    //Log(EventID.ESBDriver.SendMessageFromQueueException, ex.Message, ex.StackTrace);
                }
            }

        }

        #endregion

        #endregion

        #region IViewModel implementation

        public void Dispose()
        {
        }

        public object GetPropertyValue(string propertyName)
        {
            throw new NotImplementedException();
        }

        public void Initialize(ArgumentCollection args)
        {
            if(this.Controls == null)
                this.Controls = new List<IDashboardControl>();
            
            this.Controls.Add(new ucCpuInfo());
            this.Controls.Add(new ucRamInfo());

            #region PartitionInfo

            List<System.IO.DriveInfo> partitionInfo = new List<System.IO.DriveInfo>();
            try
            {
                System.IO.DriveInfo[] allDrives = System.IO.DriveInfo.GetDrives();
                if (allDrives != null && allDrives.Length > 0)
                    partitionInfo = allDrives.Where(d => d.DriveType == System.IO.DriveType.Fixed).ToList();
            }
            catch (Exception ex)
            {
                //ToDo Log
            }

            if (partitionInfo.Count > 0)
            {
                foreach (System.IO.DriveInfo diItem in partitionInfo)
                {
                    ArgumentCollection patitionInfoArgs = new ArgumentCollection();
                    patitionInfoArgs.Set(ArgumentCollection.ArgumentType.DriveInfo, diItem);
                    this.Controls.Add(new ucPartitionInfo(patitionInfoArgs));
                }
            }

            #endregion
            
            DashboardUpdateTask = new Task(() => DashboardUpdater(), TaskCreationOptions.LongRunning);
            DashboardUpdateTask.Start();
        }

        public bool NotifyPropertyChange(string propertyName, object propertyValue)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region IWindowPropertyNotifier implementation
        
        public string WindowTitle { get; set; }
        public double TaskBarProgressValue { get; set; }
        public System.Windows.Shell.TaskbarItemProgressState TaskBarProgressState { get; set; }

        private double height;
        public double WindowHeight
        {
            get { return this.height; }
            set
            {
                this.height = value;
                RaisePropertyChangedEvent(nameof(this.WindowHeight));
            }
        }
        private double width;
        public double WindowWidth
        {
            get { return this.width; }
            set
            {
                this.width = value;
                RaisePropertyChangedEvent(nameof(this.WindowWidth));
            }
        }

        #endregion
    }
}
