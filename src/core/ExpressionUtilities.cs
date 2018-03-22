namespace System.Linq.Expressions
{
    public static partial class ExpressionUtilities
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(this Expression<Func<T, object>> expression)
        {
            if (!(expression.Body is MemberExpression memberExpr))
                throw new ArgumentException("Expression body must be a member expression");

            string[] members = memberExpr.ToString().Split('.');
            return members.Count() > 2 ? string.Join(".", members.Skip(1)) : memberExpr.Member.Name;
        }
    }
}