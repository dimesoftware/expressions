using System;
using System.Linq.Expressions;
using Dime.Utilities.Expressions.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Expressions.Tests
{
    [TestClass]
    public class ExpressionUtilitiesTests
    {
        [TestMethod]
        public void ExpressionUtilities_GetPropertyName_ScalarProperty_ThrowsArgumentException()
        {
            Expression<Func<Person, object>> nameExpression = (x) => x.BirthDate;     
            Assert.ThrowsException<ArgumentException>(() => nameExpression.GetPropertyName(), "Expression body must be a member expression");
        }

        [TestMethod]
        public void ExpressionUtilities_GetPropertyName_NavigationProperty_ReturnsGraph()
        {
            Expression<Func<Person, object>> nameExpression = (x) => x.Characteristic.Name;
            string propertyName = nameExpression.GetPropertyName();

            Assert.IsTrue(propertyName == "Characteristic.Name");
        }

        [TestMethod]
        public void ExpressionUtilities_GetPropertyName_NestedNavigationProperty_ReturnsGraph()
        {
            Expression<Func<Person, object>> nameExpression = (x) => x.Characteristic.Stats;
            string propertyName = nameExpression.GetPropertyName();

            Assert.IsTrue(propertyName == "Characteristic.Stats");
        }

        [TestMethod]
        public void ExpressionUtilities_GetPropertyName_NestedNavigationProperty_FetchesScalarProperty_ThrowsArgumentException()
        {
            Expression<Func<Person, object>> nameExpression = (x) => x.Characteristic.Stats.Rank;
            Assert.ThrowsException<ArgumentException>(() => nameExpression.GetPropertyName(), "Expression body must be a member expression");
        }
    }
}