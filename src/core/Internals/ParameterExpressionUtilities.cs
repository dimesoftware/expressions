using System.Collections.Generic;
using System.Reflection;

namespace System.Linq.Expressions.Internals
{
    /// <summary>
    /// Utilities on top of the <see cref="ParameterExpression"/> class
    /// </summary>
    internal static class ParameterExpressionUtilities
    {
        /// <summary>
        /// Creates a graph of member expressions for the property path declared with a dot notation
        /// </summary>
        /// <param name="expr">The root expression</param>
        /// <param name="dataIndices">The path to convert</param>
        /// <returns>Member expression of the last property on top of the base expression</returns>
        internal static MemberExpression ResolvePropertyPath(
            this ParameterExpression expr,
            IDictionary<int, string> dataIndices)
        {
            // Define the parent property as the root
            // In the loop the child properties will be added to the expression: (x) => x.Property, (x) => x.Property.PropertyOfProperty, etc
            Expression parentExpression = expr;

            // Loop through each node
            for (int i = 0; i < dataIndices.Count; i++)
            {
                // Get the current record in the loop
                string dataIndex = dataIndices.OrderBy(x => x.Key).ElementAt(i).Value;
                MemberExpression member = Expression.PropertyOrField(parentExpression, dataIndex);

                // Check if the last field corresponds to a complex type or a standard type (struct)
                // If needed, optionally replace the value by the default display attribute
                if (i == dataIndices.Count - 1)
                {
                    // Use some reflection to capture the class' and properties metadata
                    PropertyInfo propertyInfo = member.Member as PropertyInfo;
                    bool isPropertyReferenceType = propertyInfo != null
                                                   && propertyInfo.PropertyType != typeof(string)
                                                   && !propertyInfo.PropertyType.GetTypeInfo().IsValueType
                                                   && !propertyInfo.PropertyType.GetTypeInfo().IsPrimitive;

                    if (!isPropertyReferenceType)
                        return member;

                    DefaultDisplayAttribute classDefaultDisplayAttribute = propertyInfo
                        .PropertyType
                        .GetTypeInfo()
                        .GetCustomAttribute<DefaultDisplayAttribute>();

                    // Create a member of the member
                    if (classDefaultDisplayAttribute != null)
                        member = Expression.PropertyOrField(member, classDefaultDisplayAttribute.Name);

                    return member;
                }

                // Update reference expression
                parentExpression = member;
            }

            return null;
        }
    }
}