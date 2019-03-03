using ArgumentCollection = DesktopDashboard.Common.ArgumentCollection;

namespace DesktopDashboard.Interfaces
{
    public interface IViewModelFactory
    {
        T CreateViewModel<T>(ArgumentCollection args) where T: IViewModel, new();
    }
}
