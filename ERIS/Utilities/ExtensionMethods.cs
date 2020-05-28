using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public static string removeItems(this string old, string[] toRemove)
        {
            string s = old;
            foreach (var c in toRemove)
            {
                s = s.Replace(c, string.Empty);
            }

            return s;
        }

        public static string RemovePhoneFormatting(this string s)
        {
            return Regex.Replace(s, "[^0-9+.]", string.Empty);
        }

        public static bool In(this string source, string csv)
        {
            var list = csv.Split(',');
            return list.Contains(source, StringComparer.OrdinalIgnoreCase);
        }
    }
}
