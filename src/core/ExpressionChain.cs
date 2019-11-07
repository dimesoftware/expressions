
namespace System.Linq.Expressions
{
    /// <summary>
    ///
    /// </summary>
    public static class ExpressionChain
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="leftExpression"></param>
        /// <param name="rightExpression"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression)
        {
            if (leftExpression != null && rightExpression != null)
                return PredicateBuilder.Or(leftExpression, rightExpression);
            if (leftExpression == null && rightExpression != null)
                return rightExpression;
            if (leftExpression != null && rightExpression == null)
                return leftExpression;

            return null;
        }

        /// <summary>
        /// Supercalifragilisticexpialidocious wrapper around LinqKit's And extension that can handle more than two expressions at the same time
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressions">The expressions to concatenate</param>
        /// <returns>One expression that concatenates the collection of expressions in the parameters</returns>
        public static Expression<Func<T, bool>> Or<T>(params Expression<Func<T, bool>>[] expressions)
        {
            if (expressions == default(Expression<Func<T, bool>>[]))
                throw new ArgumentNullException(nameof(expressions));

            Expression<Func<T, bool>> expression = default(Expression<Func<T, bool>>);

            int maxLength = expressions.Length;
            if (expressions != null && expressions.Length > 1)
            {
                for (int i = 0; i < maxLength; i += 2)
                {
                    // Take the next two records in the array - and check for the second if the array size hasn't been exceeded yet
                    Expression<Func<T, bool>> newExpression = expressions[i].Or(i + i >= maxLength ? null : expressions[i + 1]);

                    // On the first try, set the new expression to the expression variable since this variable hasn't been set yet
                    expression = expression == default(Expression<Func<T, bool>>)
                        ? newExpression
                        : expression.Or(newExpression);
                }
            }
            else if (expressions != null && expressions.Length == 1)
                expression = expressions.FirstOrDefault();

            return expression;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="leftExpression"></param>
        /// <param name="rightExpression"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression)
        {
            if (leftExpression != null && rightExpression != null)
                return PredicateBuilder.And(leftExpression, rightExpression);
            if (leftExpression == null && rightExpression != null)
                return rightExpression;
            if (leftExpression != null && rightExpression == null)
                return leftExpression;

            return null;
        }

        /// <summary>
        /// Supercalifragilisticexpialidocious wrapper around LinqKit's And extension that can handle more than two expressions at the same time
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(params Expression<Func<T, bool>>[] expressions)
        {
            if (expressions == null)
                throw new ArgumentNullException(nameof(expressions));

            if (expressions.Length <= 1)
                return expressions.ElementAt(0);

            Expression<Func<T, bool>> expression = default(Expression<Func<T, bool>>);

            // Make a count how many filters need to be concatenated
            int maxLength = expressions.Length;
            for (int i = 0; i < maxLength; i += 2)
            {
                // Take the next two records in the array - and check for the second if the array size hasn't been exceeded yet
                Expression<Func<T, bool>> newExpression =
                    expressions[i].And(i + 1 >= maxLength ? null : expressions[i + 1]);

                // On the first try, set the new expression to the expression variable since this variable hasn't been set yet
                expression = expression == default(Expression<Func<T, bool>>)
                    ? newExpression
                    : expression.And(newExpression);
            }

            return expression;
        }
    }
}