using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Syncfusion.UI.Xaml.Gauges;

using WPF.Common.Common;
using WPF.Common.Interfaces;
using Logger = WPF.Common.Logger.Logger;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;
using WPFUtils = WPF.Common.Common.Utils;

using DIComputerPerformance.Models;
using DIComputerPerformance.Internals;

namespace DIComputerPerformance.ViewModels
{
    public class PartitionInfoViewModel : ObservableViewModel, IViewModel
    {
        private string partitionName;
        public string PartitionName
        {
            get { return this.partitionName; }
            set
            {
                this.partitionName = value;
                RaisePropertyChangedEvent(nameof(this.PartitionName));
            }
        }

        private string partitionInfo;
        public string PartitionInfo
        {
            get { return this.partitionInfo; }
            set
            {
                this.partitionInfo = value;
                RaisePropertyChangedEvent(nameof(this.PartitionInfo));
            }
        }

        private System.IO.DriveInfo driveInfo;
        public System.IO.DriveInfo DriveInfo
        {
            get
            {
                return this.driveInfo;
            }
            private set
            {
                this.driveInfo = value;
                RaisePropertyChangedEvent(nameof(this.DriveInfo));
            }
        }

        private double totalSize;
        public double TotalSize
        {
            get { return this.totalSize; }
            private set
            {
                this.totalSize = value;
                RaisePropertyChangedEvent(nameof(this.TotalSize));
            }
        }

        private int scaleBarSize;
        public int ScaleBarSize
        {
            get { return this.scaleBarSize; }
            private set
            {
                this.scaleBarSize = value;
                RaisePropertyChangedEvent(nameof(this.ScaleBarSize));
            }
        }

        private double scaleBarHeight;
        public double ScaleBarHeight
        {
            get { return this.scaleBarHeight; }
            private set
            {
                this.scaleBarHeight = value;
                RaisePropertyChangedEvent(nameof(this.ScaleBarHeight));
            }
        }

        private double usedSpace;
        public double UsedSpace
        {
            get { return this.usedSpace; }
            private set
            {
                this.usedSpace = value;
                RaisePropertyChangedEvent(nameof(this.UsedSpace));
            }
        }

        #region Methods

        #region GetTotalSize
        private double GetTotalSize()
        {
            return this.DriveInfo == null ? 0.0 : Math.Round(this.DriveInfo.TotalSize * Consts.BToGbRefactor, 1);
        }
        #endregion
        #region GetTotalFreeSpace
        private double GetTotalFreeSpace()
        {
            return this.DriveInfo == null ? 0.0 : Math.Round(this.DriveInfo.TotalFreeSpace * Consts.BToGbRefactor, 1);
        }
        #endregion
        #region RefreshDriveInfo
        public void RefreshDriveInfo()
        {
            this.UsedSpace = this.DriveInfo == null ? 0.0 : this.TotalSize - GetTotalFreeSpace();
            this.PartitionInfo = String.Format("{0} GB free of {1} GB", GetTotalFreeSpace(), this.TotalSize);
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
            this.DriveInfo = args.Get<System.IO.DriveInfo>(ArgumentCollection.ArgumentType.DriveInfo);
            if (this.DriveInfo == null)
                throw new ArgumentException($"{nameof(this.DriveInfo)} cannot be null");
            this.PartitionName = String.Format("{0} ({1})", String.IsNullOrEmpty(this.DriveInfo.VolumeLabel) ? "Disc" : this.DriveInfo.VolumeLabel, this.DriveInfo.Name);      
            this.TotalSize = GetTotalSize();
            this.ScaleBarSize = 25;
            this.ScaleBarHeight = (double)this.ScaleBarSize * 1.5;
        }

        public bool NotifyPropertyChange(string propertyName, object propertyValue)
        {
            bool result = false;
            switch(propertyName)
            {
                case nameof(ArgumentCollection.ArgumentType.ForceRefresh):
                    this.RefreshDriveInfo();
                    result = true;
                    break;
                default:
                    break;
            }
            return result;
        }

        #endregion
    }
}
