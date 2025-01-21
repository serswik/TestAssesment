using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestAssesment.Helpers
{
    // Custom converter for handling DateTime parsing from strings in a format ("yyyy-MM-dd HH:mm:ss").
    // This is used to ensure that input CSV date strings are properly converted to DateTime.
    public class CustomDateTimeConverter: DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (DateTime.TryParseExact(text, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
            {
                return dateTime;
            }

            throw new FormatException($"Failed to parse {text} into DateTime.");
        }
    }
}
