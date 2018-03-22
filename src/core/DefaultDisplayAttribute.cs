namespace System.Linq.Expressions
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DefaultDisplayAttribute : Attribute
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        public DefaultDisplayAttribute(string name)
        {
            Name = name;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        #endregion Properties
    }
}