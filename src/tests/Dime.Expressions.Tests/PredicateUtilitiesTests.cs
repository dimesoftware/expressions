using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Expressions.Tests
{
    /// <summary>
    /// Tests FilterBuilder class.
    /// Mockup data is inspired by The Big Lebowski.
    /// </summary>
    [TestClass]
    public class PredicateUtilitiesTests
    {
        [TestMethod]
        [TestCategory("Filter")]
        public void PredicateUtilities_And()
        {
            List<Person> persons =
            [
                new Person { Type = PlayerType.Golfer, Category = "Category 1" },
                new Person { Type = PlayerType.Golfer, Category = "Category 2" },
                new Person { Type = PlayerType.Golfer, Category = "Category 1" },
                new Person { Type = PlayerType.Bowler, Category = "Category 1" },
            ];

            Expression<Func<Person, bool>> golferQuery = x => x.Type == PlayerType.Golfer;
            Expression<Func<Person, bool>> categoryQuery = x => x.Category == "Category 1";

            IEnumerable<Person> filtered = persons.Where(PredicateUtilities.And(golferQuery, categoryQuery).Compile());

            Assert.AreEqual(2, filtered.Count());
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void PredicateUtilities_Or()
        {
            List<Person> persons =
            [
                new Person { Type = PlayerType.Golfer, Category = "Category 1" },
                new Person { Type = PlayerType.Golfer, Category = "Category 2" },
                new Person { Type = PlayerType.Golfer, Category = "Category 1" },
                new Person { Type = PlayerType.Bowler, Category = "Category 1" },
                new Person { Type = PlayerType.Golfer, Category = "Category 2" },
            ];

            Expression<Func<Person, bool>> golferQuery = x => x.Type == PlayerType.Golfer;
            Expression<Func<Person, bool>> categoryQuery = x => x.Category == "Category 2";

            IEnumerable<Person> filtered = persons.Where(PredicateUtilities.Or(golferQuery, categoryQuery).Compile());

            Assert.AreEqual(4, filtered.Count());
        }
    }
}