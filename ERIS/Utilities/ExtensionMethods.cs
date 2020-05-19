using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERIS.Utilities
{
    public static class ExtensionMethods
    {
        public static string GetSetting(this string setting)
        {
            return ConfigurationManager.AppSettings[setting];
        }

        public static string Prepend(this string x, string pre)
        {
            return pre + x;
        }
    }
}
