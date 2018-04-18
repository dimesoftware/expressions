namespace System.ComponentModel
{
    /// <summary>
    /// Parser for a type
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Validates whether the value is compatible
        /// </summary>
        /// <param name="value">The value to verify</param>
        /// <returns>True if the value is compatible</returns>
        bool IsValid(object value);

        /// <summary>
        /// Custom type converter for instances
        /// </summary>
        /// <param name="value">The value which needs to be converted to a DateTime object</param>
        object ConvertFrom(object value);
    }
}