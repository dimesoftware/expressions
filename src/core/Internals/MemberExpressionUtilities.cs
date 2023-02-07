using System.Reflection;

namespace System.Linq.Expressions
{
    internal static class MemberExpressionUtilities
    {
        /// <summary>
        /// Inspects the property type and overrides the property field by the value specified in the default display attribute.
        /// </summary>
        /// <param name="memberField">The member field to inspect</param>
        /// <returns>The member expression</returns>
        internal static MemberExpression OverrideWithDefaultDisplay(this MemberExpression memberField)
        {
            // Check the type of the property
            PropertyInfo propertyInfo = memberField.Member as PropertyInfo;
            bool isPropertyReferenceType = propertyInfo != null
                                           && propertyInfo.PropertyType != typeof(string)
                                           && !propertyInfo.PropertyType.GetTypeInfo().IsValueType
                                           && !propertyInfo.PropertyType.GetTypeInfo().IsPrimitive;

            if (!isPropertyReferenceType)
                return memberField;

            DefaultDisplayAttribute classDefaultDisplayAttribute = propertyInfo
                .PropertyType
                .GetTypeInfo()
                .GetCustomAttribute<DefaultDisplayAttribute>();

            return classDefaultDisplayAttribute != null
                ? Expression.PropertyOrField(memberField, classDefaultDisplayAttribute.Name)
                : null;
        }

        internal static bool HasOperator(this MemberExpression memberField, string operation)
            => memberField.Type.GetMethod(operation, new[] { memberField.Type }) != null;

        internal static MethodInfo GetOperator(this MemberExpression memberField, string operation)
            => memberField.Type.GetMethod(operation, new[] { memberField.Type });
    }
}