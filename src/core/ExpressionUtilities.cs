namespace System.Linq.Expressions
{
    /// <summary>
    /// Utilities that extend the <see cref="Expression{TDelegate}"/> class
    /// </summary>
    public static class ExpressionUtilities
    {
        /// <summary>
        /// Executes an expression on type <typeparam name="T"></typeparam> to retrieve the property graph
        /// </summary>
        /// <typeparam name="T">The type for which to retrieve a certain property</typeparam>
        /// <param name="expression">The expression to execute on itself</param>
        /// <returns>A string expressed by a dot notation that indicates the property graph</returns>
        /// <exception cref="ArgumentException">Raised when the expression targets a scalar property</exception>
        public static string GetPropertyName<T>(this Expression<Func<T, object>> expression)
        {
            if (!(expression.Body is MemberExpression memberExpr))
                throw new ArgumentException("Expression body must be a member expression");

            string[] members = memberExpr.ToString().Split('.');
            return members.Length > 2 ? string.Join(".", members.Skip(1)) : memberExpr.Member.Name;
        }
    }
}