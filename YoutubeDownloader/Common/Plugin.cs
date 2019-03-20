using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPF.Common.Common;
using WPF.Common.Interfaces;
using Logger = WPF.Common.Logger.Logger;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;

using WPF.Common.Controls.Views;

using DIYoutubeDownloader.Internal;
using EventID = DIYoutubeDownloader.Internal.EventID.DIYoutubeDownloader;

namespace DIYoutubeDownloader.Common
{
    public class Plugin : IPlugin
    {
        private ArgumentCollection args { get; set; }
        private BaseWindow mainWindow { get; set; }
        private IWindowControl control { get; set; }
        private bool isInitialized { get; set; }
        #region Events

        #region UnhandledException_Raised

        private void UnhandledException_Raised(Exception exception, string source)
        {
            string message = null;
            try
            {
                message = $"Unhandled exception in {source}, exception: {exception?.ToString()}.";
                message += $"Exception in {this.GetPluginAssemblyName()} v{this.GetPluginAssemblyVersion()}";
            }
            catch (Exception ex)
            {
                Logger.Log(EventID.Application.UnhandledExceptionException, ex);
            }
            finally
            {
                Logger.Log(EventID.Application.UnhandledException, message);
            }
        }

        #endregion

        #endregion
        #region Methods

        #region Initialize
        private void Initialize()
        {
            try
            {
                Logger.Log(EventID.Application.Start);
                #region GlobalUnhandledExceptionEvents

                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                    UnhandledException_Raised((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

                System.Windows.Application.Current.DispatcherUnhandledException += (s, e) =>
                    UnhandledException_Raised(e.Exception, "Application.Current.DispatcherUnhandledException");

                TaskScheduler.UnobservedTaskException += (s, e) =>
                    UnhandledException_Raised(e.Exception, "TaskScheduler.UnobservedTaskException");

                #endregion
                args.Set(ArgumentCollection.ArgumentType.WindowIcon, ResourceImage.WindowIcon);
                args.Set(ArgumentCollection.ArgumentType.WindowTitle, Consts.WindowTitle);
                args.Set(ArgumentCollection.ArgumentType.WindowWidth, Consts.WindowDefaultWidth);
                args.Set(ArgumentCollection.ArgumentType.WindowHeight, Consts.WindowDefaultHeigth);
                args.Set(ArgumentCollection.ArgumentType.WindowCloseCommand, new Command((object parametrer) => { this.ClosePlugin(); }));
            }
            catch (Exception ex)
            {
                Logger.Log(EventID.Application.Exception, ex);
            }
        }
        #endregion
        #region InitializeControl
        private IWindowControl InitializeControl()
        {
            this.control = new ucYoutubeDownloader();
            return this.control;
        }
        #endregion
        #region InitializeWindow
        private IWindow InitializeWindow()
        {
            if (this.control == null)
                this.control = this.InitializeControl();
            mainWindow = new BaseWindow(args);
            mainWindow.SetContent(this.control);
            return mainWindow;
        }
        #endregion
        #region Close

        private void Close()
        {
            try
            {
                this.isInitialized = false;
                Logger.Log(EventID.Application.End);
                try { Logger.Close(); } catch (Exception ex) { Console.WriteLine(ex.ToString()); Logger.Log(EventID.Application.Exception, ex); }
                mainWindow?.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion

        #endregion
        #region IPlugin implementation

        public void InitializePlugin(ArgumentCollection args)
        {
            this.args = args ?? new ArgumentCollection();
            this.Initialize();
            this.isInitialized = true;
        }

        public ArgumentCollection GetArgs()
        {
            return this.args;
        }

        public string GetPluginName()
        {
            return Consts.WindowTitle;
        }

        public System.Drawing.Icon GetPluginIcon()
        {
            return ResourceImage.WindowIcon;
        }

        public PluginState GetPluginCurrentState()
        {
            PluginState currentState = new PluginState(this.GetPluginName());
            if(this.mainWindow != null)
            {
                currentState.PositionLeft = this.mainWindow.Left;
                currentState.PositionTop = this.mainWindow.Top;

                currentState.Height = this.mainWindow.Height;
                currentState.Width = this.mainWindow.Width;

                currentState.WindowState = this.mainWindow.WindowState;
            }

            return currentState;
        }

        public bool IsPluginInitialized()
        {
            return this.isInitialized;
        }

        public void ClosePlugin()
        {
            this.Close();
        }
        
        public string GetPluginAssemblyName()
        {
            return System.Reflection.Assembly.GetExecutingAssembly()?.GetName()?.Name;
        }
        
        public Version GetPluginAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly()?.GetName()?.Version;
        }

        public IWindowControl GetPluginControl()
        {
            return this.InitializeControl();
        }

        public IWindow GetPluginWindow()
        {
            return this.InitializeWindow();
        }

        #endregion
    }
}
