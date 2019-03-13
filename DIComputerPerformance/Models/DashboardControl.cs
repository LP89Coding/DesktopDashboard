using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPF.Common.Interfaces;
using Enums = WPF.Common.Enums;

namespace DIComputerPerformance.Models
{
    public class DashboardControl : IObjectState
    {
        public int IdDashboardControl { get; set; }
        public int IdDashboardControlGroup { get; set; }

        public int IndexNbr { get; set; }

        public object Control { get; protected set; }

        public DashboardControlGroup Group { get; set; }

        public DashboardControl()
        {
            this.State = Enums.ObjectState.New;
        }

        public virtual void Refresh()
        {

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
