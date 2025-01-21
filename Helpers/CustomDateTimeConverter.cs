using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace TestAssesment.Helpers
{
    // Custom converter for handling DateTime parsing from strings in a format ("yyyy-MM-dd HH:mm:ss").
    // This is used to ensure that input CSV date strings are properly converted to DateTime.
    public class CustomDateTimeConverter: DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            if (DateTime.TryParseExact(text, "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
            {
                return dateTime;
            }

            throw new FormatException($"Failed to parse {text} into DateTime.");
        }
    }
}
