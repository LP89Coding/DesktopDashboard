﻿using DesktopDashboard.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDashboard.Classes
{
    public class BuoDashboardControlGroup : IObjectState
    {
        public int IdDashboardControlGroup { get; set; }
        public int IdDashboard { get; private set; }

        public int Width { get; private set; }//InPixel
        public int Height { get; private set; }//InPixel

        public int X { get; private set; }//InPixelFromLeftUpCorner
        public int Y { get; private set; }//InPixelFromLeftUpCorner

        public BuoDashboard Dashboard { get; set; }
        public List<BuoDashboardControl> Controls { get; private set; }

        public BuoDashboardControlGroup()
        {
            this.State = Enums.ObjectState.New;

            this.Controls = new List<BuoDashboardControl>();

            this.X = 0;
            this.Y = 0;

            this.Width = 200;
            this.Height = 200;
        }


        #region IObjectState implementation
        private Enums.ObjectState _State;
        public Enums.ObjectState State
        {
            get { return this._State; }
            set
            {
                this._State = value;
                if (value == Enums.ObjectState.Delete && this.Controls != null && this.Controls.Count > 0)
                    this.Controls.ForEach(c => c.State = value);
                if ((value == Enums.ObjectState.Modified || value == Enums.ObjectState.Delete) &&
                    this.Dashboard != null)
                    this.Dashboard.State = Enums.ObjectState.Modified;
            }
        }
        #endregion
    }
}
