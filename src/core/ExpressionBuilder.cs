using System.Collections.Generic;

namespace System.Linq.Expressions
{
    public class ExpressionBuilder : IFilterExpressionBuilder
    {
        private ParserDescriptor Descriptor { get; set; } = new ParserDescriptor();

        /// <summary>
        ///
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public ExpressionBuilder WithDateTimeParser(IParser<DateTime> parser)
        {
            Descriptor.AddParser(parser);
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public ExpressionBuilder WithDoubleParser(IParser<double> parser)
        {
            Descriptor.AddParser(parser);
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public ExpressionBuilder WithDecimalParser(IParser<decimal> parser)
        {
            Descriptor.AddParser(parser);
            return this;
        }

        Expression<Func<T, bool>> IFilterExpressionBuilder.GetExpression<T>(string field, string operation, object value)
            => ((ExpressionConverter)this).GetExpression<T>(field, operation, value);

        Expression<Func<T, bool>> IFilterExpressionBuilder.GetExpression<T>(IDictionary<int, string> fields, string operation, object value, string ignoreCase)
            => ((ExpressionConverter)this).GetExpression<T>(fields, operation, value, ignoreCase);

        Expression<Func<T, TKey>> IFilterExpressionBuilder.CreateExpression<T, TKey>(string field, string complexProperty)
            => ((ExpressionConverter)this).CreateExpression<T, TKey>(field, complexProperty);

        public static implicit operator ExpressionConverter(ExpressionBuilder builder)
            => new ExpressionConverter(builder.Descriptor);
    }
}