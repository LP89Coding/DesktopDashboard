using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Syncfusion.UI.Xaml.Gauges;

using WPF.Common.Common;

using DIComputerPerformance.Internals;
using DIComputerPerformance.Views;

namespace DIComputerPerformance.Models
{
    public class PartitionInfoControl : DashboardControl
    {
        private System.IO.DriveInfo DriveInfo { get; set; }
        

        public PartitionInfoControl(System.IO.DriveInfo driveInfo)
        {
            this.DriveInfo = driveInfo;
        }

        public void Initialize()
        {
            ArgumentCollection args = new ArgumentCollection();
            args.Set(ArgumentCollection.ArgumentType.DriveInfo, this.DriveInfo);
            this.Control = new ucPartitionInfo(args);
        }

        public override void Refresh()
        {
            if (this.Control is ucPartitionInfo)
            {
                (this.Control as ucPartitionInfo).Refresh();
            }
        }
    }
}
