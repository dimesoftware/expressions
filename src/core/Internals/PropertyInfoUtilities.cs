﻿using System.Reflection;

namespace System.Linq.Expressions.Internals
{
    internal static class PropertyInfoUtilities
    {
        /// <summary>
        /// Parse the value to its matching type if the property is an enum
        /// </summary>
        /// <param name="memberField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static object ParseValue(this MemberExpression memberField, object value)
        {
            PropertyInfo propertyInfo = memberField.Member as PropertyInfo;
            Type memberType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

            if (memberType?.IsEnum ?? false)
            {
                string valueAsString = (value ?? "0").ToString();
                return int.TryParse(valueAsString, out int enumValue) ? Enum.ToObject(memberType, enumValue) : value;
            }

            return value;
        }
    }
}