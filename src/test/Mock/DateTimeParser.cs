using System;
using System.ComponentModel;
using System.Globalization;

namespace Dime.Expressions.Tests
{
    /// <summary>
    /// Represents a custom date time parser that takes a custom time zone (therefore ignoring any time zone indicated in the original date time instance) into account
    /// </summary>
    public class DateTimeParser : IParser<DateTime>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeParser"/> class
        /// </summary>
        /// <param name="timeZone"></param>
        /// <param name="cultureInfo"></param>
        public DateTimeParser(string timeZone, CultureInfo cultureInfo)
        {
            TimeZone = timeZone;
            CultureInfo = cultureInfo;
        }

        #endregion Constructor

        #region Properties

        private string TimeZone { get; }
        private CultureInfo CultureInfo { get; }

        #endregion Properties

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DateTime ConvertFrom(object value)
            => DateTime.Parse(value.ToString());

        object IParser.ConvertFrom(object value)
            => ConvertFrom(value);

        public bool IsValid(object value)
        {
            if (value == null)
                return false;

            DateTime.TryParse(value.ToString(), out var dt);
            return dt > DateTime.MinValue;
        }
    }
}