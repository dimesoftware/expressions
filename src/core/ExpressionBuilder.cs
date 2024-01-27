using System.Collections.Generic;
using System.ComponentModel;

namespace System.Linq.Expressions
{
    /// <summary>
    /// A fluent API builder for generating expressions
    /// </summary>
    public class ExpressionBuilder : IFilterExpressionBuilder
    {
        private ParserDescriptor Descriptor { get; } = new();

        /// <summary>
        /// Fluent API for adding a parser for the <see cref="DateTime"/> struct
        /// </summary>
        /// <param name="parser">A date time parser</param>
        /// <returns>The current instance which allows chaining method calls, therefore creating a fluent API</returns>
        public ExpressionBuilder WithDateTimeParser(IParser<DateTime> parser)
        {
            Descriptor.AddParser(parser);
            return this;
        }

        public ExpressionBuilder WithDateOnlyParser(IParser<DateOnly> parser)
        {
            Descriptor.AddParser(parser);
            return this;
        }

        /// <summary>
        /// Fluent API for adding a parser for the <see cref="double"/> struct
        /// </summary>
        /// <param name="parser">A double parser</param>
        /// <returns>The current instance which allows chaining method calls, therefore creating a fluent API</returns>
        public ExpressionBuilder WithDoubleParser(IParser<double> parser)
        {
            Descriptor.AddParser(parser);
            return this;
        }

        /// <summary>
        /// Fluent API for adding a parser for the <see cref="decimal"/> struct
        /// </summary>
        /// <param name="parser">A decimal parser</param>
        /// <returns>The current instance which allows chaining method calls, therefore creating a fluent API</returns>
        public ExpressionBuilder WithDecimalParser(IParser<decimal> parser)
        {
            Descriptor.AddParser(parser);
            return this;
        }

        /// <summary>
        /// Creates an expression for the <paramref name="field"/> parameter (scalar property) on type <typeparamref name="T"/>
        /// </summary>
        /// <param name="field">The path to the property on type <typeparamref name="T"/></param>
        /// <param name="operation">The operator defined as a string</param>
        /// <param name="value">The value to compare the field with</param>
        /// <returns>An expression that corresponds to the stringified query [<paramref name="field"/> - <paramref name="operation"/> - <paramref name="value"/>]</returns>
        Expression<Func<T, bool>> IFilterExpressionBuilder.GetExpression<T>(string field, string operation, object value)
            => ((ExpressionConverter)this).GetExpression<T>(field, operation, value);

        /// <summary>
        /// Creates an expression for the <paramref name="fields"/> parameter (navigation property) on type <typeparamref name="T"/>
        /// </summary>
        /// <param name="fields">The path to the property on type <typeparamref name="T"/></param>
        /// <param name="operation">The operator defined as a string</param>
        /// <param name="value">The value to compare the field with</param>
        /// <param name="ignoreCase"></param>
        /// <returns>An expression that corresponds to the stringified query [<paramref name="fields"/> - <paramref name="operation"/> - <paramref name="value"/>]</returns>
        /// <remarks>Same heuristics as <see cref="IFilterExpressionBuilder.GetExpression{T}(string,string,object)"/> but this time for navigation properties</remarks>
        Expression<Func<T, bool>> IFilterExpressionBuilder.GetExpression<T>(IDictionary<int, string> fields, string operation, object value, string ignoreCase)
            => ((ExpressionConverter)this).GetExpression<T>(fields, operation, value, ignoreCase);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="field"></param>
        /// <param name="complexProperty"></param>
        /// <returns></returns>
        Expression<Func<T, TKey>> IFilterExpressionBuilder.CreateExpression<T, TKey>(string field, string complexProperty)
            => ((ExpressionConverter)this).CreateExpression<T, TKey>(field, complexProperty);

        public static implicit operator ExpressionConverter(ExpressionBuilder builder)
            => new(builder.Descriptor);
    }
}