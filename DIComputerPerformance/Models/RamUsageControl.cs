using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

using Syncfusion.UI.Xaml.Gauges;

using DIComputerPerformance.Internals;

namespace DIComputerPerformance.Models
{
    public class RamUsageControl : DashboardControl
    {
        #region Ctor

        public RamUsageControl() : base()
        {

        }

        #endregion

        public void Initialize()
        {
            SfCircularGauge dcgRamUsage = new SfCircularGauge();

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
            this.Control = dcgRamUsage;
        }

        public override void Refresh()
        {
            if (this.Control is SfCircularGauge)
            {
                SfCircularGauge dcgRamUsage = this.Control as SfCircularGauge;
                long valRamTotalMemory = PerformanceInfo.GetTotalMemoryInMiB();
                long valRamTotal = PerformanceInfo.GetTotalMemoryInMiB() - PerformanceInfo.GetPhysicalAvailableMemoryInMiB();
                int valRamTakenPrc = Convert.ToInt32(((double)valRamTotal / (double)valRamTotalMemory) * 100.0);

                dcgRamUsage.Scales[0].Pointers[0].Value = valRamTakenPrc;
                (dcgRamUsage.GaugeHeader as TextBlock).Text = String.Format("RAM %{0}({1:0.00} {2})", Environment.NewLine,
                    valRamTotal > 1024 ? Math.Round(valRamTotal / 1024.0, 2) : valRamTotal,
                    valRamTotal > 1024 ? "GB" : "MB");
            }
        }
    }
}
