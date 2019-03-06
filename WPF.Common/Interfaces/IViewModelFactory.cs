
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;

namespace WPF.Common.Interfaces
{
    public interface IViewModelFactory
    {
        T CreateViewModel<T>(ArgumentCollection args) where T: IViewModel, new();
    }
}
