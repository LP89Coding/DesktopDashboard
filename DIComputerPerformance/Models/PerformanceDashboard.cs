﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPF.Common.Interfaces;
using Enums = WPF.Common.Enums;

namespace DIComputerPerformance.Models
{
    public class PerformanceDashboard : IObjectState
    {
        public string Resolution { get { return String.Format("{0}x{1}", this.ScreenWidth, this.ScreenHeight); } }
        public int ScreenWidth { get; private set; }//InPixel
        public int ScreenHeight { get; private set; }//InPixel

        public int Width { get; private set; }//InPixel
        public int Height { get; private set; }//InPixel

        public List<DashboardControlGroup> ControlGroups { get; private set; }

        public PerformanceDashboard()
        {
            this.State = Enums.ObjectState.New;

            this.ScreenWidth = Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenWidth);
            this.ScreenHeight = Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenHeight);

            this.Width = 200;
            this.Height = 200;
            
            this.ControlGroups = new List<DashboardControlGroup>();
        }

        #region IObjectState implementation
        private Enums.ObjectState _State;
        public Enums.ObjectState State
        {
            get { return this._State; }
            set
            {
                this._State = value;
                if (value == Enums.ObjectState.Delete)
                {
                    if (this.ControlGroups != null && this.ControlGroups.Count > 0)
                        this.ControlGroups.ForEach(c => c.State = value);
                }
            }
        }
        #endregion
    }
}
