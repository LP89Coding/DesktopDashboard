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
    public class CpuUsageControl : DashboardControl
    {
        private System.Diagnostics.PerformanceCounter cpuTotalCntr = null;

        #region Ctor

        public CpuUsageControl() : base()
        {
            if (System.Diagnostics.PerformanceCounterCategory.Exists("Processor"))
                cpuTotalCntr = new System.Diagnostics.PerformanceCounter("Processor", "% Processor Time", "_Total", true);
            else
                cpuTotalCntr = new System.Diagnostics.PerformanceCounter("Processor Information", "% Processor Time", "_Total", true);
        }

        #endregion

        public void Initialize()
        {
            SfCircularGauge dcgCpuUsage = new SfCircularGauge();

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

            this.Control = dcgCpuUsage;
        }

        public override void Refresh()
        {
            if (this.Control is SfCircularGauge)
            {
                SfCircularGauge dcgCpuUsage = this.Control as SfCircularGauge;
                int cpuTotalValue = Convert.ToInt32(Math.Round(cpuTotalCntr.NextValue(), 0));

                dcgCpuUsage.Scales[0].Pointers[0].Value = cpuTotalValue;
            }
        }
    }
}
