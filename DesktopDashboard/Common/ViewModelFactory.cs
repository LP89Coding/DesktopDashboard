using System;

using DesktopDashboard.Interfaces;
using ArgumentCollection = DesktopDashboard.Common.ArgumentCollection;

namespace DesktopDashboard.Common
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
