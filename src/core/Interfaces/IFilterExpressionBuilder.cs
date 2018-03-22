using System.Collections.Generic;

namespace System.Linq.Expressions
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFilterExpressionBuilder<T>
    {
        /// <summary>
        /// Expression builder for a struct type as the property to filter on
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field">The member name of <typeparamref name="TEntity"/></param>        
        /// <param name="operation">The type of expression as a string</param>
        /// <param name="value">The value of the member</param>        
        /// <returns>An expression</returns>
        Expression<Func<T, bool>> GetExpression<TEntity>(string field, string operation, object value);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fields"></param>
        /// <param name="operation"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        Expression<Func<T, bool>> GetExpression<TEntity>(
            IDictionary<int, string> fields,
            string operation,
            object value,
            string ignoreCase);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="field"></param>
        /// <param name="complexProperty"></param>
        /// <returns></returns>
        Expression<Func<T, TKey>> CreateExpression<TEntity, TKey>(string field, string complexProperty);
    }
}