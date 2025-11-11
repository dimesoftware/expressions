using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Dime.Expressions.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Expressions.Tests
{
    [TestClass]
    public class ExpressionConverterTests
    {
        [TestMethod]
        public void ExpressionConverter_GetExpression_WithInvalidValue_ReturnsNull()
        {
            // Arrange
            ExpressionBuilder builder = new();
            builder.WithDateTimeParser(new DateTimeParser("Europe/Paris", new CultureInfo("en-US")));

            // Act
            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)builder)
                .GetExpression<Person>("BirthDate", "eq", "invalid-date-format");

            // Assert
            Assert.IsNull(expr);
        }

        [TestMethod]
        public void ExpressionConverter_GetExpression_WithDoesNotContain_WorksCorrectly()
        {
            // Arrange
            List<Person> persons =
            [
                new Person { Name = "Jeffrey Lebowski" },
                new Person { Name = "Walter Sobchak" },
                new Person { Name = "Donny Kerabatsos" }
            ];

            ExpressionBuilder builder = new();

            // Act
            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)builder)
                .GetExpression<Person>("Name", "doesnotcontain", "Walter");
            List<Person> result = persons.Where(expr.Compile()).ToList();

            // Assert
            Assert.HasCount(2, result);
            Assert.IsFalse(result.Any(p => p.Name.Contains("Walter")));
        }

        [TestMethod]
        public void ExpressionConverter_GetExpression_WithDoesNotStartWith_WorksCorrectly()
        {
            // Arrange
            List<Person> persons =
            [
                new Person { Name = "Jeffrey Lebowski" },
                new Person { Name = "Walter Sobchak" },
                new Person { Name = "Donny Kerabatsos" }
            ];

            ExpressionBuilder builder = new();

            // Act
            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)builder)
                .GetExpression<Person>("Name", "doesnotstartwith", "Jeffrey");
            List<Person> result = persons.Where(expr.Compile()).ToList();

            // Assert
            Assert.HasCount(2, result);
            Assert.IsFalse(result.Any(p => p.Name.StartsWith("Jeffrey")));
        }

        [TestMethod]
        public void ExpressionConverter_GetExpression_WithDoesNotEndWith_WorksCorrectly()
        {
            // Arrange
            List<Person> persons =
            [
                new Person { Name = "Jeffrey Lebowski" },
                new Person { Name = "Walter Sobchak" },
                new Person { Name = "Donny Kerabatsos" }
            ];

            ExpressionBuilder builder = new();

            // Act
            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)builder)
                .GetExpression<Person>("Name", "doesnotendwith", "Lebowski");
            List<Person> result = persons.Where(expr.Compile()).ToList();

            // Assert
            Assert.HasCount(2, result);
            Assert.IsFalse(result.Any(p => p.Name.EndsWith("Lebowski")));
        }

        [TestMethod]
        public void ExpressionConverter_GetExpression_WithNeq_WorksCorrectly()
        {
            // Arrange
            List<Person> persons =
            [
                new Person { Id = 1, Name = "Test1" },
                new Person { Id = 2, Name = "Test2" },
                new Person { Id = 3, Name = "Test3" }
            ];

            ExpressionBuilder builder = new();

            // Act
            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)builder)
                .GetExpression<Person>("Id", "neq", "2");
            List<Person> result = persons.Where(expr.Compile()).ToList();

            // Assert
            Assert.HasCount(2, result);
            Assert.IsFalse(result.Any(p => p.Id == 2));
        }

        [TestMethod]
        public void ExpressionConverter_GetExpression_WithIsSame_WorksCorrectly()
        {
            // Arrange
            List<Person> persons =
            [
                new Person { Id = 1, Name = "Test1" },
                new Person { Id = 2, Name = "Test2" }
            ];

            ExpressionBuilder builder = new();

            // Act
            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)builder)
                .GetExpression<Person>("Id", "==", "1");
            List<Person> result = persons.Where(expr.Compile()).ToList();

            // Assert
            Assert.HasCount(1, result);
            Assert.AreEqual(1, result[0].Id);
        }

        [TestMethod]
        public void ExpressionConverter_CreateExpression_WithComplexProperty_WorksCorrectly()
        {
            // Arrange
            ExpressionBuilder builder = new();

            // Act
            Expression<Func<Person, string>> expr = ((IFilterExpressionBuilder)builder)
                .CreateExpression<Person, string>("Characteristic", "Name");

            Person person = new()
            {
                Characteristic = new Characteristic { Name = "TestName" }
            };
            Func<Person, string> compiled = expr.Compile();
            string result = compiled(person);

            // Assert
            Assert.AreEqual("TestName", result);
        }

        [TestMethod]
        public void ExpressionConverter_CreateExpression_WithSimpleProperty_WorksCorrectly()
        {
            // Arrange
            ExpressionBuilder builder = new();

            // Act
            Expression<Func<Person, int>> expr = ((IFilterExpressionBuilder)builder)
                .CreateExpression<Person, int>("Id", "");

            Person person = new() { Id = 42 };
            Func<Person, int> compiled = expr.Compile();
            int result = compiled(person);

            // Assert
            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public void ExpressionConverter_GetExpression_WithInvalidOperator_ThrowsArgumentException()
        {
            // Arrange
            ExpressionBuilder builder = new();

            // Act & Assert
            try
            {
                ((IFilterExpressionBuilder)builder).GetExpression<Person>("Name", "invalidoperator", "test");
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException ex)
            {
                Assert.Contains("Could not extract enum from this value", ex.Message);
            }
        }
    }
}

