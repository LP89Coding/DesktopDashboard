using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIYoutubeDownloader
{
    class DownloadMenuCollection
    { 
        private List<DownloadMenuItem> dropDownItems;
        public List<DownloadMenuItem> DropDownItems
        {
            get
            {
                return dropDownItems;
            }
            set
            {
                dropDownItems = value;
            }
        }
        public DownloadMenuCollection(List<DownloadMenuItem> dropDownItems)
        {
            this.DropDownItems = dropDownItems;
            if (this.DropDownItems == null)
                this.DropDownItems = new List<DownloadMenuItem>();
        }
}
}
