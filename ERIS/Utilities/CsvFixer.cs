using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERIS.Utilities
{
    class CsvFixer
    {
        public static string FixRecord(string record)
        {
            foreach (Match o in new Regex(@"[ ]"".+?""[ ]").Matches(record))
            {
                record = record.Replace(o.Value, o.Value.Replace('"', '\''));
            }
            return record;
        }
    }
}
