namespace System.ComponentModel
{
    /// <summary>
    /// Parser for a type
    /// </summary>
    public interface IParser<out T> : IParser
    {
        /// <summary>
        /// Custom type converter for instances of type <typeparamref name="T"/>
        /// </summary>
        /// <param name="value">The value which needs to be converted to a DateTime object</param>
        /// <returns>An instance of <typeparamref name="T"/></returns>
        new T ConvertFrom(object value);
    }
}