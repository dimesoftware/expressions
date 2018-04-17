using System;
using System.Globalization;
using System.Linq.Expressions;

namespace Dime.Expressions.Tests
{
    /// <summary>
    /// Represents a custom date time parser that takes a custom time zone (therefore ignoring any time zone indicated in the original date time instance) into account
    /// </summary>
    public class DateTimeParser : IDateTimeParser
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeParser"/> class
        /// </summary>
        /// <param name="timeZone"></param>
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
        public DateTime Parse(object value)
            => DateTime.Parse(value.ToString());

        public bool CanParse(object value)
        {
            if (value == null)
                return false;

            double.TryParse(value.ToString(), NumberStyles.Any, CultureInfo, out double res);
            DateTime.TryParse(value.ToString(), out var dt);

            return dt > DateTime.MinValue && value.ToString() != "0" && res != 0;
        }
    }
}