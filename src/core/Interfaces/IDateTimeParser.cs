namespace System.Linq.Expressions
{
    /// <summary>
    /// Custom parser for the <see cref="DateTime"/> struct
    /// </summary>
    public interface IDateTimeParser
    {
        /// <summary>
        /// Custom type converter for date time instances
        /// </summary>
        /// <param name="value">The value which needs to be converted to a DateTime object</param>
        /// <returns>A datetime instance</returns>
        DateTime Parse(object value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool CanParse(object value);
    }
}