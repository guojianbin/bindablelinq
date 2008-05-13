using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BindingOriented.SyncLinq.Debugging.Helpers
{
    internal static class ReflectionHelper
    {
        public static string FormatTypeName(Type type)
        {
            return type.Name;
        }
    }
}
