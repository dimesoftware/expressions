using System;
using System.Globalization;
using System.Linq.Expressions;

namespace Dime.Expressions.Tests.Mock
{
    public class DecimalParser : IParser<decimal>, IParser<decimal?>
    {
        public DecimalParser()
        {
        }

        public DecimalParser(string culture)
        {
            Culture = culture;
        }

        public string Culture { get; set; } = "nl-BE";

        public decimal ConvertFrom(object value)
        {
            CultureInfo culture = new CultureInfo("nl-BE");
            NumberFormatInfo formatInfo = (NumberFormatInfo)culture.GetFormat(typeof(NumberFormatInfo));
            decimal.TryParse(value.ToString(), NumberStyles.Number, formatInfo, out decimal db);

            return db;
        }

        object IParser.ConvertFrom(object value)
            => ConvertFrom(value);

        decimal? IParser<decimal?>.ConvertFrom(object value)
            => ConvertFrom(value);

        public bool IsValid(object value)
        {
            if (value == null)
                return false;

            decimal parsedValue = ConvertFrom(value);
            return value.ToString() != "0" && parsedValue != 0;
        }

        
    }
}