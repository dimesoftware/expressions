namespace System.Linq.Expressions
{
    public static class ExpressionChain
    {
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

        public static Expression<Func<T, bool>> Or<T>(params Expression<Func<T, bool>>[] expressions)
        {
            if (expressions == default(Expression<Func<T, bool>>[]))
                throw new ArgumentNullException(nameof(expressions));

            Expression<Func<T, bool>> expression = default;

            int maxLength = expressions.Length;
            switch (expressions)
            {
                case { Length: > 1 }:
                    {
                        for (int i = 0; i < maxLength; i += 2)
                        {
                            Expression<Func<T, bool>> newExpression = expressions[i].Or(i + i >= maxLength ? null : expressions[i + 1]);

                            expression = expression == default(Expression<Func<T, bool>>)
                                ? newExpression
                                : expression.Or(newExpression);
                        }

                        break;
                    }
                case { Length: 1 }:
                    expression = expressions.FirstOrDefault();
                    break;
            }

            return expression;
        }

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

        public static Expression<Func<T, bool>> And<T>(params Expression<Func<T, bool>>[] expressions)
        {
            if (expressions == null)
                throw new ArgumentNullException(nameof(expressions));

            if (expressions.Length <= 1)
                return expressions.ElementAt(0);

            Expression<Func<T, bool>> expression = default;

            int maxLength = expressions.Length;
            for (int i = 0; i < maxLength; i += 2)
            {
                Expression<Func<T, bool>> newExpression =
                    expressions[i].And(i + 1 >= maxLength ? null : expressions[i + 1]);

                expression = expression == default(Expression<Func<T, bool>>)
                    ? newExpression
                    : expression.And(newExpression);
            }

            return expression;
        }
    }
}