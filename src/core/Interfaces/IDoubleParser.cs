namespace System.Linq.Expressions
{
    /// <summary>
    /// Custom parser for the <see cref="double"/> struct
    /// </summary>
    public interface IDoubleParser
    {
        /// <summary>
        /// Custom type converter for double instances
        /// </summary>
        /// <param name="value">The value which needs to be converted to a Double object</param>
        /// <returns>A double instance</returns>
        double Parse(object value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool CanParse(object value);
    }
}