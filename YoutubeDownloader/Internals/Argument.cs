using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIYoutubeDownloader.Internal
{
    internal class Argument
    {
        private string type;
        public string Type { get { return this.type; } private set { this.type = value; } }
        private object value;
        public object Value { get { return this.value; } private set { this.value = value; } }

        public Argument(string type, object value)
        {
            this.Type = type;
            this.Value = value;
        }
    }
}
