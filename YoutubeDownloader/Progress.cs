using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIYoutubeDownloader
{
    public class Progress : IProgress<int>
    {
        public delegate void ProgressReport(int progress);

        public event ProgressReport OnProgress;

        public Progress()
        {
              
        }

        public void Report(int value)
        {
            if (this.OnProgress != null)
                OnProgress(value);
        }
    }
}
