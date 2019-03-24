﻿using System;
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
        private IWindow mainWindow { get; set; }
        private IWindowControl control { get; set; }
        private bool isPluginMode { get; set; }
        private bool isInitialized { get; set; }
        private bool isPluginWindowInitialized { get; set; }
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
                if (!this.isPluginMode)
                {
                    AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                        UnhandledException_Raised((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

                    System.Windows.Application.Current.DispatcherUnhandledException += (s, e) =>
                        UnhandledException_Raised(e.Exception, "Application.Current.DispatcherUnhandledException");

                    TaskScheduler.UnobservedTaskException += (s, e) =>
                        UnhandledException_Raised(e.Exception, "TaskScheduler.UnobservedTaskException");
                }
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
            this.control?.Dispose();
            this.control = new ucYoutubeDownloader();
            return this.control;
        }
        #endregion
        #region InitializeWindow
        private IWindow InitializeWindow()
        {
            if (!this.IsPluginWindowInitialized())
            {
                if (this.control == null)
                    this.control = this.InitializeControl();
                this.mainWindow?.Close();
                this.mainWindow = new BaseWindow(args);
                this.mainWindow.SetContent(this.control);
                this.isPluginWindowInitialized = true;
            }
            return mainWindow;
        }
        #endregion
        #region Close

        private void Close()
        {
            try
            {
                this.isInitialized = false;
                this.isPluginWindowInitialized = false;
                try
                {
                    mainWindow?.Close();
                    mainWindow = null;
                }
                catch (Exception ex)
                {
                    //ToDo Log
                }
                try
                {
                    control?.Dispose();
                    control = null;
                }
                catch (Exception ex)
                {
                    //ToDo Log
                }
                Logger.Log(EventID.Application.End);
                if (!this.isPluginMode)
                {
                    try { Logger.Close(); } catch (Exception ex) { Console.WriteLine(ex.ToString()); Logger.Log(EventID.Application.Exception, ex); }
                }
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
            this.isPluginMode = this.args.Get<bool>(ArgumentCollection.ArgumentType.IsPluginMode);
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

        public System.Drawing.Bitmap GetSmallImage()
        {
            return ResourceImage.LargeYouTube;
        }

        public System.Drawing.Bitmap GetLargeImage()
        {
            return ResourceImage.LargeYouTube;
        }

        public System.Drawing.Icon GetPluginIcon()
        {
            return ResourceImage.WindowIcon; 
        }

        public PluginState GetPluginCurrentState()
        {
            PluginState currentState = new PluginState(this.GetPluginName(), this.IsPluginInitialized());
            currentState.WindowState = this.mainWindow?.GetWindowState();

            return currentState;
        }

        public bool IsPluginInitialized()
        {
            return this.isInitialized;
        }

        public bool IsPluginWindowInitialized()
        {
            return this.isPluginWindowInitialized;
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