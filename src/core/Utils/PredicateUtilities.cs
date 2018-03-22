using System.Collections.Generic;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Enables the efficient, dynamic composition of query predicates.
    /// </summary>
    internal static class PredicateUtilities
    {
        /// <summary>
        /// Creates a predicate expression from the specified lambda expression.
        /// </summary>
        internal static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate)
            => predicate;

        /// <summary>
        /// Combines the first predicate with the second using the logical "and".
        /// </summary>
        internal static Expression<Func<T, bool>> And<T>(Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
            => first.Compose(second, Expression.AndAlso);

        /// <summary>
        /// Combines the first predicate with the second using the logical "or".
        /// </summary>
        internal static Expression<Func<T, bool>> Or<T>(Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
            => first.Compose(second, Expression.OrElse);

        /// <summary>
        /// Negates the predicate.
        /// </summary>
        internal static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            UnaryExpression negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        /// <summary>
        /// Combines the first expression with the second using the specified merge function.
        /// </summary>
        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // zip parameters (map from parameters of second to parameters of first)
            Dictionary<ParameterExpression, ParameterExpression> map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with the parameters in the first
            Expression secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // create a merged lambda expression with parameters from the first expression
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        private class ParameterRebinder : ExpressionVisitor
        {
            /// <summary>
            /// 
            /// </summary>
            private readonly Dictionary<ParameterExpression, ParameterExpression> map;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="map"></param>
            private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="map"></param>
            /// <param name="exp"></param>
            /// <returns></returns>
            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
                => new ParameterRebinder(map).Visit(exp);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            protected override Expression VisitParameter(ParameterExpression p)
            {
                if (map.TryGetValue(p, out ParameterExpression replacement))
                    p = replacement;

                return base.VisitParameter(p);
            }
        }
    }
}