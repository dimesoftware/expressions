using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Represents a class that is capable of creating expressions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class ExpressionBuilder<T> : IFilterExpressionBuilder<T>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionBuilder"/> class
        /// </summary>
        public ExpressionBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionBuilder"/> class
        /// </summary>
        public ExpressionBuilder(IDateTimeParser dateTimeParser) : this()
        {
            this.DateTimeParser = dateTimeParser;
        }

        #endregion Constructor

        #region Properties

        private IDateTimeParser DateTimeParser { get; set; }

        /// <summary>
        ///
        /// </summary>
        private enum Operators
        {
            [Description("==")]
            IsSame,

            [Description("=")]
            Equals,

            [Description("eq")]
            Eq,

            [Description("neq")]
            Neq,

            [Description("like")]
            Like,

            [Description("contains")]
            Contains,

            [Description("doesnotcontain")]
            DoesNotContain,

            [Description("startswith")]
            StartsWith,

            [Description("endswith")]
            EndsWith,

            [Description("gte")]
            Gte,

            [Description("lte")]
            Lte,

            [Description("gt")]
            Gt,

            [Description("lt")]
            Lt
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Expression builder for a struct type as the property to filter on
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field">The member name of <typeparamref name="TEntity"/></param>        
        /// <param name="operation">The type of expression as a string</param>
        /// <param name="value">The value of the member</param>        
        /// <returns>An expression</returns>
        public Expression<Func<T, bool>> GetExpression<TEntity>(string field, string operation, object value)
        {
            // Get field expressions
            ParameterExpression initialExpression = Expression.Parameter(typeof(T), "x");
            MemberExpression memberField = Expression.PropertyOrField(initialExpression, field);

            PropertyInfo propertyInfo = memberField.Member as PropertyInfo;
            if (propertyInfo != null && !propertyInfo.PropertyType.GetTypeInfo().IsValueType && !propertyInfo.PropertyType.GetTypeInfo().IsPrimitive && propertyInfo.PropertyType != typeof(string))
            {
                DefaultDisplayAttribute classDefaultDisplayAttribute = propertyInfo.PropertyType.GetTypeInfo().GetCustomAttribute<DefaultDisplayAttribute>();
                if (classDefaultDisplayAttribute != null)
                    memberField = Expression.PropertyOrField(memberField, classDefaultDisplayAttribute.Name);
            }

            // Parse the value to its matching type if the property is an enum
            if (propertyInfo.PropertyType.IsEnum)
            {
                string valueAsString = (value ?? "0").ToString();

                if (int.TryParse(valueAsString, out int enumValue))
                    value = Enum.ToObject(propertyInfo.PropertyType, enumValue);
            }

            // Get property type
            Type propertyType = memberField.Type;

            // Get converter for type
            TypeConverter converter = TypeDescriptor.GetConverter(propertyType);

            return this.CreateExpression(operation, value, initialExpression, memberField, converter);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fields"></param>
        /// <param name="operation"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public Expression<Func<T, bool>> GetExpression<TEntity>(IDictionary<int, string> fields, string operation, object value, string ignoreCase)
        {
            // Get field expressions
            ParameterExpression initialExpression = Expression.Parameter(typeof(T), "x");
            IEnumerable<MemberExpression> fieldExpressions = this.GetFields(fields, initialExpression);
            MemberExpression memberField = fieldExpressions.Last();

            // Get property type
            Type propertyType = memberField.Type;

            // Get converter for type
            TypeConverter converter = TypeDescriptor.GetConverter(propertyType);

            return this.CreateExpression(operation, value, initialExpression, memberField, converter);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="complexProperty"></param>
        /// <returns></returns>
        public Expression<Func<T, TKey>> CreateExpression<TEntity, TKey>(string field, string complexProperty)
        {
            ParameterExpression initialExpression = Expression.Parameter(typeof(T), "x");
            MemberExpression member = Expression.Property(initialExpression, field);
            if (!string.IsNullOrEmpty(complexProperty))
            {
                MemberExpression memberField = Expression.PropertyOrField(member, complexProperty);
                Expression<Func<T, TKey>> lambda = Expression.Lambda<Func<T, TKey>>(memberField, initialExpression);
                return lambda;
            }
            else
            {
                Expression<Func<T, TKey>> lambda = Expression.Lambda<Func<T, TKey>>(member, initialExpression);
                return lambda;
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="value"></param>
        /// <param name="whereClause"></param>
        /// <param name="param"></param>
        /// <param name="memberField"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        private Expression<Func<T, bool>> CreateExpression(string operation, object value, ParameterExpression param, MemberExpression memberField, TypeConverter converter)
        {
            Expression<Func<T, bool>> whereClause = default(Expression<Func<T, bool>>);
            bool isValidDateTime = this.CanParseDateTime(value);

            // Convert property to type
            if (converter.IsValid(value) || isValidDateTime)
            {
                Expression comparingExpression = default(BinaryExpression);

                // Convert value to constant value
                object result = value != null ?
                    isValidDateTime && this.DateTimeParser != null ? this.DateTimeParser.Parse(value) :
                    converter.ConvertFrom(value.ToString()) : value;

                ConstantExpression constant = Expression.Constant(result);

                Operators operatorType = operation.GetValueFromDescription<Operators>();
                switch (operatorType)
                {
                    case Operators.IsSame:
                    case Operators.Equals:
                    case Operators.Eq:
                        comparingExpression = Expression.Equal(memberField, Expression.Convert(constant, memberField.Type));
                        break;

                    case Operators.Neq:
                        comparingExpression = Expression.NotEqual(memberField, Expression.Convert(constant, memberField.Type));
                        break;

                    case Operators.Like:
                    case Operators.Contains:
                        MethodInfo containsMethodInfo = memberField.Type.GetMethod("Contains", new Type[] { memberField.Type });
                        if (containsMethodInfo != null)
                            comparingExpression = Expression.Call(memberField, containsMethodInfo, Expression.Convert(constant, memberField.Type));
                        else
                            return this.CreateExpression("eq", value, param, memberField, converter);
                        break;

                    case Operators.DoesNotContain:
                        MethodInfo doesNotcontainMethodInfo = memberField.Type.GetMethod("Contains", new Type[] { memberField.Type });
                        comparingExpression = Expression.Call(memberField, doesNotcontainMethodInfo, Expression.Convert(constant, memberField.Type));
                        break;

                    case Operators.StartsWith:
                        MethodInfo startsWithMethodInfo = memberField.Type.GetMethod("StartsWith", new Type[] { memberField.Type });
                        comparingExpression = Expression.Call(memberField, startsWithMethodInfo, Expression.Convert(constant, memberField.Type));
                        break;

                    case Operators.EndsWith:
                        MethodInfo endsWithMethodInfo = memberField.Type.GetMethod("StartsWith", new Type[] { memberField.Type });
                        comparingExpression = Expression.Call(memberField, endsWithMethodInfo, Expression.Convert(constant, memberField.Type));
                        break;

                    case Operators.Gte:
                        comparingExpression = Expression.GreaterThanOrEqual(memberField, Expression.Convert(constant, memberField.Type));
                        break;

                    case Operators.Gt:
                        comparingExpression = Expression.GreaterThan(memberField, Expression.Convert(constant, memberField.Type));
                        break;

                    case Operators.Lte:
                        comparingExpression = Expression.LessThanOrEqual(memberField, Expression.Convert(constant, memberField.Type));
                        break;

                    case Operators.Lt:
                        comparingExpression = Expression.LessThan(memberField, Expression.Convert(constant, memberField.Type));
                        break;

                    default:
                        break;
                }

                // Convert expression
                Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(comparingExpression, param);
                whereClause = whereClause == default(Expression<Func<T, bool>>) ? lambda : PredicateUtilities.And(whereClause, lambda);

                return whereClause;
            }
            else
                return whereClause;
        }

        /// <summary>
        /// Due to a flaw in <see cref="TypeConverter.IsValid(object)"/> we must manually verify whether the value is a valid date time.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool CanParseDateTime(object value)
        {
            if (value == null)
                return false;

            DateTime dt = DateTime.MinValue;
            DateTime.TryParse(value.ToString(), out dt);

            return dt > DateTime.MinValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dataIndices"></param>
        /// <returns></returns>
        private IEnumerable<MemberExpression> GetFields(IDictionary<int, string> dataIndices, ParameterExpression expr)
        {
            // Define the parent property
            Expression parentExpression = expr;

            // Loop through each node
            for (int i = 0; i < dataIndices.Count(); i++)
            {
                // Get the current record in the loop
                string dataIndex = dataIndices.OrderBy(x => x.Key).ElementAt(i).Value;
                MemberExpression member = Expression.PropertyOrField(parentExpression, dataIndex);

                // Check if the last field corresponds to a complex type or a standard type (struct)
                if (i == dataIndices.Count() - 1)
                {
                    // Use some reflection to capture the class' and properties metadata
                    PropertyInfo propertyInfo = (PropertyInfo)member.Member;

                    if (propertyInfo != null && !propertyInfo.PropertyType.GetTypeInfo().IsValueType && !propertyInfo.PropertyType.GetTypeInfo().IsPrimitive && propertyInfo.PropertyType != typeof(string))
                    {
                        DefaultDisplayAttribute classDefaultDisplayAttribute = propertyInfo.PropertyType.GetTypeInfo().GetCustomAttribute<DefaultDisplayAttribute>();
                        if (classDefaultDisplayAttribute != null)
                        {
                            member = Expression.PropertyOrField(member, classDefaultDisplayAttribute.Name);
                        }
                    }
                }

                // Update reference expression
                parentExpression = member;

                yield return member;
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}