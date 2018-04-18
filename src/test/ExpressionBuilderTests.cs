using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Dime.Expressions.Tests;
using Dime.Expressions.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Utilities.Expressions.Tests
{
    [TestClass]
    public class ExpressionBuilderTests
    {
        public ExpressionBuilder Builder { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Builder = new ExpressionBuilder();
            Builder.WithDateTimeParser(new DateTimeParser("Europe/Paris", new CultureInfo("nl-BE")));
            Builder.WithDoubleParser(new DoubleParser());
            Builder.WithDecimalParser(new DecimalParser());
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionBuilder_GetExpression_ScalarProperty_Enum_Like_BuildsAndExecutesExpression()
        {
            List<Person> persons = new List<Person>
            {
                new Person { Type = PlayerType.Bowler },
                new Person { Type = PlayerType.Bowler },
                new Person { Type = PlayerType.Golfer },
            };
            
            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)Builder).GetExpression<Person>("Type", "like", "1");

            var items = persons.Where(expr.Compile());
            Assert.IsTrue(items.Count() == 2);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionBuilder_GetExpression_ScalarProperty_Double_Like_BuildsAndExecutesExpression()
        {
            List<Person> persons = new List<Person>
            {
                new Person { Type = PlayerType.Bowler, BirthDate = new DateTime(1980,10,15), Height = 192.25},
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 185.52 },
            };

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)Builder).GetExpression<Person>("Height", "like", "185,52");

            var items = persons.Where(expr.Compile());
            Assert.IsTrue(items.Count() == 1);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionBuilder_GetExpression_ScalarProperty_Boolean_Double_Like_BuildsAndExecutesExpression()
        {
            List<Person> persons = new List<Person>
            {
                new Person { Type = PlayerType.Bowler, BirthDate = new DateTime(1980,10,15), Height = 192.25},
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 185.52 },
            };

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)Builder).GetExpression<Person>("IsGolfer", "like", "185,52");
            Assert.IsNull(expr);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionBuilder_GetExpression_ScalarProperty_Boolean_Decimal_Like_BuildsAndExecutesExpression()
        {
            List<Person> persons = new List<Person>
            {
                new Person { Type = PlayerType.Bowler, BirthDate = new DateTime(1980,10,15), Height = 192.25, Width = 100},
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 185.52, Width= 150.52M },
            };

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder();
            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)Builder).GetExpression<Person>("Width", "like", "150,52");
            var items = persons.Where(expr.Compile());
            Assert.IsTrue(items.Count() == 1);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionBuilder_GetExpression_ScalarProperty_Boolean_NullableDecimal_Like_BuildsAndExecutesExpression()
        {
            List<Person> persons = new List<Person>
            {
                new Person { Type = PlayerType.Bowler, BirthDate = new DateTime(1980,10,15), Height = 192.25, Length = 100},
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 185.52, Length= 150.52M },
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 185.52, Length= 150.52M },
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 185.52, Length= 150.53M },
            };

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder();
            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)Builder).GetExpression<Person>("Length", "like", "150,52");
            var items = persons.Where(expr.Compile());
            Assert.IsTrue(items.Count() == 2);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionBuilder_GetExpression_ScalarProperty_Double_Gt_BuildsAndExecutesExpression()
        {
            List<Person> persons = new List<Person>
            {
                new Person { Type = PlayerType.Bowler, BirthDate = new DateTime(1980,10,15), Height = 192.25},
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 185.52 },
            };

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)Builder).GetExpression<Person>("Height", "gt", "190");

            var items = persons.Where(expr.Compile());
            Assert.IsTrue(items.Count() == 1);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionBuilder_GetExpression_ScalarProperty_Double_Gte_BuildsAndExecutesExpression()
        {
            List<Person> persons = new List<Person>
            {
                new Person { Type = PlayerType.Bowler, BirthDate = new DateTime(1980,10,15), Height = 192.25},
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 185.52 },
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 175.52 },
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 175 },
            };

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)Builder).GetExpression<Person>("Height", "gte", "175,52");

            var items = persons.Where(expr.Compile());
            Assert.IsTrue(items.Count() == 3);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionBuilder_GetExpression_ScalarProperty_Double_Lt_BuildsAndExecutesExpression()
        {
            List<Person> persons = new List<Person>
            {
                new Person { Type = PlayerType.Bowler, BirthDate = new DateTime(1980,10,15), Height = 192.25},
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 185.52 },
            };

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)Builder).GetExpression<Person>("Height", "lt", "190");

            var items = persons.Where(expr.Compile());
            Assert.IsTrue(items.Count() == 1);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionBuilder_GetExpression_ScalarProperty_Double_Lte_BuildsAndExecutesExpression()
        {
            List<Person> persons = new List<Person>
            {
                new Person { Type = PlayerType.Bowler, BirthDate = new DateTime(1980,10,15), Height = 192.25},
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 190 },
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 185.52 },
            };

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)Builder).GetExpression<Person>("Height", "lte", "190");

            var items = persons.Where(expr.Compile());
            Assert.IsTrue(items.Count() == 2);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionBuilder_GetExpression_NavigationProperty_Double_Like_BuildsAndExecutesExpression()
        {
            List<Person> persons = new List<Person>
            {
                new Person { Type = PlayerType.Bowler, BirthDate = new DateTime(1980,10,15), Height = 192.25, Characteristic = new Characteristic { Height = 185.53, Name = "Goodbye cruel world"}},
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 185.52, Characteristic = new Characteristic { Height = 185.52, Name = "Hello world"} },
            };

            IDictionary<int, string> navigationProperty = new Dictionary<int, string>();
            navigationProperty.Add(new KeyValuePair<int, string>(1, "Characteristic"));
            navigationProperty.Add(new KeyValuePair<int, string>(2, "Height"));

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)Builder).GetExpression<Person>(navigationProperty, "like", "185,52", "");

            var items = persons.Where(expr.Compile());
            Assert.IsTrue(items.Count() == 1);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionBuilder_GetExpression_NavigationProperty_Boolean_Double_Like_BuildsAndExecutesExpression()
        {
            List<Person> persons = new List<Person>
            {
                new Person { Type = PlayerType.Bowler, BirthDate = new DateTime(1980,10,15), Height = 192.25, Characteristic = new Characteristic { Height = 185.53, Name = "Goodbye cruel world"}},
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 185.52, Characteristic = new Characteristic { Height = 185.52, Name = "Hello world"} },
            };

            IDictionary<int, string> navigationProperty = new Dictionary<int, string>();
            navigationProperty.Add(new KeyValuePair<int, string>(1, "Characteristic"));
            navigationProperty.Add(new KeyValuePair<int, string>(2, "IsGolfer"));

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)Builder).GetExpression<Person>(navigationProperty, "like", "185,52", "");
            Assert.IsNull(expr);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionBuilder_GetExpression_NavigationProperty_Boolean_Double_Dot_Like_BuildsAndExecutesExpression()
        {
            List<Person> persons = new List<Person>
            {
                new Person { Type = PlayerType.Bowler, BirthDate = new DateTime(1980,10,15), Height = 192.25, Characteristic = new Characteristic { Height = 185.53, Name = "Goodbye cruel world"}},
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 185.52, Characteristic = new Characteristic { Height = 185.52, Name = "Hello world"} },
            };

            IDictionary<int, string> navigationProperty = new Dictionary<int, string>();
            navigationProperty.Add(new KeyValuePair<int, string>(1, "Characteristic"));
            navigationProperty.Add(new KeyValuePair<int, string>(2, "IsGolfer"));

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)Builder).GetExpression<Person>(navigationProperty, "like", "185.52", "");
            Assert.IsNull(expr);
        }

        [DataTestMethod]
        [DataRow("IsGolfer", "like", "185.52", true, 0)]
        [DataRow("Width", "like", "185,52", false, 1)]
        [DataRow("Width", "like", "185.52", false, 0)]
        [TestCategory("Filter")]
        public void ExpressionBuilder_GetExpression_NavigationProperty_OperatorAndValueAsParameter(string property, string operation, string val, bool isNull, int resultCount)
        {
            List<Person> persons = new List<Person>
            {
                new Person { Type = PlayerType.Bowler, BirthDate = new DateTime(1980,10,15), Height = 192.25, Characteristic = new Characteristic { Height = 185.53, Width = 185.5M,Name = "Goodbye cruel world"}},
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 185.52, Characteristic = new Characteristic { Height = 185.52, Width = 185.52M, Name = "Hello world"} },
            };

            IDictionary<int, string> navigationProperty = new Dictionary<int, string>();
            navigationProperty.Add(new KeyValuePair<int, string>(1, "Characteristic"));
            navigationProperty.Add(new KeyValuePair<int, string>(2, property));

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)Builder).GetExpression<Person>(navigationProperty, operation, val, "");

            if (isNull)
                Assert.IsTrue(expr == null);
            else
            {
                var items = persons.Where(expr.Compile()).Count();
                Assert.IsTrue(items == resultCount);
            }
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionBuilder_GetExpression_NavigationProperty_Boolean_Double_Dot_Like_NLNL_BuildsAndExecutesExpression()
        {
            List<Person> persons = new List<Person>
            {
                new Person { Type = PlayerType.Bowler, BirthDate = new DateTime(1980,10,15), Height = 192.25, Characteristic = new Characteristic { Height = 185.53, Name = "Goodbye cruel world"}},
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 185.52, Characteristic = new Characteristic { Height = 185.52, Name = "Hello world"} },
            };

            IDictionary<int, string> navigationProperty = new Dictionary<int, string>();
            navigationProperty.Add(new KeyValuePair<int, string>(1, "Characteristic"));
            navigationProperty.Add(new KeyValuePair<int, string>(2, "IsGolfer"));

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)Builder).GetExpression<Person>(navigationProperty, "like", "185.52", "");
            Assert.IsTrue(persons.Count == 2);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionBuilder_GetExpression_ScalarProperty_DateTime_Like_BuildsAndExecutesExpression()
        {
            List<Person> persons = new List<Person>
            {
                new Person { Type = PlayerType.Bowler, BirthDate = new DateTime(1980,10,15), Height = 192.25, Characteristic = new Characteristic { Height = 185.53, Name = "Goodbye cruel world"}},
                new Person { Type = PlayerType.Golfer, BirthDate = new DateTime(1960,08,3), Height = 185.52, Characteristic = new Characteristic { Height = 185.52, Name = "Hello world"} },
            };

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)Builder).GetExpression<Person>("BirthDate", "like", "1960-08-03");

            var items = persons.Where(expr.Compile());
            Assert.IsTrue(items.Count() == 1);
        }
    }
}