using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Expressions.Tests.Internals
{
    [TestClass]
    public class PredicateBuilderTests
    {
        [TestMethod]
        public void PredicateBuilder_True_ReturnsAlwaysTrueExpression()
        {
            // Arrange
            List<Person> persons =
            [
                new Person { Id = 1, Name = "Test1" },
                new Person { Id = 2, Name = "Test2" },
                new Person { Id = 3, Name = "Test3" }
            ];

            // Act
            Expression<Func<Person, bool>> trueExpression = PredicateBuilder.True<Person>();
            List<Person> result = persons.Where(trueExpression.Compile()).ToList();

            // Assert
            Assert.HasCount(3, result);
        }

        [TestMethod]
        public void PredicateBuilder_False_ReturnsAlwaysFalseExpression()
        {
            // Arrange
            List<Person> persons =
            [
                new Person { Id = 1, Name = "Test1" },
                new Person { Id = 2, Name = "Test2" },
                new Person { Id = 3, Name = "Test3" }
            ];

            // Act
            Expression<Func<Person, bool>> falseExpression = PredicateBuilder.False<Person>();
            List<Person> result = persons.Where(falseExpression.Compile()).ToList();

            // Assert
            Assert.IsEmpty(result);
        }

        [TestMethod]
        public void PredicateBuilder_Or_CombinesTwoExpressions()
        {
            // Arrange
            Expression<Func<Person, bool>> expr1 = p => p.Id == 1;
            Expression<Func<Person, bool>> expr2 = p => p.Id == 3;
            List<Person> persons =
            [
                new Person { Id = 1, Name = "Test1" },
                new Person { Id = 2, Name = "Test2" },
                new Person { Id = 3, Name = "Test3" }
            ];

            // Act
            Expression<Func<Person, bool>> combinedExpression = PredicateBuilder.Or(expr1, expr2);
            List<Person> result = persons.Where(combinedExpression.Compile()).ToList();

            // Assert
            Assert.HasCount(2, result);
            Assert.IsTrue(result.Any(p => p.Id == 1));
            Assert.IsTrue(result.Any(p => p.Id == 3));
        }

        [TestMethod]
        public void PredicateBuilder_And_CombinesTwoExpressions()
        {
            // Arrange
            Expression<Func<Person, bool>> expr1 = p => p.Id > 1;
            Expression<Func<Person, bool>> expr2 = p => p.Id < 3;
            List<Person> persons =
            [
                new Person { Id = 1, Name = "Test1" },
                new Person { Id = 2, Name = "Test2" },
                new Person { Id = 3, Name = "Test3" }
            ];

            // Act
            Expression<Func<Person, bool>> combinedExpression = PredicateBuilder.And(expr1, expr2);
            List<Person> result = persons.Where(combinedExpression.Compile()).ToList();

            // Assert
            Assert.HasCount(1, result);
            Assert.AreEqual(2, result[0].Id);
        }

        [TestMethod]
        public void PredicateBuilder_And_WithComplexConditions_WorksCorrectly()
        {
            // Arrange
            Expression<Func<Person, bool>> expr1 = p => p.Name.StartsWith("Test");
            Expression<Func<Person, bool>> expr2 = p => p.Id <= 2;
            List<Person> persons =
            [
                new Person { Id = 1, Name = "Test1" },
                new Person { Id = 2, Name = "Test2" },
                new Person { Id = 3, Name = "Test3" },
                new Person { Id = 4, Name = "Other" }
            ];

            // Act
            Expression<Func<Person, bool>> combinedExpression = PredicateBuilder.And(expr1, expr2);
            List<Person> result = persons.Where(combinedExpression.Compile()).ToList();

            // Assert
            Assert.HasCount(2, result);
            Assert.IsTrue(result.All(p => p.Id <= 2 && p.Name.StartsWith("Test")));
        }

        [TestMethod]
        public void PredicateBuilder_Or_WithComplexConditions_WorksCorrectly()
        {
            // Arrange
            Expression<Func<Person, bool>> expr1 = p => p.Name == "Test1";
            Expression<Func<Person, bool>> expr2 = p => p.Id > 3;
            List<Person> persons =
            [
                new Person { Id = 1, Name = "Test1" },
                new Person { Id = 2, Name = "Test2" },
                new Person { Id = 3, Name = "Test3" },
                new Person { Id = 4, Name = "Other" }
            ];

            // Act
            Expression<Func<Person, bool>> combinedExpression = PredicateBuilder.Or(expr1, expr2);
            List<Person> result = persons.Where(combinedExpression.Compile()).ToList();

            // Assert
            Assert.HasCount(2, result);
            Assert.IsTrue(result.Any(p => p.Name == "Test1"));
            Assert.IsTrue(result.Any(p => p.Id == 4));
        }

        [TestMethod]
        public void PredicateBuilder_ChainedOperations_WorksCorrectly()
        {
            // Arrange
            Expression<Func<Person, bool>> expr1 = p => p.Id > 0;
            Expression<Func<Person, bool>> expr2 = p => p.Id < 10;
            Expression<Func<Person, bool>> expr3 = p => p.Name != null;
            List<Person> persons =
            [
                new Person { Id = 1, Name = "Test1" },
                new Person { Id = 2, Name = null },
                new Person { Id = 3, Name = "Test3" }
            ];

            // Act
            Expression<Func<Person, bool>> combinedExpression = PredicateBuilder.And(
                PredicateBuilder.And(expr1, expr2), expr3);
            List<Person> result = persons.Where(combinedExpression.Compile()).ToList();

            // Assert
            Assert.HasCount(2, result);
            Assert.IsTrue(result.All(p => p.Name != null));
        }
    }
}

