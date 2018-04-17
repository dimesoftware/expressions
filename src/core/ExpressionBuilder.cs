using System.Collections.Generic;

namespace System.Linq.Expressions
{
    public class ExpressionBuilder : IFilterExpressionBuilder
    {
        private IDateTimeParser _dateTimeParser;
        private IDoubleParser _doubleParser;

        /// <summary>
        ///
        /// </summary>
        /// <param name="dateTimeParser"></param>
        /// <returns></returns>
        public ExpressionBuilder WithDateTimeParser(IDateTimeParser dateTimeParser)
        {
            _dateTimeParser = dateTimeParser;
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="doubleParser"></param>
        /// <returns></returns>
        public ExpressionBuilder WithDoubleParser(IDoubleParser doubleParser)
        {
            _doubleParser = doubleParser;
            return this;
        }

        /// <summary>
        /// Builds and verifies the state of the expression builder
        /// </summary>   
        /// <returns></returns>
        public ExpressionBuilder Build()
        {
            if (_doubleParser == null || _dateTimeParser == null)
                throw new ArgumentException();

            return this;
        }

        Expression<Func<T, bool>> IFilterExpressionBuilder.GetExpression<T>(string field, string operation, object value)
            => ((ExpressionConverter)this).GetExpression<T>(field, operation, value);

        Expression<Func<T, bool>> IFilterExpressionBuilder.GetExpression<T>(IDictionary<int, string> fields, string operation, object value, string ignoreCase)
            => ((ExpressionConverter)this).GetExpression<T>(fields, operation, value, ignoreCase);

        Expression<Func<T, TKey>> IFilterExpressionBuilder.CreateExpression<T, TKey>(string field, string complexProperty)
            => ((ExpressionConverter)this).CreateExpression<T, TKey>(field, complexProperty);

        public static implicit operator ExpressionConverter(ExpressionBuilder builder)
            => new ExpressionConverter(builder._dateTimeParser, builder._doubleParser);
    }
}