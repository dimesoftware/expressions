using System;
using System.ComponentModel;
using System.Globalization;

namespace Dime.Expressions.Tests
{
    /// <summary>
    /// Represents a custom date time parser that takes a custom time zone (therefore ignoring any time zone indicated in the original date time instance) into account
    /// </summary>
    public class DateOnlyParser : IParser<DateOnly>, IParser<DateOnly?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateOnlyParser"/> class
        /// </summary>
        /// <param name="timeZone">The time zone</param>
        /// <param name="culture"></param>
        public DateOnlyParser(string timeZone, CultureInfo culture)
        {
            TimeZone = timeZone;
            Culture = culture;
        }

        private string TimeZone { get; }
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// Validates whether the value is compatible
        /// </summary>
        /// <param name="value">The value to verify</param>
        /// <returns>True if the value is compatible</returns>
        public bool IsValid(object value)
        {
            if (value == null)
                return false;

            DateTime.TryParse(value.ToString(), Culture, DateTimeStyles.None, out DateTime dt);
            return dt > DateTime.MinValue;
        }

        /// <summary>
        /// Custom type converter for instances of type <see cref="DateTime"/>
        /// </summary>
        /// <param name="value">The value which needs to be converted to a <see cref="DateTime"/> instance</param>
        /// <returns>An instance of <see cref="DateTime"/></returns>
        DateOnly? IParser<DateOnly?>.ConvertFrom(object value)
            => ConvertFrom(value);

        /// <summary>
        /// Custom type converter for instances of type <see cref="DateTime"/>
        /// </summary>
        /// <param name="value">The value which needs to be converted to a <see cref="DateTime"/> instance</param>
        /// <returns>An instance of <see cref="DateTime"/></returns>
        object IParser.ConvertFrom(object value)
            => ConvertFrom(value);

        /// <summary>
        /// Custom type converter for instances of type <see cref="DateTime"/>
        /// </summary>
        /// <param name="value">The value which needs to be converted to a <see cref="DateTime"/> instance</param>
        /// <returns>An instance of <see cref="DateTime"/></returns>
        public DateOnly ConvertFrom(object value)
            => DateOnly.FromDateTime(DateTime.Parse(value.ToString(), Culture));
    }
}