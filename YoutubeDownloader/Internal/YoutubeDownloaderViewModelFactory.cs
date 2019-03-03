using DIYoutubeDownloader.ViewModels;

using DesktopDashboard.Common;
using ArgumentCollection = DesktopDashboard.Common.ArgumentCollection;

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
