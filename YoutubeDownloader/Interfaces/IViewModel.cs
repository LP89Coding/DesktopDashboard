using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIYoutubeDownloader
{
    public interface IViewModel : IDisposable
    {
        void Initialize(params object[] parameters);
    }
}
