using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopDashboard
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Overrides

        #region OnStartup

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new wDesktopDashboard().Show();
        }

        #endregion

        #endregion
    }
}
