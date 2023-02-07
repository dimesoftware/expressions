namespace System.Linq.Expressions
{
    internal static class PredicateBuilder
    {
        internal static Expression<Func<T, bool>> True<T>() => f => true;

        internal static Expression<Func<T, bool>> False<T>() => f => false;

        internal static Expression<Func<T, bool>> Or<T>(Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            InvocationExpression invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        internal static Expression<Func<T, bool>> And<T>(Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            InvocationExpression invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}