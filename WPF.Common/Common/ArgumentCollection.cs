﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Common.Common
{
    public class ArgumentCollection
    {
        private readonly Dictionary<ArgumentType, object> arguments;

        public enum ArgumentType
        {
            Unknown = 0,
            WindowIcon = 1,
            WindowTitle = 2,
            Downloader = 3,
            IsPluginMode = 4,
            WindowCloseCommand = 5,
            Plugin = 6,
            DriveInfo = 7,
            ForceRefresh = 8,
            WindowHeight = 9,
            WindowWidth = 10,
            DockingManager = 11,
            PluginState = 12,
            PluginArgs = 13,
            RestorePlugin = 14,
            ParentWidth = 15,
            WindowState = 16,
        }

        public int Length { get { return this.arguments?.Count ?? 0; } }
        public ArgumentType[] Keys { get { return this.arguments?.Keys.ToArray() ?? new ArgumentType[] { }; } }

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
        public T Get<T>(ArgumentType type)
        {
            object result = null;
            arguments?.TryGetValue(type, out result);
            if (result == null && typeof(T).IsPrimitive)
                return default(T);
            return (T)result;
        }

        public void Set(ArgumentType type, object value)
        {
            this.arguments[type] = value;
        }
        public void Set(ArgumentCollection args)
        {
            if(args != null)
            {
                foreach(ArgumentType key in args.Keys)
                {
                    this.Set(key, args.Get(key));
                }
            }
        }

        public bool Contains(ArgumentType type)
        {
            return this.arguments.ContainsKey(type);
        }
    }
}
