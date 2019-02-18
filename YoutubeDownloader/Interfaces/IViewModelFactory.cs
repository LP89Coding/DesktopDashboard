
namespace DIYoutubeDownloader
{
    public interface IViewModelFactory
    {
        T CreateViewModel<T>(params object[] parameters) where T: IViewModel, new();
    }
}
