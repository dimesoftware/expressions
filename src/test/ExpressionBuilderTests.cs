using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Utilities.Expressions.Tests
{
    [TestClass]
    public class ExpressionBuilderTests
    {
        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionBuilder()
        {
            List<Person> persons = new List<Person>()
            {
                new Person() { Type = PlayerType.Bowler },
                new Person() { Type = PlayerType.Golfer },
            };

            ExpressionBuilder<Person> expressionBuilder = new System.Linq.Expressions.ExpressionBuilder<Person>();
            Expression<Func<Person, bool>> expr = expressionBuilder.GetExpression<Person>("Type", "like", "1");

            var items = persons.Where(expr.Compile());
            Assert.IsTrue(items.Count() == 1);
        }
    }
}