using System.ComponentModel;
using System.Globalization;

namespace Dime.Expressions.Tests.Mock
{
    public class DoubleParser : IParser<double>
    {
        public DoubleParser()
        {
        }

        public DoubleParser(string culture)
        {
            Culture = culture;
        }

        public string Culture { get; set; } = "nl-BE";

        public double ConvertFrom(object value)
        {
            CultureInfo culture = new CultureInfo("nl-BE");
            NumberFormatInfo formatInfo = (NumberFormatInfo)culture.GetFormat(typeof(NumberFormatInfo));
            double.TryParse(value.ToString(), NumberStyles.Number, formatInfo, out double db);

            return db;
        }

        object IParser.ConvertFrom(object value)
            => ConvertFrom(value);

        public bool IsValid(object value)
        {
            if (value == null)
                return false;

            double parsedValue = ConvertFrom(value);
            return value.ToString() != "0" && parsedValue != 0;
        }
    }
}