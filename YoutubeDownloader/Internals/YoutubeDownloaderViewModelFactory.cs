using DIYoutubeDownloader.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIYoutubeDownloader.Internal
{
    internal class YoutubeDownloaderViewModelFactory : ViewModelFactory
    {
        public YoutubeDownloaderViewModelFactory()
        {

        }

        public ArgumentCollection GetRequiredViewModelArgs()
        {
            ArgumentCollection args = new ArgumentCollection();
            
            args.Set(ArgumentCollection.ArgumentType.Downloader, new Downloader());

            return args;
        }

        public YoutubeDownloaderViewModel CreateViewModel(ArgumentCollection args = null)
        {
            ArgumentCollection requiredArgs = this.GetRequiredViewModelArgs();
            requiredArgs.Set(args);
            return base.CreateViewModel<YoutubeDownloaderViewModel>(requiredArgs);
        }
    }
}
