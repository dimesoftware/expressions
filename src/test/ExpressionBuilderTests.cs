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
        [DataTestMethod]

        // ENUM Tests
        [DataRow("nl-BE", "Europe/Paris", "Type", "like", "1", 2, false)]
        [DataRow("en-US", "Europe/Paris", "Type", "like", "1", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Type", "like", "0", 1, false)]
        [DataRow("en-US", "Europe/Paris", "Type", "like", "0", 1, false)]

        // STRING Tests
        [DataRow("nl-BE", "Europe/Paris", "Name", "like", "Jeffrey Lebowski", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Name", "like", "Jeffrey", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Name", "like", "Walter Sobchak", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "Name", "like", "Sobchak", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "Name", "like", "Theodore Donald 'Donny' Kerabatsos", 0, false)]

        // BOOLEAN tests
        [DataRow("nl-BE", "Europe/Paris", "IsGolfer", "like", "true", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "IsGolfer", "like", "false", 2, false)]

        // NULLABLE BOOLEAN tests
        [DataRow("nl-BE", "Europe/Paris", "IsPederast", "like", "true", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "IsPederast", "like", "false", 2, false)]

        // DOUBLE tests
        [DataRow("nl-BE", "Europe/Paris", "Height", "like", "185,25", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "like", "185.25", 0, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "like", "185,25", 0, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "like", "185.25", 2, false)]
        [DataRow("sv-SE", "Europe/Paris", "Height", "like", "185.25", 0, true)]
        [DataRow("sv-SE", "Europe/Paris", "Height", "like", "185,25", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "gt", "185,25", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "gte", "185,25", 3, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "gt", "185,25", 0, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "gte", "185,25", 0, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "gt", "185.25", 1, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "gte", "185.25", 3, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "gt", "185.25", 0, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "gte", "185.25", 0, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "lt", "190,25", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "lte", "185,25", 2, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "lt", "190,25", 3, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "lte", "185,25", 3, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "lt", "190.25", 2, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "lte", "185.25", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "lt", "190.25", 3, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "lte", "185.25", 3, false)]

        // DECIMAL tests
        [DataRow("nl-BE", "Europe/Paris", "Width", "like", "185,25", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Width", "like", "185.25", 0, false)]
        [DataRow("en-US", "Europe/Paris", "Width", "like", "185,25", 0, false)]
        [DataRow("en-US", "Europe/Paris", "Width", "like", "185.25", 2, false)]
        [DataRow("sv-SE", "Europe/Paris", "Width", "like", "185.25", 0, true)]
        [DataRow("sv-SE", "Europe/Paris", "Width", "like", "185,25", 2, false)]

        // NULLABLE DECIMAL tests
        [DataRow("nl-BE", "Europe/Paris", "Length", "like", "185,25", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Length", "like", "185.25", 0, false)]
        [DataRow("en-US", "Europe/Paris", "Length", "like", "185,25", 0, false)]
        [DataRow("en-US", "Europe/Paris", "Length", "like", "185.25", 2, false)]
        [DataRow("sv-SE", "Europe/Paris", "Length", "like", "185.25", 0, true)]
        [DataRow("sv-SE", "Europe/Paris", "Length", "like", "185,25", 2, false)]

        // DATETIME tests
        [DataRow("nl-BE", "Europe/Paris", "BirthDate", "like", "4/12/1942", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "BirthDate", "like", "4/12/1942 18:05:02", 1, false)]
        [DataRow("en-US", "Europe/Paris", "BirthDate", "like", "4/12/1942", 0, false)]
        [DataRow("en-US", "Europe/Paris", "BirthDate", "like", "4/12/1942 18:05:02", 0, false)]
        [DataRow("en-US", "Europe/Paris", "BirthDate", "like", "12-4-1942", 1, false)]
        [DataRow("en-US", "Europe/Paris", "BirthDate", "like", "12-4-1942 18:05:02", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "BirthDate", "like", "12-4-1942", 0, false)]
        [DataRow("nl-BE", "Europe/Paris", "BirthDate", "like", "12-4-1942 18:05:02", 0, false)]

        // NULLABLE DATETIME tests
        [DataRow("nl-BE", "Europe/Paris", "JoinedNam", "like", "4/12/2000", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "JoinedNam", "like", "4/12/2000 18:05:02", 1, false)]
        [DataRow("en-US", "Europe/Paris", "JoinedNam", "like", "4/12/2000", 0, false)]
        [DataRow("en-US", "Europe/Paris", "JoinedNam", "like", "4/12/2000 18:05:02", 0, false)]
        [DataRow("en-US", "Europe/Paris", "JoinedNam", "like", "12-4-2000", 1, false)]
        [DataRow("en-US", "Europe/Paris", "JoinedNam", "like", "12-4-2000 18:05:02", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "JoinedNam", "like", "12-4-2000", 0, false)]
        [DataRow("nl-BE", "Europe/Paris", "JoinedNam", "like", "12-4-2000 18:05:02", 0, false)]
        public void ExpressionBuilder_GetExpression_ScalarProperty(
            string culture,
            string timezone,
            string property,
            string operation,
            string value,
            int expectedCount,
            bool? generatesNull = false)
        {
            List<Person> persons = new List<Person>
            {
                new Person
                {
                    Type = PlayerType.Bowler,
                    IsGolfer = false,
                    Name = "Jeffrey Lebowski",
                    Height = 185.25,
                    Width = 185.25M,
                    Length = 185.25M,
                    BirthDate = new DateTime(1942, 12,4),
                    JoinedNam = new DateTime(2000, 12,4),
                    IsPederast = false
                },
                new Person
                {
                    Type = PlayerType.Bowler,
                    IsGolfer = false,
                    Name = "Jeffrey Lebowski",
                    Height = 185.25,
                    Width = 185.25M,
                    Length =  185.25M,
                    BirthDate = new DateTime(1942, 12,4, 18,5,2),
                    JoinedNam = new DateTime(2000, 12,4, 18,5,2),
                    IsPederast = false
                },
                new Person
                {
                    Type = PlayerType.Golfer,
                    IsGolfer = true,
                    Name = "Walter Sobchak",
                    Height = 193.64,
                    Width = 193.64M,
                    Length = 193.64M,
                    BirthDate = new DateTime(1940,1,5),
                    JoinedNam = new DateTime(2000,1,5),
                    IsPederast = true
                }
            };

            ExpressionBuilder builder = new ExpressionBuilder();
            builder.WithDateTimeParser(new DateTimeParser(timezone, new CultureInfo(culture)));
            builder.WithDoubleParser(new DoubleParser(culture));
            builder.WithDecimalParser(new DecimalParser(culture));

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)builder).GetExpression<Person>(property, operation, value);

            if (generatesNull == true)
            {
                Assert.IsNull(expr);
                return;
            }

            ICollection<Person> items = persons.Where(expr.Compile()).ToList();
            Assert.IsTrue(items.Count == expectedCount);
        }

        [DataTestMethod]
        [DataRow("nl-BE", "Europe/Paris", "Characteristic", "like", "Well, You Know, That's Just, Like, Your Opinion, Man", 2, false)]
        public void ExpressionBuilder_GetExpression_NavigationProperty_Root_UseDefaultDisplayAttribute(
            string culture,
            string timezone,
            string property,
            string operation,
            string value,
            int expectedCount,
            bool? generatesNull = false)
        {
            List<Person> persons = new List<Person>
            {
                new Person
                {
                    Type = PlayerType.Bowler,
                    IsGolfer = false,
                    Name = "Jeffrey Lebowski",
                    Height = 185.25,
                    Width = 185.25M,
                    Length = 185.25M,
                    BirthDate = new DateTime(1942, 12,4),
                    JoinedNam = new DateTime(2000, 12,4),
                    Characteristic =  new Characteristic()
                    {
                        Category = "Well, You Know, That's Just, Like, Your Opinion, Man"
                    }
                },
                new Person
                {
                    Type = PlayerType.Bowler,
                    IsGolfer = false,
                    Name = "Jeffrey Lebowski",
                    Height = 185.25,
                    Width = 185.25M,
                    Length =  185.25M,
                    BirthDate = new DateTime(1942, 12,4, 18,5,2),
                    JoinedNam = new DateTime(2000, 12,4, 18,5,2),
                    Characteristic =  new Characteristic()
                    {
                        Category = "Well, You Know, That's Just, Like, Your Opinion, Man"
                    }
                },
                new Person
                {
                    Type = PlayerType.Golfer,
                    IsGolfer = true,
                    Name = "Walter Sobchak",
                    Height = 193.64,
                    Width = 193.64M,
                    Length = 193.64M,
                    BirthDate = new DateTime(1940,1,5),
                    JoinedNam = new DateTime(2000,1,5),
                    Characteristic =  new Characteristic()
                    {
                        Category = "Smokey, this is not \'Nam. This is bowling. There are rules."
                    }
                }
            };

            ExpressionBuilder builder = new ExpressionBuilder();
            builder.WithDateTimeParser(new DateTimeParser(timezone, new CultureInfo(culture)));
            builder.WithDoubleParser(new DoubleParser(culture));
            builder.WithDecimalParser(new DecimalParser(culture));

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)builder).GetExpression<Person>(property, operation, value);

            if (generatesNull == true)
            {
                Assert.IsNull(expr);
                return;
            }

            ICollection<Person> items = persons.Where(expr.Compile()).ToList();
            Assert.IsTrue(items.Count == expectedCount);
        }

        [DataTestMethod]

        // ENUM Tests
        [DataRow("nl-BE", "Europe/Paris", "Type", "like", "1", 2, false)]
        [DataRow("en-US", "Europe/Paris", "Type", "like", "1", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Type", "like", "0", 1, false)]
        [DataRow("en-US", "Europe/Paris", "Type", "like", "0", 1, false)]

        // STRING Tests
        [DataRow("nl-BE", "Europe/Paris", "Name", "like", "Jeffrey Lebowski", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Name", "like", "Jeffrey", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Name", "like", "Walter Sobchak", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "Name", "like", "Sobchak", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "Name", "like", "Theodore Donald 'Donny' Kerabatsos", 0, false)]

        // BOOLEAN tests
        [DataRow("nl-BE", "Europe/Paris", "IsGolfer", "like", "true", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "IsGolfer", "like", "false", 2, false)]

        // NULLABLE BOOLEAN tests
        [DataRow("nl-BE", "Europe/Paris", "IsPederast", "like", "true", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "IsPederast", "like", "false", 2, false)]

        // DOUBLE tests
        [DataRow("nl-BE", "Europe/Paris", "Height", "like", "185,25", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "like", "185.25", 0, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "like", "185,25", 0, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "like", "185.25", 2, false)]
        [DataRow("sv-SE", "Europe/Paris", "Height", "like", "185.25", 0, true)]
        [DataRow("sv-SE", "Europe/Paris", "Height", "like", "185,25", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "gt", "185,25", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "gte", "185,25", 3, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "gt", "185,25", 0, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "gte", "185,25", 0, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "gt", "185.25", 1, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "gte", "185.25", 3, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "gt", "185.25", 0, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "gte", "185.25", 0, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "lt", "190,25", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "lte", "185,25", 2, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "lt", "190,25", 3, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "lte", "185,25", 3, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "lt", "190.25", 2, false)]
        [DataRow("en-US", "Europe/Paris", "Height", "lte", "185.25", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "lt", "190.25", 3, false)]
        [DataRow("nl-BE", "Europe/Paris", "Height", "lte", "185.25", 3, false)]

        // DECIMAL tests
        [DataRow("nl-BE", "Europe/Paris", "Width", "like", "185,25", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Width", "like", "185.25", 0, false)]
        [DataRow("en-US", "Europe/Paris", "Width", "like", "185,25", 0, false)]
        [DataRow("en-US", "Europe/Paris", "Width", "like", "185.25", 2, false)]
        [DataRow("sv-SE", "Europe/Paris", "Width", "like", "185.25", 0, true)]
        [DataRow("sv-SE", "Europe/Paris", "Width", "like", "185,25", 2, false)]

        // NULLABLE DECIMAL tests
        [DataRow("nl-BE", "Europe/Paris", "Length", "like", "185,25", 2, false)]
        [DataRow("nl-BE", "Europe/Paris", "Length", "like", "185.25", 0, false)]
        [DataRow("en-US", "Europe/Paris", "Length", "like", "185,25", 0, false)]
        [DataRow("en-US", "Europe/Paris", "Length", "like", "185.25", 2, false)]
        [DataRow("sv-SE", "Europe/Paris", "Length", "like", "185.25", 0, true)]
        [DataRow("sv-SE", "Europe/Paris", "Length", "like", "185,25", 2, false)]

        // DATETIME tests
        [DataRow("nl-BE", "Europe/Paris", "BirthDate", "like", "4/12/1942", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "BirthDate", "like", "4/12/1942 18:05:02", 1, false)]
        [DataRow("en-US", "Europe/Paris", "BirthDate", "like", "4/12/1942", 0, false)]
        [DataRow("en-US", "Europe/Paris", "BirthDate", "like", "4/12/1942 18:05:02", 0, false)]
        [DataRow("en-US", "Europe/Paris", "BirthDate", "like", "12-4-1942", 1, false)]
        [DataRow("en-US", "Europe/Paris", "BirthDate", "like", "12-4-1942 18:05:02", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "BirthDate", "like", "12-4-1942", 0, false)]
        [DataRow("nl-BE", "Europe/Paris", "BirthDate", "like", "12-4-1942 18:05:02", 0, false)]

        // NULLABLE DATETIME tests
        [DataRow("nl-BE", "Europe/Paris", "JoinedNam", "like", "4/12/2000", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "JoinedNam", "like", "4/12/2000 18:05:02", 1, false)]
        [DataRow("en-US", "Europe/Paris", "JoinedNam", "like", "4/12/2000", 0, false)]
        [DataRow("en-US", "Europe/Paris", "JoinedNam", "like", "4/12/2000 18:05:02", 0, false)]
        [DataRow("en-US", "Europe/Paris", "JoinedNam", "like", "12-4-2000", 1, false)]
        [DataRow("en-US", "Europe/Paris", "JoinedNam", "like", "12-4-2000 18:05:02", 1, false)]
        [DataRow("nl-BE", "Europe/Paris", "JoinedNam", "like", "12-4-2000", 0, false)]
        [DataRow("nl-BE", "Europe/Paris", "JoinedNam", "like", "12-4-2000 18:05:02", 0, false)]
        public void ExpressionBuilder_GetExpression_NavigationProperty(
            string culture,
            string timezone,
            string property,
            string operation,
            string value,
            int expectedCount,
            bool? generatesNull = false)
        {
            List<Person> persons = new List<Person>
            {
                new Person
                {
                    Characteristic = new Characteristic()
                    {
                        Type = PlayerType.Bowler,
                        IsGolfer = false,
                        Name = "Jeffrey Lebowski",
                        Height = 185.25,
                        Width = 185.25M,
                        Length = 185.25M,
                        BirthDate = new DateTime(1942, 12,4),
                        JoinedNam = new DateTime(2000, 12,4),
                        IsPederast = false
                    }
                },
                new Person
                {
                    Characteristic = new Characteristic()
                    {
                        Type = PlayerType.Bowler,
                        IsGolfer = false,
                        Name = "Jeffrey Lebowski",
                        Height = 185.25,
                        Width = 185.25M,
                        Length =  185.25M,
                        BirthDate = new DateTime(1942, 12,4, 18,5,2),
                        JoinedNam = new DateTime(2000, 12,4, 18,5,2),
                        IsPederast = false
                    }
                },
                new Person
                {
                    Characteristic = new Characteristic()
                    {
                        Type = PlayerType.Golfer,
                        IsGolfer = true,
                        Name = "Walter Sobchak",
                        Height = 193.64,
                        Width = 193.64M,
                        Length = 193.64M,
                        BirthDate = new DateTime(1940,1,5),
                        JoinedNam = new DateTime(2000,1,5),
                        IsPederast = true
                    }
                }
            };

            ExpressionBuilder builder = new ExpressionBuilder();
            builder.WithDateTimeParser(new DateTimeParser(timezone, new CultureInfo(culture)));
            builder.WithDoubleParser(new DoubleParser(culture));
            builder.WithDecimalParser(new DecimalParser(culture));

            IDictionary<int, string> navigationProperty = new Dictionary<int, string>();
            navigationProperty.Add(new KeyValuePair<int, string>(1, "Characteristic"));
            navigationProperty.Add(new KeyValuePair<int, string>(2, property));

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)builder).GetExpression<Person>(navigationProperty, operation, value, "");
            if (generatesNull == true)
            {
                Assert.IsNull(expr);
                return;
            }

            ICollection<Person> items = persons.Where(expr.Compile()).ToList();
            Assert.IsTrue(items.Count == expectedCount);
        }

        [TestMethod]
        public void ExpressionBuilder_GetExpression_MissingOperator_ThrowsArgumentExceptionException()
        {
            ExpressionBuilder builder = new ExpressionBuilder();
            builder.WithDateTimeParser(new DateTimeParser("", new CultureInfo("")));
            builder.WithDoubleParser(new DoubleParser(""));
            builder.WithDecimalParser(new DecimalParser(""));

            Assert.ThrowsException<ArgumentException>(() =>
                ((IFilterExpressionBuilder)builder).GetExpression<Person>("Type", "", "1"));
        }
    }
}