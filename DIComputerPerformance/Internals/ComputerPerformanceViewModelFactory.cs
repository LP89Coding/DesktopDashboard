using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPF.Common.Factories;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;

using DIComputerPerformance.ViewModels;

namespace DIComputerPerformance.Internals
{
    internal class ComputerPerformanceViewModelFactory : ViewModelFactory
    {
        public ComputerPerformanceViewModelFactory()
        {

        }

        public ArgumentCollection GetRequiredViewModelArgs()
        {
            ArgumentCollection args = new ArgumentCollection();

            //args.Set(ArgumentCollection.ArgumentType.Downloader, new Downloader());

            return args;
        }

        public ComputerPerformanceViewModel CreateViewModel(ArgumentCollection args = null)
        {
            ArgumentCollection requiredArgs = this.GetRequiredViewModelArgs();
            requiredArgs.Set(args);
            return base.CreateViewModel<ComputerPerformanceViewModel>(requiredArgs);
        }
    }
}
