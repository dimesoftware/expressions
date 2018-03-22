using System.Collections.Generic;

namespace System.Linq.Expressions
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IOrderExpressionBuilder<T>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        Expression<Func<T, object>> GetExpression(string dataIndex);

        /// <summary>
        ///
        /// </summary>
        /// <param name="dataIndices"></param>
        /// <returns></returns>
        Expression<Func<T, object>> GetExpression(IDictionary<int, string> dataIndices);
    }
}