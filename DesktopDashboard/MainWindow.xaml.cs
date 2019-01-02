using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Diagnostics;
using System.Threading;
using Syncfusion.UI.Xaml.Gauges;
using DesktopDashboard.Resources;

namespace DesktopDashboard
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PerformanceCounter cpuTotalCntr = null;


        private readonly ManualResetEvent DashboardUpdateWaitEvent = new ManualResetEvent(false);
        private Task DashboardUpdateTask = null;

        private delegate void ProceedDashboardUpdateDelegate();
        private ProceedDashboardUpdateDelegate ProceedDashboardUpdateDelegateItem;

        SfCircularGauge dcgCpuUsage = null;
        SfCircularGauge dcgRamUsage = null;
        List<GOPartitionInfo> PartitionsInfo = new List<GOPartitionInfo>();
        List<System.IO.DriveInfo> PartitionInfo = new List<System.IO.DriveInfo>();


        const double BToGbRefactor = (1.0 / 1024.0 / 1024.0 / 1024.0);

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                Left = SystemParameters.PrimaryScreenWidth - Width;

                ProceedDashboardUpdateDelegateItem = new ProceedDashboardUpdateDelegate(ProceedDashboardUpdate);

                #region CPUUsage
                dcgCpuUsage = new SfCircularGauge();

                TextBlock tbCpuUsageHeader = new TextBlock()
                {
                    Text = "CPU (%)",
                    FontSize = 15,
                    Foreground = new SolidColorBrush(Colors.Black)
                };
                dcgCpuUsage.GaugeHeader = tbCpuUsageHeader;
                dcgCpuUsage.HeaderAlignment = HeaderAlignment.Custom;
                dcgCpuUsage.GaugeHeaderPosition = new Point(0.35, 0.70);


                dcgCpuUsage.Scales = new System.Collections.ObjectModel.ObservableCollection<CircularScale>();
                CircularScale csItem = new CircularScale();
                csItem.StartValue = 0;
                csItem.EndValue = 100;
                csItem.SweepAngle = 270;
                csItem.Interval = 10;
                csItem.MinorTicksPerInterval = 4;

                csItem.Ranges = new CircularRangeCollection();
                csItem.Ranges.Add(new CircularRange()
                {
                    StartValue = 0,
                    EndValue = 85,
                    Stroke = new LinearGradientBrush(Colors.LightGreen, Colors.DarkGreen, 45),
                });
                csItem.Ranges.Add(new CircularRange()
                {
                    StartValue = 85,
                    EndValue = 95,
                    Stroke = new LinearGradientBrush(Colors.Yellow, Colors.Orange, 45),
                });
                csItem.Ranges.Add(new CircularRange()
                {
                    StartValue = 95,
                    EndValue = 100,
                    Stroke = new LinearGradientBrush(Colors.Red, Colors.DarkRed, 45),
                });

                csItem.Pointers.Add(new CircularPointer()
                {
                    PointerType = PointerType.NeedlePointer,
                    Value = 0,
                    NeedlePointerType = NeedlePointerType.Triangle,
                    NeedlePointerStrokeThickness = 15,
                    NeedlePointerStroke = new LinearGradientBrush(Colors.DarkGray, Colors.Black, 45),
                    PointerCapStroke = new SolidColorBrush(Colors.Black)
                });

                dcgCpuUsage.Scales.Add(csItem);

                wpDashboard.Children.Add(dcgCpuUsage);

                if (PerformanceCounterCategory.Exists("Processor"))
                    cpuTotalCntr = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
                else
                    cpuTotalCntr = new PerformanceCounter("Processor Information", "% Processor Time", "_Total", true);
                // Logger.Add(EventID.InitCPUCounter);
                #endregion
                #region RAMUsage
                dcgRamUsage = new SfCircularGauge();

                TextBlock tbRamUsageHeader = new TextBlock()
                {
                    Text = "RAM (%)",
                    FontSize = 15,
                    Foreground = new SolidColorBrush(Colors.Black),
                    TextAlignment = TextAlignment.Center
                };
                dcgRamUsage.GaugeHeader = tbRamUsageHeader;
                dcgRamUsage.HeaderAlignment = HeaderAlignment.Custom;
                dcgRamUsage.GaugeHeaderPosition = new Point(0.35, 0.70);

                dcgRamUsage.Scales = new System.Collections.ObjectModel.ObservableCollection<CircularScale>();
                CircularScale csRamItem = new CircularScale();
                csRamItem.StartValue = 0;
                csRamItem.EndValue = 100;
                csRamItem.SweepAngle = 270;
                csRamItem.Interval = 10;
                csRamItem.MinorTicksPerInterval = 4;

                csRamItem.Ranges = new CircularRangeCollection();
                csRamItem.Ranges.Add(new CircularRange()
                {
                    StartValue = 0,
                    EndValue = 85,
                    Stroke = new LinearGradientBrush(Colors.LightGreen, Colors.DarkGreen, 45),
                });
                csRamItem.Ranges.Add(new CircularRange()
                {
                    StartValue = 85,
                    EndValue = 95,
                    Stroke = new LinearGradientBrush(Colors.Yellow, Colors.Orange, 45),
                });
                csRamItem.Ranges.Add(new CircularRange()
                {
                    StartValue = 95,
                    EndValue = 100,
                    Stroke = new LinearGradientBrush(Colors.Red, Colors.DarkRed, 45),
                });

                csRamItem.Pointers.Add(new CircularPointer()
                {
                    PointerType = PointerType.NeedlePointer,
                    Value = 0,
                    NeedlePointerType = NeedlePointerType.Triangle,
                    NeedlePointerStrokeThickness = 15,
                    NeedlePointerStroke = new LinearGradientBrush(Colors.DarkGray, Colors.Black, 45),
                    PointerCapStroke = new SolidColorBrush(Colors.Black)
                });

                dcgRamUsage.Scales.Add(csRamItem);

                wpDashboard.Children.Add(dcgRamUsage);
                #endregion
                #region PartitionInfo

                try
                {
                    System.IO.DriveInfo[] allDrives = System.IO.DriveInfo.GetDrives();
                    if (allDrives != null && allDrives.Length > 0)
                        this.PartitionInfo = allDrives.Where(d => d.DriveType == System.IO.DriveType.Fixed).ToList();
                }
                catch (Exception ex)
                {

                }

                if(this.PartitionInfo.Count > 0)
                {
                    foreach(System.IO.DriveInfo diItem in this.PartitionInfo)
                    {
                        //DockPanel dpItem = new DockPanel();

                        //TextBlock tbPartitionName = new TextBlock();
                        //tbPartitionName.Name = "tbPartitionName";
                        //tbPartitionName.Text = String.Format("{0} ({1})", String.IsNullOrEmpty(diItem.VolumeLabel) ? "Disc" : diItem.VolumeLabel, diItem.Name);
                        //tbPartitionName.FontSize = 15;
                        //tbPartitionName.Padding = new Thickness(15, 0, 0, 0);
                        //tbPartitionName.Background = new SolidColorBrush(ConvertColorType(System.Drawing.Color.White));
                        //DockPanel.SetDock(tbPartitionName, Dock.Top);
                        //dpItem.Children.Add(tbPartitionName);

                        //TextBlock tbPartitionInfo = new TextBlock();
                        //tbPartitionInfo.Name = "tbPartitionInfo";
                        //tbPartitionInfo.Text = String.Format("{0} GB free of {1} GB", Math.Round(diItem.TotalFreeSpace * BToGbRefactor, 1), Math.Round(diItem.TotalSize * BToGbRefactor, 1));
                        //tbPartitionInfo.FontSize = 15;
                        //tbPartitionInfo.Padding = new Thickness(15, 0, 0, 0);
                        //tbPartitionInfo.Background = new SolidColorBrush(ConvertColorType(System.Drawing.Color.White));
                        //DockPanel.SetDock(tbPartitionInfo, Dock.Bottom);
                        //dpItem.Children.Add(tbPartitionInfo);

                        //int scaleBarSize = 25;
                        //double scaleBarHeight = (double)scaleBarSize * 1.5;
                        ////Try BulletGraph
                        //SfLinearGauge dlgPartitionInfo = new SfLinearGauge();
                        //dlgPartitionInfo.Tag = diItem;

                        //dlgPartitionInfo.MinWidth = this.Width;
                        //dlgPartitionInfo.MaxWidth = this.Width;
                        //dlgPartitionInfo.Width = this.Width;

                        //dlgPartitionInfo.MinHeight = scaleBarHeight;
                        //dlgPartitionInfo.MaxHeight = scaleBarHeight;
                        //dlgPartitionInfo.Height = scaleBarHeight;

                        //LinearScale lcItem = new LinearScale(); 
                        //lcItem.ScaleDirection = LinearScaleDirection.Forward;
                        //lcItem.Minimum = 0;
                        //lcItem.Maximum = Math.Round(diItem.TotalSize * BToGbRefactor, 1);
                        //lcItem.LabelVisibility = Visibility.Collapsed;
                        ////lcItem.LabelPostfix = "GB";
                        //lcItem.ScaleBarSize = scaleBarSize;
                        //lcItem.ScaleBarStroke = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
                        //lcItem.TickPosition = LinearTicksPosition.Cross;
                        //lcItem.MinorTicksPerInterval = 0;
                        //lcItem.Interval = lcItem.Maximum;// / 5.0;
                        //lcItem.Pointers.Add(new LinearPointer() { PointerType = LinearPointerType.BarPointer });

                        //lcItem.MinWidth = this.Width;
                        //lcItem.MaxWidth = this.Width;
                        //lcItem.Width = this.Width;

                        //lcItem.MinHeight = scaleBarHeight;
                        //lcItem.MaxHeight = scaleBarHeight;
                        //lcItem.Height = scaleBarHeight;

                        //dlgPartitionInfo.MainScale = lcItem;
                        //dlgPartitionInfo.Background = new SolidColorBrush(ConvertColorType(System.Drawing.Color.Yellow));
                        //dpItem.Children.Add(dlgPartitionInfo);

                        GOPartitionInfo piItem = new GOPartitionInfo(diItem, this.Width);
                        wpDashboard.Children.Add(piItem);
                        this.PartitionsInfo.Add(piItem);
                    }
                }

                #endregion

                DashboardUpdateTask = new Task(() => DashboardUpdater(), TaskCreationOptions.LongRunning);
                DashboardUpdateTask.Start();
            }
            catch(Exception ex)
            {

            }
        }

        
        private static System.Windows.Media.Color ConvertColorType(System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private static ImageSourceConverter ImageSourceConverter = new ImageSourceConverter();
        private static System.Windows.Media.ImageSource ConvertImage(System.Drawing.Bitmap image)
        {
            if (image == null)
                return null;
            return (ImageSource)ImageSourceConverter.ConvertFrom(image);
        }

        private void DashboardUpdater()
        {
            while (!DashboardUpdateWaitEvent.WaitOne(1000, false))
            {
                try
                {
                    if (DashboardUpdateWaitEvent.WaitOne(0, false))
                        break;
                    this.Dispatcher.Invoke(ProceedDashboardUpdateDelegateItem);
                }
                catch (Exception ex)
                {
                    //Log(EventID.ESBDriver.SendMessageFromQueueException, ex.Message, ex.StackTrace);
                }
            }

        }

        private void ProceedDashboardUpdate()
        {

            try
            {

                DateTime timeStamp = DateTime.Now;
                #region CPUUsage
                int cpuTotalValue = Convert.ToInt32(Math.Round(cpuTotalCntr.NextValue(), 0));

                dcgCpuUsage.Scales[0].Pointers[0].Value = cpuTotalValue;
                #endregion
                #region RAMUsage

                long valRamTotalMemory = PerformanceInfo.GetTotalMemoryInMiB();
                long valRamTotal = PerformanceInfo.GetTotalMemoryInMiB() - PerformanceInfo.GetPhysicalAvailableMemoryInMiB();
                int valRamTakenPrc = Convert.ToInt32(((double)valRamTotal / (double)valRamTotalMemory) * 100.0);

                dcgRamUsage.Scales[0].Pointers[0].Value = valRamTakenPrc;
                (dcgRamUsage.GaugeHeader as TextBlock).Text = String.Format("RAM %{0}({1:0.00} {2})", Environment.NewLine,
                    valRamTotal > 1024 ? Math.Round(valRamTotal / 1024.0, 2) : valRamTotal,
                    valRamTotal > 1024 ? "GB" : "MB");
                #endregion
                #region PartitionInfo

                foreach(GOPartitionInfo piItem in this.PartitionsInfo)
                {
                    //System.IO.DriveInfo diItem = dlgItem.Tag as System.IO.DriveInfo;
                    //dlgItem.MainScale.Pointers[0].Value = dlgItem.MainScale.Maximum - diItem.TotalFreeSpace * BToGbRefactor;
                    piItem.Refresh();
                }

                #endregion
            }
            catch (Exception ex)
            {
                //Log(EventID.ESBDriver.SendMessageFromQueueException, ex.Message, ex.StackTrace);
            }
        }



        private void LoadPerformanceCuntersCategories()
        {
            List<BuoPerformanceCounterCategory> performanceCounterCategories = PerformanceCounterCategory.GetCategories().Select(c => new BuoPerformanceCounterCategory(c)).ToList();
            foreach (BuoPerformanceCounterCategory category in performanceCounterCategories.OrderBy(c => c.ToString()))
            {
                //lbSystemCountersCategories.Items.Add(category);

            }
        }
        private List<BuoPerformanceCounterCategory> GetPerformanceCategory(string categoryName)
        {
            PerformanceCounterCategory[] pccArray = PerformanceCounterCategory.GetCategories();
            List<BuoPerformanceCounterCategory> retList = new List<BuoPerformanceCounterCategory>();
            if (pccArray != null)
            {
                retList = pccArray.Where(c => String.Equals(c.CategoryName.ToUpper(), categoryName.ToUpper()))
                                  .Select(c => new BuoPerformanceCounterCategory(c)).ToList();
            }
            return retList;
        }
        private List<BuoPerformanceCounter> GetPerformanceCuntersByCategory(BuoPerformanceCounterCategory performanceCategory)
        {
            if (performanceCategory != null && performanceCategory.PerformanceCounterCategory != null)
            {
                //lbSystemCounters.Items.Clear();
                List<BuoPerformanceCounter> performanceCounters = new List<BuoPerformanceCounter>();
                string[] instances = performanceCategory.PerformanceCounterCategory.GetInstanceNames();
                if (instances.Any())
                {
                    foreach (string instance in instances)
                    {
                        if (performanceCategory.PerformanceCounterCategory.InstanceExists(instance))
                        {
                            PerformanceCounter[] countersOfCategory = performanceCategory.PerformanceCounterCategory.GetCounters(instance);
                            foreach (PerformanceCounter pc in countersOfCategory)
                            {
                                performanceCounters.Add(new BuoPerformanceCounter(pc));
                            }
                        }
                    }
                }
                else
                {
                    PerformanceCounter[] countersOfCategory = performanceCategory.PerformanceCounterCategory.GetCounters();
                    foreach (PerformanceCounter pc in countersOfCategory.OrderBy(c => c.ToString()))
                    {
                        performanceCounters.Add(new BuoPerformanceCounter(pc));
                    }
                }
                return performanceCounters;
                //performanceCounters.ForEach(c => lbSystemCounters.Items.Add(c));
            }
            return new List<BuoPerformanceCounter>();
        }

        private void baToolBarCloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void baToolBarTopMost_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = !this.Topmost;
            if (this.Topmost)
                baToolBarTopMost.SmallIcon = Common.Common.GetBitmapImage(Common.Enums.ImageName.LockOpen, Common.Enums.ImageSize._48x48);//(System.Windows.Media.Imaging.BitmapImage)BitmapConverter.Convert(ResourceImage48.LockOpen, typeof(BitmapImage), null, Thread.CurrentThread.CurrentCulture);// ConvertImage(ResourceImage48.LockOpen);
            else
                baToolBarTopMost.SmallIcon = Common.Common.GetBitmapImage(Common.Enums.ImageName.Lock, Common.Enums.ImageSize._48x48);//(System.Windows.Media.Imaging.BitmapImage)BitmapConverter.Convert(ResourceImage48.Lock, typeof(BitmapImage), null, Thread.CurrentThread.CurrentCulture);//ConvertImage(ResourceImage48.Lock);
        }

        #region PerformanceInfo
        public static class PerformanceInfo
        {
            [System.Runtime.InteropServices.DllImport("psapi.dll", SetLastError = true)]
            [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
            public static extern bool GetPerformanceInfo([System.Runtime.InteropServices.Out] out PerformanceInformation PerformanceInformation, [System.Runtime.InteropServices.In] int Size);

            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            public struct PerformanceInformation
            {
                public int Size;
                public IntPtr CommitTotal;
                public IntPtr CommitLimit;
                public IntPtr CommitPeak;
                public IntPtr PhysicalTotal;
                public IntPtr PhysicalAvailable;
                public IntPtr SystemCache;
                public IntPtr KernelTotal;
                public IntPtr KernelPaged;
                public IntPtr KernelNonPaged;
                public IntPtr PageSize;
                public int HandlesCount;
                public int ProcessCount;
                public int ThreadCount;
            }

            public static Int64 GetPhysicalAvailableMemoryInMiB()
            {
                PerformanceInformation pi = new PerformanceInformation();
                if (GetPerformanceInfo(out pi, System.Runtime.InteropServices.Marshal.SizeOf(pi)))
                {
                    return Convert.ToInt64((pi.PhysicalAvailable.ToInt64() * pi.PageSize.ToInt64() / 1048576));
                }
                else
                {
                    return -1;
                }

            }

            public static Int64 GetTotalMemoryInMiB()
            {
                PerformanceInformation pi = new PerformanceInformation();
                if (GetPerformanceInfo(out pi, System.Runtime.InteropServices.Marshal.SizeOf(pi)))
                {
                    return Convert.ToInt64((pi.PhysicalTotal.ToInt64() * pi.PageSize.ToInt64() / 1048576));
                }
                else
                {
                    return -1;
                }

            }
        }
        #endregion
        public class GOPartitionInfo : DockPanel
        {
            private SfLinearGauge Gauge { get; set; }
            private TextBlock PartitionName { get; set; }
            private TextBlock PartitionInfo { get; set; }
            private System.IO.DriveInfo DriveInfo { get; set; }

            private double TotalSize = 0;

            public GOPartitionInfo(System.IO.DriveInfo driveInfo, double width)
            {
                this.DriveInfo = driveInfo;
                this.Width = width;
                this.TotalSize = Math.Round(driveInfo.TotalSize * BToGbRefactor, 1);

                TextBlock tbPartitionName = new TextBlock();
                tbPartitionName.Name = "tbPartitionName";
                tbPartitionName.Text = String.Format("{0} ({1})", String.IsNullOrEmpty(driveInfo.VolumeLabel) ? "Disc" : driveInfo.VolumeLabel, driveInfo.Name);
                tbPartitionName.FontSize = 15;
                tbPartitionName.Padding = new Thickness(15, 0, 0, 0);
                this.PartitionName = tbPartitionName;
                DockPanel.SetDock(this.PartitionName, Dock.Top);
                this.Children.Add(this.PartitionName);

                TextBlock tbPartitionInfo = new TextBlock();
                tbPartitionInfo.Name = "tbPartitionInfo";
                tbPartitionInfo.FontSize = 15;
                tbPartitionInfo.Padding = new Thickness(15, 0, 0, 0);
                this.PartitionInfo = tbPartitionInfo;
                DockPanel.SetDock(this.PartitionInfo, Dock.Bottom);
                this.Children.Add(this.PartitionInfo);

                int scaleBarSize = 25;
                double scaleBarHeight = (double)scaleBarSize * 1.5;
                //Try BulletGraph
                SfLinearGauge dlgPartitionInfo = new SfLinearGauge();
                dlgPartitionInfo.Tag = driveInfo;

                dlgPartitionInfo.MinWidth = this.Width;
                dlgPartitionInfo.MaxWidth = this.Width;
                dlgPartitionInfo.Width = this.Width;

                dlgPartitionInfo.MinHeight = scaleBarHeight;
                dlgPartitionInfo.MaxHeight = scaleBarHeight;
                dlgPartitionInfo.Height = scaleBarHeight;

                LinearScale lcItem = new LinearScale();
                lcItem.ScaleDirection = LinearScaleDirection.Forward;
                lcItem.Minimum = 0;
                lcItem.Maximum = this.TotalSize;
                lcItem.LabelVisibility = Visibility.Collapsed;
                //lcItem.LabelPostfix = "GB";
                lcItem.ScaleBarSize = scaleBarSize;
                lcItem.ScaleBarStroke = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
                lcItem.TickPosition = LinearTicksPosition.Cross;
                lcItem.MinorTicksPerInterval = 0;
                lcItem.Interval = lcItem.Maximum;// / 5.0;
                lcItem.Pointers.Add(new LinearPointer() { PointerType = LinearPointerType.BarPointer });

                lcItem.MinWidth = this.Width;
                lcItem.MaxWidth = this.Width;
                lcItem.Width = this.Width;

                lcItem.MinHeight = scaleBarHeight;
                lcItem.MaxHeight = scaleBarHeight;
                lcItem.Height = scaleBarHeight;

                dlgPartitionInfo.MainScale = lcItem;
                this.Gauge = dlgPartitionInfo;
                this.Children.Add(this.Gauge);
            }

            public void Refresh()
            {
                this.Gauge.MainScale.Pointers[0].Value = this.Gauge.MainScale.Maximum - this.DriveInfo.TotalFreeSpace * BToGbRefactor;
                this.PartitionInfo.Text = String.Format("{0} GB free of {1} GB", GetTotalFreeSpace(), this.TotalSize);
            }
            private double GetTotalFreeSpace()
            {
                return Math.Round(this.DriveInfo.TotalFreeSpace * BToGbRefactor, 1);
            }
        }
    }
}
