using DIYoutubeDownloader.ViewModels;

using WPF.Common.Factories;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;

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
