
using DIYoutubeDownloader.Internal;

namespace DIYoutubeDownloader
{
    public interface IViewModelFactory
    {
        T CreateViewModel<T>(ArgumentCollection args) where T: IViewModel, new();
    }
}
