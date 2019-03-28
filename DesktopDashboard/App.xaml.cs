using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using WPF.Common.Common;
using WPF.Common.Interfaces;
using WPF.Common.Logger;
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
        #region Overrides

        #region OnStartup

        protected override void OnStartup(StartupEventArgs e)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            try
            {
                sw.Start();
                Logger.Log(EventID.DesktopDashboard.Application.StartupEnter);
                base.OnStartup(e);
                WindowState windowState = null;
                wDesktopDashboard control = new wDesktopDashboard();
                try
                {
                    windowState = UserSettings.LoadSetting<WindowState>(UserSettings.SettingType.WindowState);
                }
                catch (Exception ex)
                {
                    Logger.Log(EventID.DesktopDashboard.Application.Exception, nameof(UserSettings.LoadSetting), ex);
                }
                if (windowState == null)
                {
                    windowState = new WindowState();
                    windowState.Width = Consts.DefaultWindowWidth;
                    windowState.Height = Consts.DefaultWindowHeight;

                    windowState.PositionTop = 0;
                    windowState.PositionLeft = SystemParameters.PrimaryScreenWidth - windowState.Width;
                    windowState.TopMost = false;
                    try
                    {
                        UserSettings.SaveSetting(UserSettings.SettingType.WindowState, windowState);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(EventID.DesktopDashboard.Application.Exception, nameof(UserSettings.SaveSetting), ex);
                    }
                }
                ArgumentCollection args = new ArgumentCollection();
                args.Set(ArgumentCollection.ArgumentType.WindowState, windowState);
                args.Set(ArgumentCollection.ArgumentType.WindowTitle, Consts.WindowTitle);
                args.Set(ArgumentCollection.ArgumentType.WindowIcon, DesktopDashboard.Resources.ResourceImage.WindowIcon);
                args.Set(ArgumentCollection.ArgumentType.WindowCloseCommand, new Command((object parameter) =>
                {
                    control?.Close();
                    this.mainWindow?.Close();
                }));

                this.mainWindow = new BaseWindow(args);
                this.mainWindow.SetContent(control);
                this.mainWindow.Show();
            }
            catch(Exception ex)
            {
                Logger.Log(EventID.DesktopDashboard.Application.Exception, nameof(this.OnStartup), ex);
            }
            finally
            {
                sw.Stop();
                Logger.Log(EventID.DesktopDashboard.Application.StartupExit, sw.ElapsedMilliseconds);
            }
        }

        #endregion
        #region OnExit

        protected override void OnExit(ExitEventArgs e)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            try
            {
                sw.Start();
                Logger.Log(EventID.DesktopDashboard.Application.EndEnter);

                WindowState windowState = mainWindow?.GetWindowState();
                if(windowState != null)
                    UserSettings.SaveSetting(UserSettings.SettingType.WindowState, windowState);
                this.mainWindow = null;
                base.OnExit(e);
            }
            catch (Exception ex)
            {
                Logger.Log(EventID.DesktopDashboard.Application.Exception, nameof(this.OnExit), ex);
            }
            finally
            {
                sw.Stop();
                Logger.Log(EventID.DesktopDashboard.Application.EndExit, sw.ElapsedMilliseconds);
            }
        }

        #endregion

        #endregion
    }
}
