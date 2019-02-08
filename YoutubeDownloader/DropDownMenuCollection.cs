using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIYoutubeDownloader
{
    class DropDownMenuCollection
    { 
        private List<DropDownMenuItem> dropDownItems;
        public List<DropDownMenuItem> DropDownItems
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
        public DropDownMenuCollection(List<DropDownMenuItem> dropDownItems)
        {
            this.DropDownItems = dropDownItems;
            if (this.DropDownItems == null)
                this.DropDownItems = new List<DropDownMenuItem>();
        }
}
}
