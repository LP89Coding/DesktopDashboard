using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using WPF.Common.Interfaces;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;

namespace DesktopDashboard.Internals
{
    internal class Plugin : IPlugin
    {
        private IPlugin instance { get; set; }
        private ArgumentCollection args { get; set; }
        private AppDomain domain { get; set; }

        public Type Type { get; private set; }
        public string Path { get; private set; }

        public Plugin(Type type, IPlugin instance, string path)
        {
            this.Type = type;
            this.instance = instance;
            this.Path = path;
        }

        #region IPlugin implementation

        public void InitializePlugin(ArgumentCollection args)
        {
            //if (this.domain == null)
            //{
            //    string appDirectory = System.IO.Path.GetDirectoryName(this.Path);
            //    //PermissionSet permSet = new PermissionSet(PermissionState.None);
            //    //permSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            //    this.domain = AppDomain.CreateDomain(this.GetPluginName(), null, new AppDomainSetup() { ApplicationBase = appDirectory });

            //}
            //domain.ExecuteAssembly(this.Path);


            this.args = args ?? new ArgumentCollection();
            this.args.Set(ArgumentCollection.ArgumentType.IsPluginMode, true);
            this.instance?.InitializePlugin(args);
        }

        public ArgumentCollection GetArgs()
        {
            return this.args;
        }

        public string GetPluginName()
        {
            return this.instance?.GetPluginName();
        }

        public Icon GetPluginIcon()
        {
            return this.instance?.GetPluginIcon();
        }

        public void ClosePlugin()
        {
            this.instance?.ClosePlugin();
        }

        public string GetPluginAssemblyName()
        {
            return System.Reflection.Assembly.GetExecutingAssembly()?.GetName()?.Name;
        }

        public Version GetPluginAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly()?.GetName()?.Version;
        }

        #endregion
    }
}
