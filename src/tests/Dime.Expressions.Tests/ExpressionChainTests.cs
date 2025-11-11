using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Expressions.Tests
{
    /// <summary>
    /// Tests ExpressionChain class.
    /// Mockup data is inspired by The Big Lebowski.
    /// </summary>
    [TestClass]
    public class ExpressionChainTests
    {
        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_And_ArgumentNull_ThrowsArgumentNullException()
        {
            Expression<Func<Person, bool>>[] includes = null;
            try
            {
                ExpressionChain.And(includes);
                Assert.Fail("Expected ArgumentNullException was not thrown");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("Value cannot be null. (Parameter 'expressions')", ex.Message);
            }
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_And_OneArgument_ReturnsSame()
        {
            Expression<Func<Person, bool>> f1 = x => x.Id > 0;
            Expression<Func<Person, bool>> mergedExpression = ExpressionChain.And(f1);

            Assert.AreEqual(f1, mergedExpression);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_And_TwoArguments_Success()
        {
            Expression<Func<Person, bool>> f1 = x => x.Id > 0;
            Expression<Func<Person, bool>> f2 = x => x.Category == "CATEGORY 1";
            Expression<Func<Person, bool>> mergedExpression = f1.And(f2);

            IQueryable<Person> mockups = new List<Person>
            {
                new() { Id = 0, Category = "CATEGORY 1" },
                new() { Id = 1, Category = "CATEGORY 1" },
                new() { Id = 2, Category = "CATEGORY 1" },
                new() { Id = 3, Category = "CATEGORY 2" },
            }.AsQueryable();

            Assert.AreEqual(2, mockups.Where(mergedExpression).Count());
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_And_ThreeArguments_Success()
        {
            Expression<Func<Person, bool>> f1 = x => x.Id > 0;
            Expression<Func<Person, bool>> f2 = x => x.Category == "CATEGORY 1";
            Expression<Func<Person, bool>> f3 = x => x.Name == "Dude";
            Expression<Func<Person, bool>> mergedExpression = f1.And(f2).And(f3);

            IQueryable<Person> mockups = new List<Person>
            {
                new() { Id = 0, Category = "CATEGORY 1", Name = "Duderino" },
                new() { Id = 1, Category = "CATEGORY 1", Name = "Dude" },
                new() { Id = 2, Category = "CATEGORY 1", Name = "Dude" },
                new() { Id = 3, Category = "CATEGORY 2", Name = "Dude" },
            }.AsQueryable();

            Assert.AreEqual(2, mockups.Where(mergedExpression).Count());
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_And_FourArguments_Success()
        {
            Expression<Func<Person, bool>> f1 = x => x.Id > 0;
            Expression<Func<Person, bool>> f2 = x => x.Category == "CATEGORY 1";
            Expression<Func<Person, bool>> f3 = x => x.Name == "Dude";
            Expression<Func<Person, bool>> f4 = x => x.City == "LA";
            Expression<Func<Person, bool>> mergedExpression = f1.And(f2).And(f3).And(f4);

            IQueryable<Person> mockups = new List<Person>
            {
                new() { Id = 0, Category = "CATEGORY 1", Name = "Duderino", City = "LA" }, // Miss: Id <= 0
                new() { Id = 1, Category = "CATEGORY 1", Name = "Dude", City = "LA" }, // Hit
                new() { Id = 2, Category = "CATEGORY 1", Name = "Dude", City = "NY" }, // Miss:City != LA
                new() { Id = 3, Category = "CATEGORY 2", Name = "Dude", City = "LA" }, // Miss: Category
            }.AsQueryable();

            Assert.AreEqual(1, mockups.Where(mergedExpression).Count());
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_And_FiveArguments_Success()
        {
            Expression<Func<Person, bool>> f1 = x => x.Id > 0;
            Expression<Func<Person, bool>> f2 = x => x.Category == "CATEGORY 1";
            Expression<Func<Person, bool>> f3 = x => x.Name == "Dude";
            Expression<Func<Person, bool>> f4 = x => x.City == "LA";
            Expression<Func<Person, bool>> f5 = x => x.IsGolfer == true;
            Expression<Func<Person, bool>> mergedExpression = f1.And(f2).And(f3).And(f4).And(f5);

            IQueryable<Person> mockups = new List<Person>
            {
                new() { Id = 0, Category = "CATEGORY 1", Name = "Duderino", City = "NY", IsGolfer = true },
                new() { Id = 1, Category = "CATEGORY 1", Name = "Dude", City = "LA", IsGolfer = true },
                new() { Id = 2, Category = "CATEGORY 1", Name = "Dude", City = "LA", IsGolfer = true },
                new() { Id = 3, Category = "CATEGORY 2", Name = "Dude", City = "LA", IsGolfer = true },
            }.AsQueryable();

            Assert.AreEqual(2, mockups.Where(mergedExpression).Count());
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_Or_ArgumentNull_ThrowsArgumentNullException()
        {
            Expression<Func<Person, bool>>[] includes = null;
            try
            {
                ExpressionChain.Or(includes);
                Assert.Fail("Expected ArgumentNullException was not thrown");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("Value cannot be null. (Parameter 'expressions')", ex.Message);
            }
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_Or_OneArgument_ReturnsSame()
        {
            Expression<Func<Person, bool>> f1 = x => x.Id > 0;
            Expression<Func<Person, bool>> mergedExpression = ExpressionChain.Or(f1);

            Assert.AreEqual(f1, mergedExpression);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_Or_TwoArguments_Success()
        {
            Expression<Func<Person, bool>> f1 = x => x.Id > 0;
            Expression<Func<Person, bool>> f2 = x => x.Category == "CATEGORY 1";
            Expression<Func<Person, bool>> mergedExpression = f1.Or(f2);

            IQueryable<Person> mockups = new List<Person>
            {
                new() { Id = 0, Category = "CATEGORY 1" },
                new() { Id = 1, Category = "CATEGORY 1" },
                new() { Id = 2, Category = "CATEGORY 1" },
                new() { Id = 3, Category = "CATEGORY 2" },
            }.AsQueryable();

            Assert.AreEqual(4, mockups.Where(mergedExpression).Count());
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_Or_ThreeArguments_Success()
        {
            Expression<Func<Person, bool>> f1 = x => x.Id > 0;
            Expression<Func<Person, bool>> f2 = x => x.Category == "CATEGORY 1";
            Expression<Func<Person, bool>> f3 = x => x.Name == "Dude";
            Expression<Func<Person, bool>> mergedExpression = f1.Or(f2).Or(f3);

            IQueryable<Person> mockups = new List<Person>
            {
                new() { Id = 0, Category = "CATEGORY 1", Name = "Duderino" },
                new() { Id = 1, Category = "CATEGORY 1", Name = "Dude" },
                new() { Id = 2, Category = "CATEGORY 1", Name = "Dude" },
                new() { Id = 3, Category = "CATEGORY 2", Name = "Dude" },
            }.AsQueryable();

            Assert.AreEqual(4, mockups.Where(mergedExpression).Count());
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_Or_FourArguments_Success()
        {
            Expression<Func<Person, bool>> f1 = x => x.Id > 0;
            Expression<Func<Person, bool>> f2 = x => x.Category == "CATEGORY 1";
            Expression<Func<Person, bool>> f3 = x => x.Name == "Dude";
            Expression<Func<Person, bool>> f4 = x => x.City == "LA";
            Expression<Func<Person, bool>> mergedExpression = f1.Or(f2).Or(f3).Or(f4);

            IQueryable<Person> mockups = new List<Person>
            {
                new() { Id = 0, Category = "CATEGORY 1", Name = "Duderino", City = "NY" },
                new() { Id = 1, Category = "CATEGORY 1", Name = "Dude", City = "LA" },
                new() { Id = 2, Category = "CATEGORY 1", Name = "Dude", City = "LA" },
                new() { Id = 3, Category = "CATEGORY 2", Name = "Dude", City = "LA" },
            }.AsQueryable();

            Assert.AreEqual(4, mockups.Where(mergedExpression).Count());
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_Or_FiveArguments_Success()
        {
            Expression<Func<Person, bool>> f1 = x => x.Id > 0;
            Expression<Func<Person, bool>> f2 = x => x.Category == "CATEGORY 1";
            Expression<Func<Person, bool>> f3 = x => x.Name == "Dude";
            Expression<Func<Person, bool>> f4 = x => x.City == "LA";
            Expression<Func<Person, bool>> f5 = x => x.IsGolfer == true;
            Expression<Func<Person, bool>> mergedExpression = f1.Or(f2).Or(f3).Or(f4).Or(f5);

            IQueryable<Person> mockups = new List<Person>
            {
                new() { Id = 0, Category = "CATEGORY 1", Name = "Duderino", City = "NY", IsGolfer = true },
                new() { Id = 1, Category = "CATEGORY 1", Name = "Dude", City = "LA", IsGolfer = true },
                new() { Id = 2, Category = "CATEGORY 1", Name = "Dude", City = "LA", IsGolfer = true },
                new() { Id = 3, Category = "CATEGORY 2", Name = "Dude", City = "LA", IsGolfer = true },
            }.AsQueryable();

            Assert.AreEqual(4, mockups.Where(mergedExpression).Count());
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_And_NullLeftExpression_ReturnsRightExpression()
        {
            Expression<Func<Person, bool>> leftExpr = null;
            Expression<Func<Person, bool>> rightExpr = x => x.Id > 0;

            Expression<Func<Person, bool>> result = leftExpr.And(rightExpr);

            Assert.AreEqual(rightExpr, result);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_And_NullRightExpression_ReturnsLeftExpression()
        {
            Expression<Func<Person, bool>> leftExpr = x => x.Id > 0;
            Expression<Func<Person, bool>> rightExpr = null;

            Expression<Func<Person, bool>> result = leftExpr.And(rightExpr);

            Assert.AreEqual(leftExpr, result);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_And_BothNull_ReturnsNull()
        {
            Expression<Func<Person, bool>> leftExpr = null;
            Expression<Func<Person, bool>> rightExpr = null;

            Expression<Func<Person, bool>> result = leftExpr.And(rightExpr);

            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_Or_NullLeftExpression_ReturnsRightExpression()
        {
            Expression<Func<Person, bool>> leftExpr = null;
            Expression<Func<Person, bool>> rightExpr = x => x.Id > 0;

            Expression<Func<Person, bool>> result = leftExpr.Or(rightExpr);

            Assert.AreEqual(rightExpr, result);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_Or_NullRightExpression_ReturnsLeftExpression()
        {
            Expression<Func<Person, bool>> leftExpr = x => x.Id > 0;
            Expression<Func<Person, bool>> rightExpr = null;

            Expression<Func<Person, bool>> result = leftExpr.Or(rightExpr);

            Assert.AreEqual(leftExpr, result);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_Or_BothNull_ReturnsNull()
        {
            Expression<Func<Person, bool>> leftExpr = null;
            Expression<Func<Person, bool>> rightExpr = null;

            Expression<Func<Person, bool>> result = leftExpr.Or(rightExpr);

            Assert.IsNull(result);
        }
    }
}