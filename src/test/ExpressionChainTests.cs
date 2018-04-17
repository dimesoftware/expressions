using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Utilities.Expressions.Tests
{
    /// <summary>
    /// Tests ExpressionChain class.
    /// Mockup data is inspired by The Big Lebowski.
    /// </summary>
    [TestClass]
    public class ExpressionChainTests
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        public ExpressionChainTests()
        {
        }

        #endregion Constructor

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_And_ArgumentNull_ThrowsArgumentNullException()
        {
            Expression<Func<Person, bool>>[] includes = null;
            ArgumentNullException ex = Assert.ThrowsException<ArgumentNullException>(() => ExpressionChain.And(includes));
            Assert.AreEqual(ex.Message, "Value cannot be null.\r\nParameter name: expressions");
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_And_OneArgument_ReturnsSame()
        {
            Expression<Func<Person, bool>> f1 = x => x.Id > 0;
            Expression<Func<Person, bool>> mergedExpression = ExpressionChain.And(f1);

            Assert.IsTrue(mergedExpression == f1);
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
                new Person { Id = 0, Category = "CATEGORY 1" },
                new Person { Id = 1, Category = "CATEGORY 1" },
                new Person { Id = 2, Category = "CATEGORY 1" },
                new Person { Id = 3, Category = "CATEGORY 2" },
            }.AsQueryable();

            Assert.IsTrue(mockups.Where(mergedExpression).Count() == 2);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_And_ThreeArguments_Success()
        {
            Expression<Func<Person, bool>> f1 = x => x.Id > 0;
            Expression<Func<Person, bool>> f2 = x => x.Category == "CATEGORY 1";
            Expression<Func<Person, bool>> f3 = x => x.Name == "Dude";
            Expression<Func<Person, bool>> mergedExpression = ExpressionChain.And(f1, f2, f3);

            IQueryable<Person> mockups = new List<Person>
            {
                new Person { Id = 0, Category = "CATEGORY 1", Name = "Duderino" },
                new Person { Id = 1, Category = "CATEGORY 1", Name = "Dude" },
                new Person { Id = 2, Category = "CATEGORY 1", Name = "Dude" },
                new Person { Id = 3, Category = "CATEGORY 2", Name = "Dude" },
            }.AsQueryable();

            Assert.IsTrue(mockups.Where(mergedExpression).Count() == 2);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_And_FourArguments_Success()
        {
            Expression<Func<Person, bool>> f1 = x => x.Id > 0;
            Expression<Func<Person, bool>> f2 = x => x.Category == "CATEGORY 1";
            Expression<Func<Person, bool>> f3 = x => x.Name == "Dude";
            Expression<Func<Person, bool>> f4 = x => x.City == "LA";
            Expression<Func<Person, bool>> mergedExpression = ExpressionChain.And(f1, f2, f3, f4);

            IQueryable<Person> mockups = new List<Person>
            {
                new Person { Id = 0, Category = "CATEGORY 1", Name = "Duderino", City = "LA" }, // Miss: Id <= 0
                new Person { Id = 1, Category = "CATEGORY 1", Name = "Dude", City = "LA" }, // Hit
                new Person { Id = 2, Category = "CATEGORY 1", Name = "Dude", City = "NY" }, // Miss:City != LA
                new Person { Id = 3, Category = "CATEGORY 2", Name = "Dude", City = "LA" }, // Miss: Category
            }.AsQueryable();

            Assert.IsTrue(mockups.Where(mergedExpression).Count() == 1);
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
            Expression<Func<Person, bool>> mergedExpression = ExpressionChain.And(f1, f2, f3, f4, f5);

            IQueryable<Person> mockups = new List<Person>
            {
                new Person { Id = 0, Category = "CATEGORY 1", Name = "Duderino", City = "NY", IsGolfer = true },
                new Person { Id = 1, Category = "CATEGORY 1", Name = "Dude", City = "LA", IsGolfer = true },
                new Person { Id = 2, Category = "CATEGORY 1", Name = "Dude", City = "LA", IsGolfer = true },
                new Person { Id = 3, Category = "CATEGORY 2", Name = "Dude", City = "LA", IsGolfer = true },
            }.AsQueryable();

            Assert.IsTrue(mockups.Where(mergedExpression).Count() == 2);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_Or_ArgumentNull_ThrowsArgumentNullException()
        {
            Expression<Func<Person, bool>>[] includes = null;
            ArgumentNullException ex = Assert.ThrowsException<ArgumentNullException>(() => ExpressionChain.Or(includes));
            Assert.AreEqual(ex.Message, "Value cannot be null.\r\nParameter name: expressions");
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_Or_OneArgument_ReturnsSame()
        {
            Expression<Func<Person, bool>> f1 = x => x.Id > 0;
            Expression<Func<Person, bool>> mergedExpression = ExpressionChain.Or(f1);

            Assert.IsTrue(mergedExpression == f1);
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
                new Person { Id = 0, Category = "CATEGORY 1" },
                new Person { Id = 1, Category = "CATEGORY 1" },
                new Person { Id = 2, Category = "CATEGORY 1" },
                new Person { Id = 3, Category = "CATEGORY 2" },
            }.AsQueryable();

            Assert.IsTrue(mockups.Where(mergedExpression).Count() == 4);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_Or_ThreeArguments_Success()
        {
            Expression<Func<Person, bool>> f1 = x => x.Id > 0;
            Expression<Func<Person, bool>> f2 = x => x.Category == "CATEGORY 1";
            Expression<Func<Person, bool>> f3 = x => x.Name == "Dude";
            Expression<Func<Person, bool>> mergedExpression = ExpressionChain.Or(f1, f2, f3);

            IQueryable<Person> mockups = new List<Person>
            {
                new Person { Id = 0, Category = "CATEGORY 1", Name = "Duderino" },
                new Person { Id = 1, Category = "CATEGORY 1", Name = "Dude" },
                new Person { Id = 2, Category = "CATEGORY 1", Name = "Dude" },
                new Person { Id = 3, Category = "CATEGORY 2", Name = "Dude" },
            }.AsQueryable();

            Assert.IsTrue(mockups.Where(mergedExpression).Count() == 4);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionChain_Or_FourArguments_Success()
        {
            Expression<Func<Person, bool>> f1 = x => x.Id > 0;
            Expression<Func<Person, bool>> f2 = x => x.Category == "CATEGORY 1";
            Expression<Func<Person, bool>> f3 = x => x.Name == "Dude";
            Expression<Func<Person, bool>> f4 = x => x.City == "LA";
            Expression<Func<Person, bool>> mergedExpression = ExpressionChain.Or(f1, f2, f3, f4);

            IQueryable<Person> mockups = new List<Person>
            {
                new Person { Id = 0, Category = "CATEGORY 1", Name = "Duderino", City = "NY" },
                new Person { Id = 1, Category = "CATEGORY 1", Name = "Dude", City = "LA" },
                new Person { Id = 2, Category = "CATEGORY 1", Name = "Dude", City = "LA" },
                new Person { Id = 3, Category = "CATEGORY 2", Name = "Dude", City = "LA" },
            }.AsQueryable();

            Assert.IsTrue(mockups.Where(mergedExpression).Count() == 4);
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
            Expression<Func<Person, bool>> mergedExpression = ExpressionChain.Or(f1, f2, f3, f4, f5);

            IQueryable<Person> mockups = new List<Person>
            {
                new Person { Id = 0, Category = "CATEGORY 1", Name = "Duderino", City = "NY", IsGolfer = true },
                new Person { Id = 1, Category = "CATEGORY 1", Name = "Dude", City = "LA", IsGolfer = true },
                new Person { Id = 2, Category = "CATEGORY 1", Name = "Dude", City = "LA", IsGolfer = true },
                new Person { Id = 3, Category = "CATEGORY 2", Name = "Dude", City = "LA", IsGolfer = true },
            }.AsQueryable();

            Assert.IsTrue(mockups.Where(mergedExpression).Count() == 4);
        }
    }
}