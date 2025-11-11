using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Dime.Expressions.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Expressions.Tests.Internals
{
    [TestClass]
    public class MemberExpressionUtilitiesTests
    {
        public ExpressionBuilder Builder { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Builder = new ExpressionBuilder();
            Builder.WithDateTimeParser(new DateTimeParser("Europe/Paris", new CultureInfo("nl-BE")));
            Builder.WithDoubleParser(new DoubleParser("nl-BE"));
            Builder.WithDecimalParser(new DecimalParser("nl-BE"));
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void MemberExpressionUtilities_GetExpression_FieldIsNavigationProperty_HasDefaultDisplay_TakesCategory_ShouldReturnOne()
        {
            List<Person> persons =
            [
                new Person { Characteristic = new Characteristic {Category = "Hello world"}},
                new Person { Characteristic = new Characteristic {Category = "No hello world"}},
            ];

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder();
            Expression<Func<Person, bool>> expr = expressionBuilder.GetExpression<Person>("Characteristic", "like", "Hello world");

            IEnumerable<Person> items = persons.Where(expr.Compile());
            Assert.AreEqual(1, items.Count());
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void MemberExpressionUtilities_GetExpression_FieldIsScalarPropertyInsideNavigationProperty_ShouldThrowArgumentException()
        {
            List<Person> persons =
            [
                new Person { Characteristic = new Characteristic { Category = "Hello world" } },
                new Person { Characteristic = new Characteristic { Category = "No hello world" } },
            ];

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder();
            try
            {
                expressionBuilder.GetExpression<Person>("Characteristic.Category", "like", "Hello world");
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void MemberExpressionUtilities_GetExpression_WithNavigationProperty_UsesDefaultDisplay()
        {
            // Arrange
            List<Person> persons =
            [
                new Person { Characteristic = new Characteristic { Category = "Category1" } },
                new Person { Characteristic = new Characteristic { Category = "Category2" } }
            ];

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder();

            // Act - Characteristic class has DefaultDisplay attribute pointing to Category
            Expression<Func<Person, bool>> expr = expressionBuilder.GetExpression<Person>("Characteristic", "like", "Category1");
            List<Person> result = persons.Where(expr.Compile()).ToList();

            // Assert
            Assert.HasCount(1, result);
            Assert.AreEqual("Category1", result[0].Characteristic.Category);
        }
    }
}