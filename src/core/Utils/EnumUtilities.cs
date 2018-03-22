using System.ComponentModel;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Utilities that extend the capabilities of enums and their interaction with strings
    /// </summary>
    internal static class EnumUtilities
    {
        /// <summary>
        /// Maps the string value to the description of an enum
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="description">The value to compare the enum's descriptions with</param>
        /// <returns>The enum that matches the description - or the enum's default value if no match was made.</returns>        
        internal static T GetValueFromDescription<T>(this string description)
        {
            var type = typeof(T);
            if (!type.GetTypeInfo().IsEnum)
                throw new InvalidOperationException();

            foreach (FieldInfo field in type.GetFields())
            {
                DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else if (field.Name == description)
                    return (T)field.GetValue(null);
            }

            throw new ArgumentException(string.Format("Could not extract enum from this value: {0}", description), nameof(description));
        }
    }
}