using System.ComponentModel;

namespace System.Linq.Expressions
{
    /// <summary>
    ///
    /// </summary>
    internal enum Operators
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
}