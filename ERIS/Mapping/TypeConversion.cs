using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Text.RegularExpressions;

namespace MERS.Mapping
{
    internal sealed class DateConverter : DateTimeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            DateTime dt;

            if (DateTime.TryParse(text, out dt))
                return dt;
            else
                return null;
        }
    }

    /// <summary>
    /// Need to change
    /// </summary>
    internal sealed class RegionConverter : StringConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            switch (text.ToLower())
            {
                case "0":
                    return "CO";

                case "a":
                    return "10";

                case "w":
                    return "NCR";

                default:
                    return text.PadLeft(2, '0');
            }
        }
    }

    /// <summary>
    /// If first letter equals O return A, if W return P, otherwise return first letter (might need to change this)
    /// </summary>
    internal sealed class MajorOrgConverter : StringConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            string officeSymbol = string.Empty;

            officeSymbol = Regex.Match(text, "[A-Za-z]").Value;

            if (officeSymbol.ToLower().Equals("o").ToString().Length == 1)
                return officeSymbol;

            switch (officeSymbol.ToLower())
            {
                case "o":
                    return "A";

                case "w":
                    return "P";

                default:
                    return officeSymbol;
            }
        }
    }
}
