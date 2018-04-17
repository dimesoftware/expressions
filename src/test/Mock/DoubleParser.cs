using System;
using System.Globalization;
using System.Linq.Expressions;

namespace Dime.Expressions.Tests.Mock
{
    public class DoubleParser : IDoubleParser
    {
        public DoubleParser()
        {
        }

        public DoubleParser(string culture)
        {
            Culture = culture;
        }

        public string Culture { get; set; } = "nl-BE";

        public double Parse(object value)
        {
            CultureInfo culture = new CultureInfo("nl-BE");
            NumberFormatInfo formatInfo = (NumberFormatInfo)culture.GetFormat(typeof(NumberFormatInfo));
            double.TryParse(value.ToString(), NumberStyles.Number, formatInfo, out double db);

            return db;
        }

        public bool CanParse(object value)
        {
            if (value == null)
                return false;

            double parsedValue = Parse(value);
            return value.ToString() != "0" && parsedValue != 0;
        }
    }
}