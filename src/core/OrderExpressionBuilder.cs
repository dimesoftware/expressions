using System.Collections.Generic;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Represents a generator of expressions
    /// </summary>
    /// <typeparam name="T">The type for which to create the filters</typeparam>
    public class ExpressionBuilder<T> : IOrderExpressionBuilder<T>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        public Expression<Func<T, object>> GetExpression(string dataIndex)
        {
            Type type = typeof(T);
            ParameterExpression parameter = Expression.Parameter(type, "x");
            Expression member = Expression.Property(parameter, dataIndex);

            Type typeIfNullable = Nullable.GetUnderlyingType(member.Type);
            if (typeIfNullable != null)
                member = Expression.Call(member, "GetValueOrDefault", Type.EmptyTypes);

            LambdaExpression currentExpression = Expression.Lambda(Expression.Convert(member, typeof(object)), parameter);
            return (Expression<Func<T, object>>)currentExpression;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dataIndices"></param>
        /// <returns></returns>
        public Expression<Func<T, object>> GetExpression(IDictionary<int, string> dataIndices)
        {
            // Get type of root object
            Type type = typeof(T);
            ParameterExpression parentParameterExpression = Expression.Parameter(type, "x");
            MemberExpression memberExpression = default(MemberExpression);

            // Loop through each node
            IOrderedEnumerable<KeyValuePair<int, string>> orderedList = dataIndices.OrderBy(x => x.Key);
            for (int i = 0; i < dataIndices.Count; i++)
            {
                // Get the current record in the loop
                string dataIndex = orderedList.ElementAt(i).Value;

                // If this is the first iteration, just set the variable - else append the expa
                memberExpression = i == 0
                    ? Expression.PropertyOrField(parentParameterExpression, dataIndex)
                    : Expression.PropertyOrField(memberExpression, dataIndex);

                // Check if the last field corresponds to a complex type or a standard type (struct)
                if (i != dataIndices.Count - 1)
                    continue;

                // Use some reflection to capture the class' and properties metadata
                PropertyInfo propertyInfo = (PropertyInfo)memberExpression.Member;

                bool isReferenceType = propertyInfo != null
                                       && !propertyInfo.PropertyType.GetTypeInfo().IsValueType
                                       && !propertyInfo.PropertyType.GetTypeInfo().IsPrimitive
                                       && propertyInfo.PropertyType != typeof(string);
                if (!isReferenceType)
                    continue;

                DefaultDisplayAttribute classDefaultDisplayAttribute =
                    propertyInfo
                        .PropertyType
                        .GetTypeInfo()
                        .GetCustomAttribute<DefaultDisplayAttribute>();

                if (classDefaultDisplayAttribute != null)
                    memberExpression = Expression.PropertyOrField(memberExpression, classDefaultDisplayAttribute.Name);
            }

            UnaryExpression sortExpression = Expression.Convert(memberExpression, typeof(object));
            return Expression.Lambda<Func<T, dynamic>>(sortExpression, parentParameterExpression);
        }
    }
}