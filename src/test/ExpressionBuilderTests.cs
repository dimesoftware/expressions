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
        [TestMethod]
        [TestCategory("Filter")]
        public void ExpressionBuilder_GetExpression_ScalarProperty_Enum_Like_BuildsAndExecutesExpression()
        {
            List<Person> persons = new List<Person>
            {
                new Person { Type = PlayerType.Bowler },
                new Person { Type = PlayerType.Golfer },
            };

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder()
                .WithDateTimeParser(new DateTimeParser("Europe/Paris", new CultureInfo("nl-BE")))
                .WithDoubleParser(new DoubleParser())
                .Build();

            Expression<Func<Person, bool>> expr = expressionBuilder.GetExpression<Person>("Type", "like", "1");

            var items = persons.Where(expr.Compile());
            Assert.IsTrue(items.Count() == 1);
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

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder()
                .WithDateTimeParser(new DateTimeParser("Europe/Paris", new CultureInfo("nl-BE")))
                .WithDoubleParser(new DoubleParser())
                .Build();

            Expression<Func<Person, bool>> expr = expressionBuilder.GetExpression<Person>("Height", "like", "185,52");

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

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder()
                .WithDateTimeParser(new DateTimeParser("Europe/Paris", new CultureInfo("nl-BE")))
                .WithDoubleParser(new DoubleParser())
                .Build();

            Expression<Func<Person, bool>> expr = expressionBuilder.GetExpression<Person>("IsGolfer", "like", "185,52");
            Assert.IsNull(expr);
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

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder()
                .WithDateTimeParser(new DateTimeParser("Europe/Paris", new CultureInfo("nl-BE")))
                .WithDoubleParser(new DoubleParser())
                .Build();

            Expression<Func<Person, bool>> expr = expressionBuilder.GetExpression<Person>("Height", "gt", "190");

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

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder()
                .WithDateTimeParser(new DateTimeParser("Europe/Paris", new CultureInfo("nl-BE")))
                .WithDoubleParser(new DoubleParser())
                .Build();

            Expression<Func<Person, bool>> expr = expressionBuilder.GetExpression<Person>("Height", "gte", "175,52");

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

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder()
                .WithDateTimeParser(new DateTimeParser("Europe/Paris", new CultureInfo("nl-BE")))
                .WithDoubleParser(new DoubleParser())
                .Build();

            Expression<Func<Person, bool>> expr = expressionBuilder.GetExpression<Person>("Height", "lt", "190");

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

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder()
                .WithDateTimeParser(new DateTimeParser("Europe/Paris", new CultureInfo("nl-BE")))
                .WithDoubleParser(new DoubleParser())
                .Build();

            Expression<Func<Person, bool>> expr = expressionBuilder.GetExpression<Person>("Height", "lte", "190");

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

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder()
                .WithDateTimeParser(new DateTimeParser("Europe/Paris", new CultureInfo("nl-BE")))
                .WithDoubleParser(new DoubleParser());

            Expression<Func<Person, bool>> expr = expressionBuilder.GetExpression<Person>(navigationProperty, "like", "185,52", "");

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

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder()
                .WithDateTimeParser(new DateTimeParser("Europe/Paris", new CultureInfo("nl-BE")))
                .WithDoubleParser(new DoubleParser());

            Expression<Func<Person, bool>> expr = expressionBuilder.GetExpression<Person>(navigationProperty, "like", "185,52", "");
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

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder()
                .WithDateTimeParser(new DateTimeParser("Europe/Paris", new CultureInfo("nl-BE")))
                .WithDoubleParser(new DoubleParser());

            Expression<Func<Person, bool>> expr = expressionBuilder.GetExpression<Person>(navigationProperty, "like", "185.52", "");
            Assert.IsNull(expr);
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

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder()
                .WithDateTimeParser(new DateTimeParser("Europe/Paris", new CultureInfo("nl")))
                .WithDoubleParser(new DoubleParser("nl"));

            Expression<Func<Person, bool>> expr = expressionBuilder.GetExpression<Person>(navigationProperty, "like", "185.52", "");
            Assert.IsTrue(persons.Count() == 2);
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

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder()
                .WithDateTimeParser(new DateTimeParser("Europe/Paris", new CultureInfo("nl-BE")))
                .WithDoubleParser(new DoubleParser())
                .Build();

            Expression<Func<Person, bool>> expr = expressionBuilder.GetExpression<Person>("BirthDate", "like", "1960-08-03");

            var items = persons.Where(expr.Compile());
            Assert.IsTrue(items.Count() == 1);
        }
    }
}