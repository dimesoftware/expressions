using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions.Internals;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Represents a class that is capable of creating expressions
    /// </summary>
    public class ExpressionConverter
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionConverter"/> class
        /// </summary>
        /// <param name="descriptor"></param>
        internal ExpressionConverter(ParserDescriptor descriptor)
        {
            Descriptor = descriptor;
        }

        #endregion Constructor

        #region Properties

        private ParserDescriptor Descriptor { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates an expression for the <paramref name="field"/> parameter (scalar property) on type <typeparamref name="T"/>
        /// </summary>
        /// <param name="field">The path to the property on type <typeparamref name="T"/></param>
        /// <param name="operation">The operator defined as a string</param>
        /// <param name="value">The value to compare the field with</param>
        /// <returns>An expression that corresponds to the stringified query [<paramref name="field"/> - <paramref name="operation"/> - <paramref name="value"/>]</returns>
        public Expression<Func<T, bool>> GetExpression<T>(string field, string operation, object value)
        {
            // Part of the lambda: (x) =>
            ParameterExpression initialExpression = Expression.Parameter(typeof(T), "x");

            // Part of the lambda: x.Property
            MemberExpression memberField = Expression.PropertyOrField(initialExpression, field).OverrideWithDefaultDisplay();

            // Join the pieces together to a lambda expression: (x) => x.Property OPERATION Value
            return CreateExpression<T>(initialExpression, memberField, operation, value);
        }

        /// <summary>
        /// Creates an expression for the <paramref name="fields"/> parameter (navigation property) on type <typeparamref name="T"/>
        /// </summary>
        /// <param name="fields">The path to the property on type <typeparamref name="T"/></param>
        /// <param name="operation">The operator defined as a string</param>
        /// <param name="value">The value to compare the field with</param>
        /// <param name="ignoreCase"></param>
        /// <returns>An expression that corresponds to the stringified query [<paramref name="fields"/> - <paramref name="operation"/> - <paramref name="value"/>]</returns>
        /// <remarks>Same heuristics as <see cref="GetExpression{T}(string,string,object)"/> but this time for navigation properties</remarks>
        public Expression<Func<T, bool>> GetExpression<T>(
            IDictionary<int, string> fields,
            string operation,
            object value,
            string ignoreCase)
        {
            // Part of the lambda: (x) =>
            ParameterExpression initialExpression = Expression.Parameter(typeof(T), "x");

            // Part of the lambda: x.Property or x.Property.PropertyOfProperty
            MemberExpression memberField = initialExpression.ResolvePropertyPath(fields);

            // Join the pieces together to a lambda expression: (x) => x.Property OPERATION Value
            return CreateExpression<T>(initialExpression, memberField, operation, value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="field">The path to the property on type <typeparamref name="T"/></param>
        /// <param name="complexProperty"></param>
        /// <returns></returns>
        public Expression<Func<T, TKey>> CreateExpression<T, TKey>(string field, string complexProperty)
        {
            // Part of the lambda: (x) =>
            ParameterExpression initialExpression = Expression.Parameter(typeof(T), "x");

            // Part of the lambda: x.Property
            MemberExpression member = Expression.Property(initialExpression, field);
            MemberExpression expression = !string.IsNullOrEmpty(complexProperty)
                ? Expression.PropertyOrField(member, complexProperty)
                : member;

            // Join the pieces together to a lambda expression: (x) => x.Property OPERATION Value
            return Expression.Lambda<Func<T, TKey>>(expression, initialExpression);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param">The root expression</param>
        /// <param name="memberField">The expression that leads to the property</param>
        /// <param name="operation">The operator defined as a string</param>
        /// <param name="value">The value to compare the field with</param>
        /// <returns></returns>
        private Expression<Func<T, bool>> CreateExpression<T>(
            ParameterExpression param,
            MemberExpression memberField,
            string operation,
            object value)
        {
            // Get the converter and parser
            TypeConverter converter = TypeDescriptor.GetConverter(memberField.Type);
            IParser parser = Descriptor.GetParser(converter);

            // Check if the value can be converted and/or parsed
            object parsedValue = memberField.ParseValue(value);
            bool canConvert = parser?.IsValid(parsedValue) ?? converter.IsValid(parsedValue);
            if (!canConvert)
                return null;

            // Convert value to constant value
            object result = parser?.ConvertFrom(value) ?? converter.ConvertFrom(value);
            ConstantExpression constant = Expression.Constant(result);

            // Interpret the operator and convert into an expression
            Expression comparingExpression = default(Expression);
            Switcher<Operators> swatch = new Switcher<Operators>();
            Operators operatorType = operation.GetValueFromDescription<Operators>();
            swatch.Switch(operatorType,
                swatch.Case(x => x == Operators.IsSame, x => comparingExpression = Equals(memberField, constant)),
                swatch.Case(x => x == Operators.Equals, x => comparingExpression = Equals(memberField, constant)),
                swatch.Case(x => x == Operators.Eq, x => comparingExpression = Equals(memberField, constant)),
                swatch.Case(x => x == Operators.Neq, x => comparingExpression = NotEquals(memberField, constant)),
                swatch.Case(x => x == Operators.Like, x => comparingExpression = Like(memberField, constant)),
                swatch.Case(x => x == Operators.Contains, x => comparingExpression = Like(memberField, constant)),
                swatch.Case(x => x == Operators.DoesNotContain, x => comparingExpression = DoesNotContain(memberField, constant)),
                swatch.Case(x => x == Operators.StartsWith, x => comparingExpression = StartsWith(memberField, constant)),
                swatch.Case(x => x == Operators.EndsWith, x => comparingExpression = EndsWith(memberField, constant)),
                swatch.Case(x => x == Operators.DoesNotStartWith, x => comparingExpression = DoesNotStartWith(memberField, constant)),
                swatch.Case(x => x == Operators.DoesNotEndWith, x => comparingExpression = DoesNotEndWith(memberField, constant)),
                swatch.Case(x => x == Operators.Gte, x => comparingExpression = GreaterThanOrEqual(memberField, constant)),
                swatch.Case(x => x == Operators.Gt, x => comparingExpression = GreaterThan(memberField, constant)),
                swatch.Case(x => x == Operators.Lte, x => comparingExpression = LessThanOrEqual(memberField, constant)),
                swatch.Case(x => x == Operators.Lt, x => comparingExpression = LessThan(memberField, constant)),
                swatch.Case(x => x == Operators.IsNullOrEmpty, x => comparingExpression = IsNullOrEmpty(memberField)),
                swatch.Case(x => x == Operators.IsNotNullOrEmpty, x => comparingExpression = IsNotNullOrEmpty(memberField)),
                swatch.Default(x => comparingExpression = null)
            );

            // Convert expression
            return comparingExpression == null ? null : Expression.Lambda<Func<T, bool>>(comparingExpression, param);
        }

        private static Expression DoesNotContain(MemberExpression memberField, Expression constant)
            => memberField.HasOperator("Contains")
                ? Expression.Not(Expression.Call(memberField, memberField.GetOperator("Contains"), Expression.Convert(constant, memberField.Type)))
                : null;

        private static Expression Like(MemberExpression memberField, Expression constant)
            => memberField.HasOperator("Contains")
                ? Expression.Call(memberField, memberField.GetOperator("Contains"), Expression.Convert(constant, memberField.Type))
                : Equals(memberField, constant);

        private static Expression DoesNotStartWith(MemberExpression memberField, Expression constant)
            => memberField.HasOperator("StartsWith")
                ? Expression.Not(Expression.Call(memberField, memberField.GetOperator("StartsWith"), Expression.Convert(constant, memberField.Type)))
                : null;

        private static Expression DoesNotEndWith(MemberExpression memberField, Expression constant)
            => memberField.HasOperator("EndsWith")
                ? Expression.Not(Expression.Call(memberField, memberField.GetOperator("EndsWith"), Expression.Convert(constant, memberField.Type)))
                : null;

        private static Expression StartsWith(MemberExpression memberField, Expression constant)
            => memberField.HasOperator("StartsWith")
                ? Expression.Call(memberField, memberField.GetOperator("StartsWith"), Expression.Convert(constant, memberField.Type))
                : null;

        private static Expression EndsWith(MemberExpression memberField, Expression constant)
            => memberField.HasOperator("EndsWith")
                ? Expression.Call(memberField, memberField.GetOperator("EndsWith"), Expression.Convert(constant, memberField.Type))
                : null;

        private static Expression GreaterThanOrEqual(Expression memberField, Expression constant)
            => Expression.GreaterThanOrEqual(memberField, Expression.Convert(constant, memberField.Type));

        private static Expression GreaterThan(Expression memberField, Expression constant)
            => Expression.GreaterThan(memberField, Expression.Convert(constant, memberField.Type));

        private static Expression LessThan(Expression memberField, Expression constant)
            => Expression.LessThan(memberField, Expression.Convert(constant, memberField.Type));

        private static Expression LessThanOrEqual(Expression memberField, Expression constant)
            => Expression.LessThanOrEqual(memberField, Expression.Convert(constant, memberField.Type));

        private static Expression Equals(Expression memberField, Expression constant)
            => Expression.Equal(memberField, Expression.Convert(constant, memberField.Type));

        private static Expression NotEquals(Expression memberField, Expression constant)
            => Expression.NotEqual(memberField, Expression.Convert(constant, memberField.Type));

        private static Expression IsNotNullOrEmpty(MemberExpression memberField)
            => memberField.HasOperator("IsNullOrEmpty")
                ? Expression.Not(Expression.Call(typeof(string), nameof(string.IsNullOrEmpty), null, memberField))
                : null;

        private static Expression IsNullOrEmpty(MemberExpression memberField)
            => memberField.HasOperator("IsNullOrEmpty")
                ? Expression.Call(typeof(string), nameof(string.IsNullOrEmpty), null, memberField)
                : null;

        #endregion Methods
    }
}