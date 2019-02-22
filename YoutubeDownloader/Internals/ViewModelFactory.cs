using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIYoutubeDownloader.Internal
{
    internal class ViewModelFactory : IViewModelFactory
    {
        public T CreateViewModel<T>(ArgumentCollection args) where T : IViewModel, new()
        {
            T item = (T)Activator.CreateInstance(typeof(T));

            item.Initialize(args);

            return item;
        }
    }
}
