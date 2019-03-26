using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using WPF.Common.Common;
using WPF.Common.Interfaces;
using WindowState = WPF.Common.Common.WindowState;

using WPF.Common.Controls.Views;

using DesktopDashboard.Internals;

namespace DesktopDashboard
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IWindow mainWindow { get; set; }
        private wDesktopDashboard control { get; set; }
        #region Overrides

        #region OnStartup

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            WindowState windowState = null;
            this.control = new wDesktopDashboard();
            try
            {
                windowState = UserSettings.LoadSetting<WindowState>(UserSettings.SettingType.WindowState);
            }
            catch(Exception ex)
            {
                //ToDo Log
            }
            if(windowState == null)
            {
                windowState = new WindowState();
                int availablePluginsCount = control.GetAvailablePluginsCount();
                windowState.Width = Consts.DefaultWindowWidth;
                windowState.Height = Consts.DefaultWindowHeight;

                windowState.PositionTop = 0;
                windowState.PositionLeft = SystemParameters.PrimaryScreenWidth - windowState.Width;
                windowState.TopMost = false;
                try
                {
                    UserSettings.SaveSetting(UserSettings.SettingType.WindowState, windowState);
                }
                catch(Exception ex)
                {
                    //ToDo Log
                }
            }
            ArgumentCollection args = new ArgumentCollection();
            args.Set(ArgumentCollection.ArgumentType.WindowState, windowState);
            args.Set(ArgumentCollection.ArgumentType.WindowCloseCommand, new Command((object parameter) => 
            {
                this.control?.Close();
                this.mainWindow?.Close();
            }));

            this.mainWindow = new BaseWindow(args);
            this.mainWindow.SetContent(control);
            this.mainWindow.Show();
        }

        #endregion
        #region OnExit

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {

                WindowState windowState = mainWindow?.GetWindowState();
                if(windowState != null)
                    UserSettings.SaveSetting(UserSettings.SettingType.WindowState, windowState);
                this.mainWindow = null;
            }
            catch (Exception ex)
            {
                //ToDo Log
            }
            base.OnExit(e);
        }

        #endregion

        #endregion
    }
}
