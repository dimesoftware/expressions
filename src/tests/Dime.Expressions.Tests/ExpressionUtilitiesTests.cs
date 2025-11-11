using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Expressions.Tests
{
    [TestClass]
    public class ExpressionUtilitiesTests
    {
        [TestMethod]
        public void ExpressionUtilities_GetPropertyName_ScalarProperty_ThrowsArgumentException()
        {
            Expression<Func<Person, object>> nameExpression = x => x.BirthDate;
            try
            {
                nameExpression.GetPropertyName();
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException ex)
            {
                Assert.Contains("Expression body must be a member expression", ex.Message);
            }
        }

        [TestMethod]
        public void ExpressionUtilities_GetPropertyName_NavigationProperty_ReturnsGraph()
        {
            Expression<Func<Person, object>> nameExpression = x => x.Characteristic.Name;
            string propertyName = nameExpression.GetPropertyName();

            Assert.AreEqual("Characteristic.Name", propertyName);
        }

        [TestMethod]
        public void ExpressionUtilities_GetPropertyName_NestedNavigationProperty_ReturnsGraph()
        {
            Expression<Func<Person, object>> nameExpression = x => x.Characteristic.Stats;
            string propertyName = nameExpression.GetPropertyName();

            Assert.AreEqual("Characteristic.Stats", propertyName);
        }

        [TestMethod]
        public void ExpressionUtilities_GetPropertyName_NestedNavigationProperty_FetchesScalarProperty_ThrowsArgumentException()
        {
            Expression<Func<Person, object>> nameExpression = x => x.Characteristic.Stats.Rank;
            try
            {
                nameExpression.GetPropertyName();
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException ex)
            {
                Assert.Contains("Expression body must be a member expression", ex.Message);
            }
        }
    }
}