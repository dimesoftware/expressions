using System;
using System.Linq.Expressions;
using System.Linq.Expressions.Internals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Expressions.Tests.Internals
{
    [TestClass]
    public class PropertyInfoUtilitiesTests
    {
        [TestMethod]
        public void PropertyInfoUtilities_ParseValue_WithEnum_ParsesInteger()
        {
            // Arrange
            Expression<Func<PersonWithEnum, bool>> expr = p => p.Status == PersonStatus.Active;
            BinaryExpression memberExpr = (BinaryExpression)expr.Body;
            UnaryExpression unaryExpr = memberExpr.Left as UnaryExpression;
            MemberExpression memberField = (MemberExpression)(unaryExpr?.Operand ?? memberExpr.Left);

            // Act
            object result = memberField.ParseValue("1");

            // Assert
            Assert.AreEqual(PersonStatus.Pending, result);
        }

        [TestMethod]
        public void PropertyInfoUtilities_ParseValue_WithEnum_KeepsStringValue()
        {
            // Arrange
            Expression<Func<PersonWithEnum, bool>> expr = p => p.Status == PersonStatus.Active;
            BinaryExpression memberExpr = (BinaryExpression)expr.Body;
            UnaryExpression unaryExpr = memberExpr.Left as UnaryExpression;
            MemberExpression memberField = (MemberExpression)(unaryExpr?.Operand ?? memberExpr.Left);

            // Act
            object result = memberField.ParseValue("Active");

            // Assert
            Assert.AreEqual("Active", result);
        }

        [TestMethod]
        public void PropertyInfoUtilities_ParseValue_WithNonEnum_ReturnsOriginalValue()
        {
            // Arrange
            Expression<Func<Person, bool>> expr = p => p.Name == "test";
            BinaryExpression memberExpr = (BinaryExpression)expr.Body;
            MemberExpression memberField = (MemberExpression)memberExpr.Left;
            string testValue = "TestValue";

            // Act
            object result = memberField.ParseValue(testValue);

            // Assert
            Assert.AreEqual(testValue, result);
        }

        [TestMethod]
        public void PropertyInfoUtilities_ParseValue_WithNullValue_ParsesAsZero()
        {
            // Arrange
            Expression<Func<PersonWithEnum, bool>> expr = p => p.Status == PersonStatus.Active;
            BinaryExpression memberExpr = (BinaryExpression)expr.Body;
            UnaryExpression unaryExpr = memberExpr.Left as UnaryExpression;
            MemberExpression memberField = (MemberExpression)(unaryExpr?.Operand ?? memberExpr.Left);

            // Act
            object result = memberField.ParseValue(null);

            // Assert
            Assert.AreEqual(PersonStatus.Active, result);
        }

        [TestMethod]
        public void PropertyInfoUtilities_ParseValue_WithNullableEnum_ParsesInteger()
        {
            // Arrange
            Expression<Func<PersonWithEnum, bool>> expr = p => p.OptionalStatus == PersonStatus.Active;
            BinaryExpression memberExpr = (BinaryExpression)expr.Body;
            UnaryExpression unaryExpr = memberExpr.Left as UnaryExpression;
            MemberExpression memberField = (MemberExpression)(unaryExpr?.Operand ?? memberExpr.Left);

            // Act
            object result = memberField.ParseValue("2");

            // Assert
            Assert.AreEqual(PersonStatus.Inactive, result);
        }
    }

    public class PersonWithEnum
    {
        public string Name { get; set; }
        public PersonStatus Status { get; set; }
        public PersonStatus? OptionalStatus { get; set; }
    }

    public enum PersonStatus
    {
        Active = 0,
        Pending = 1,
        Inactive = 2
    }
}
