using System.Collections.Generic;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Defines the capabilities of an expression builder that creates expressions that are capable of filtering a data set
    /// </summary>
    public interface IFilterExpressionBuilder
    {
        /// <summary>
        /// Expression builder for a struct type as the property to filter on
        /// </summary>
        /// <param name="field">The member name of <typeparamref name="T"/></param>
        /// <param name="operation">The type of expression as a string</param>
        /// <param name="value">The value of the member</param>
        /// <returns>An expression</returns>
        Expression<Func<T, bool>> GetExpression<T>(string field, string operation, object value);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="operation"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        Expression<Func<T, bool>> GetExpression<T>(
            IDictionary<int, string> fields,
            string operation,
            object value,
            string ignoreCase);

        ///  <summary>
        /// 
        ///  </summary>
        ///  <typeparam name="TKey"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        ///  <param name="complexProperty"></param>
        ///  <returns></returns>
        Expression<Func<T, TKey>> CreateExpression<T, TKey>(string field, string complexProperty);
    }
}