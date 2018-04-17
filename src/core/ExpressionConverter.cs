using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions.Internals;
using System.Reflection;

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
        internal ExpressionConverter(IDateTimeParser dateTimeParser, IDoubleParser doubleParser)
        {
            DateTimeParser = dateTimeParser;
            DoubleParser = doubleParser;
        }

        #endregion Constructor

        #region Properties

        private IDateTimeParser DateTimeParser { get; }
        private IDoubleParser DoubleParser { get; }

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

            // Now the member field is known we can determine the type of the value member
            // Part of the lambda: Value
            object parsedValue = memberField.ParseValue(value);

            // Join the pieces together to a lambda expression: (x) => x.Property OPERATION Value
            TypeConverter converter = TypeDescriptor.GetConverter(memberField.Type);
            return CreateExpression<T>(initialExpression, memberField, converter, operation, parsedValue);
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

            // Now the member field is known we can determine the type of the value member
            // Part of the lambda: Value
            object parsedValue = memberField.ParseValue(value);

            // Join the pieces together to a lambda expression: (x) => x.Property OPERATION Value
            TypeConverter converter = TypeDescriptor.GetConverter(memberField.Type);
            return CreateExpression<T>(initialExpression, memberField, converter, operation, parsedValue);
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
        /// <param name="converter">The type converter that is responsible for converting an <see cref="object"/> to the type that matches the type of the <paramref name="memberField"/></param>
        /// <param name="operation">The operator defined as a string</param>
        /// <param name="value">The value to compare the field with</param>
        /// <returns></returns>
        private Expression<Func<T, bool>> CreateExpression<T>(
            ParameterExpression param,
            MemberExpression memberField,
            TypeConverter converter,
            string operation,
            object value)
        {
            bool isValidDateTime = DateTimeParser.CanParse(value);
            bool isValidDouble = DoubleParser.CanParse(value);

            // Convert property to type           
            if (converter is DoubleConverter ? !isValidDouble : !converter.IsValid(value) && !isValidDateTime)
                return null;

            // Convert value to constant value
            object result = value != null
                ? isValidDateTime && !isValidDouble
                    ? DateTimeParser.Parse(value)
                    : converter is DoubleConverter
                        ? DoubleParser.Parse(value)
                        : converter.IsValid(value)
                            ? converter.ConvertFrom(value.ToString())
                            : null
                : null;

            ConstantExpression constant = Expression.Constant(result);

            // Interpret the operator and convert into an expression
            Operators operatorType = operation.GetValueFromDescription<Operators>();

            Expression comparingExpression = default(Expression);
            Switcher<Operators> swatch = new Switcher<Operators>();
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
                swatch.Case(x => x == Operators.Gte, x => comparingExpression = GreaterThanOrEqual(memberField, constant)),
                swatch.Case(x => x == Operators.Gt, x => comparingExpression = GreaterThan(memberField, constant)),
                swatch.Case(x => x == Operators.Lte, x => comparingExpression = LessThanOrEqual(memberField, constant)),
                swatch.Case(x => x == Operators.Lt, x => comparingExpression = LessThan(memberField, constant))
            );

            // Convert expression
            return Expression.Lambda<Func<T, bool>>(comparingExpression, param);
        }

        private static Expression DoesNotContain(MemberExpression memberField, ConstantExpression constant)
            => memberField.HasOperator("Contains")
                ? Expression.Call(memberField, memberField.GetOperator("Contains"), Expression.Convert(constant, memberField.Type))
                : null;

        private static Expression Like(MemberExpression memberField, ConstantExpression constant)
            => memberField.HasOperator("Contains")
                ? Expression.Call(memberField, memberField.GetOperator("Contains"), Expression.Convert(constant, memberField.Type))
                : Equals(memberField, constant);

        private static Expression StartsWith(MemberExpression memberField, ConstantExpression constant)
            => memberField.HasOperator("StartsWith")
                ? Expression.Call(memberField, memberField.GetOperator("StartsWith"), Expression.Convert(constant, memberField.Type))
                : null;

        private static Expression EndsWith(MemberExpression memberField, ConstantExpression constant)
            => memberField.HasOperator("EndsWith")
                ? Expression.Call(memberField, memberField.GetOperator("EndsWith"), Expression.Convert(constant, memberField.Type))
                : null;

        private static Expression GreaterThanOrEqual(MemberExpression memberField, ConstantExpression constant)
            => Expression.GreaterThanOrEqual(memberField, Expression.Convert(constant, memberField.Type));

        private static Expression GreaterThan(MemberExpression memberField, ConstantExpression constant)
            => Expression.GreaterThan(memberField, Expression.Convert(constant, memberField.Type));

        private static Expression LessThan(MemberExpression memberField, ConstantExpression constant)
            => Expression.LessThan(memberField, Expression.Convert(constant, memberField.Type));

        private static Expression LessThanOrEqual(MemberExpression memberField, ConstantExpression constant)
            => Expression.LessThanOrEqual(memberField, Expression.Convert(constant, memberField.Type));

        private static Expression Equals(MemberExpression memberField, ConstantExpression constant)
            => Expression.Equal(memberField, Expression.Convert(constant, memberField.Type));

        private static Expression NotEquals(MemberExpression memberField, ConstantExpression constant)
            => Expression.NotEqual(memberField, Expression.Convert(constant, memberField.Type));

        #endregion Methods
    }
}