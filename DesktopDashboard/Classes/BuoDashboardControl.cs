using DesktopDashboard.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enums = WPF.Common.Enums;

namespace DesktopDashboard.Classes
{
    public class BuoDashboardControl : IObjectState
    {
        public int IdDashboardControl { get; set; }
        public int IdDashboardControlGroup { get; set; }

        public int IndexNbr { get; set; }

        public BuoDashboardControlGroup Group { get; set; }

        public BuoDashboardControl()
        {
            this.State = Enums.ObjectState.New;
        }

        #region IObjectState implementation
        private Enums.ObjectState _State;
        public Enums.ObjectState State
        {
            get { return this._State; }
            set
            {
                this._State = value;
                if (this.Group != null && 
                    (value == Enums.ObjectState.Modified || value == Enums.ObjectState.Delete))
                    this.Group.State = Enums.ObjectState.Modified;
            }
        }
        #endregion
    }
}
