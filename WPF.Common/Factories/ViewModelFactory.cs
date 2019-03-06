using System;

using WPF.Common.Interfaces;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;

namespace WPF.Common.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {
        public virtual T CreateViewModel<T>(ArgumentCollection args) where T : IViewModel, new()
        {
            T item = (T)Activator.CreateInstance(typeof(T));

            item.Initialize(args);

            return item;
        }
    }
}
