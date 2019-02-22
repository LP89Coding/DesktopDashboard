using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIYoutubeDownloader.Internal
{
    public class ArgumentCollection
    {
        private Dictionary<ArgumentType, object> arguments;

        public enum ArgumentType
        {
            Unknown = 0,
            WindowIcon = 1,
            WindowTitle = 2,
            Downloader = 3,
        }

        public int Length { get { return this?.arguments.Count ?? 0; } }

        public ArgumentCollection()
        {
            this.arguments = new Dictionary<ArgumentType, object>();
        }

        public object Get(ArgumentType type)
        {
            object result = null;
            arguments?.TryGetValue(type, out result);
            return result;
        }

        public void Set(ArgumentType type, object value)
        {
            this.arguments[type] = value;
        }

        public bool Contains(ArgumentType type)
        {
            return this.arguments.ContainsKey(type);
        }
    }
}
