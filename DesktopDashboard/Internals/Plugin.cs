using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using DesktopDashboard.Interfaces;
using ArgumentCollection = DesktopDashboard.Common.ArgumentCollection;

namespace DesktopDashboard.Internals
{
    internal class Plugin : IPlugin
    {
        private IPlugin instance { get; set; }
        private ArgumentCollection args { get; set; }
       // private AppDomain domain { get; set; }

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
            this.args = args ?? new ArgumentCollection();
            this.args.Set(ArgumentCollection.ArgumentType.IsPluginMode, true);
           // if (this.domain == null)
          //  {
              //  string appDirectory = System.IO.Path.GetDirectoryName(this.Path);
                //PermissionSet permSet = new PermissionSet(PermissionState.None);
                //permSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
              //  this.domain = AppDomain.CreateDomain(this.GetPluginName(), null, new AppDomainSetup() { ApplicationBase = appDirectory });

         //   }
          //  domain.ExecuteAssembly(this.Path);
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

        #endregion
    }
}
