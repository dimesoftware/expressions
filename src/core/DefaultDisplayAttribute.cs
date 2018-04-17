namespace System.Linq.Expressions
{
    /// <summary>
    /// Attribute that creates a shortcut to a property inside that class.
    /// The presence of such an attribute safely handles the situation in which filter is defined on class level - which in many cases would not yield a (correct) result.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DefaultDisplayAttribute : Attribute
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDisplayAttribute"/> class
        /// </summary>
        /// <param name="name">The name to display</param>
        public DefaultDisplayAttribute(string name)
        {
            Name = name;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        #endregion Properties
    }
}