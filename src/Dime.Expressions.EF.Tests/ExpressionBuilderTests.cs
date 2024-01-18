using System.Globalization;
using System.Linq.Expressions;
using Dime.Expressions.EF.Tests.Mock;
using Dime.Expressions.Tests;
using Dime.Expressions.Tests.Mock;

namespace Dime.Expressions.EF.Tests
{
    public class ExpressionBuilderTests
    {
        [Theory]

        // ENUM Tests
        [InlineData("nl-BE", "Europe/Paris", "Type", "like", "1", 2, false)]
        [InlineData("en-US", "Europe/Paris", "Type", "like", "1", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Type", "like", "0", 1, false)]
        [InlineData("en-US", "Europe/Paris", "Type", "like", "0", 1, false)]

        // STRING Tests
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Jeffrey Lebowski", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Jeffrey", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Walter Sobchak", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Sobchak", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Theodore Donald 'Donny' Kerabatsos", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "nullorempty", "", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "notnullorempty", "", 3, false)]

        // BOOLEAN tests
        [InlineData("nl-BE", "Europe/Paris", "IsGolfer", "like", "true", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "IsGolfer", "like", "false", 2, false)]

        // NULLABLE BOOLEAN tests
        [InlineData("nl-BE", "Europe/Paris", "IsPederast", "like", "true", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "IsPederast", "like", "false", 2, false)]

        // DOUBLE tests
        [InlineData("nl-BE", "Europe/Paris", "Height", "like", "185,25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "like", "185.25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "like", "185,25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "like", "185.25", 2, false)]
        [InlineData("sv-SE", "Europe/Paris", "Height", "like", "185.25", 0, true)]
        [InlineData("sv-SE", "Europe/Paris", "Height", "like", "185,25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "gt", "185,25", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "gte", "185,25", 3, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "gt", "185,25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "gte", "185,25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "gt", "185.25", 1, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "gte", "185.25", 3, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "gt", "185.25", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "gte", "185.25", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "lt", "190,25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "lte", "185,25", 2, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "lt", "190,25", 3, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "lte", "185,25", 3, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "lt", "190.25", 2, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "lte", "185.25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "lt", "190.25", 3, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "lte", "185.25", 3, false)]

        // INT tests
        [InlineData("nl-BE", "Europe/Paris", "Id", "endswith", "1", 1, true)]
        [InlineData("nl-BE", "Europe/Paris", "Id", "startswith", "1", 1, true)]
        [InlineData("nl-BE", "Europe/Paris", "Id", "nullorempty", "1", 1, true)]

        // DECIMAL tests
        [InlineData("nl-BE", "Europe/Paris", "Width", "like", "185,25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Width", "like", "185.25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Width", "like", "185,25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Width", "like", "185.25", 2, false)]
        [InlineData("sv-SE", "Europe/Paris", "Width", "like", "185.25", 0, true)]
        [InlineData("sv-SE", "Europe/Paris", "Width", "like", "185,25", 2, false)]

        // NULLABLE DECIMAL tests
        [InlineData("nl-BE", "Europe/Paris", "Length", "like", "185,25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Length", "like", "185.25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Length", "like", "185,25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Length", "like", "185.25", 2, false)]
        [InlineData("sv-SE", "Europe/Paris", "Length", "like", "185.25", 0, true)]
        [InlineData("sv-SE", "Europe/Paris", "Length", "like", "185,25", 2, false)]

        // DATETIME tests
        [InlineData("nl-BE", "Europe/Paris", "BirthDate", "like", "4/12/1942", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "BirthDate", "like", "4/12/1942 18:05:02", 1, false)]
        [InlineData("en-US", "Europe/Paris", "BirthDate", "like", "4/12/1942", 0, false)]
        [InlineData("en-US", "Europe/Paris", "BirthDate", "like", "4/12/1942 18:05:02", 0, false)]
        [InlineData("en-US", "Europe/Paris", "BirthDate", "like", "12-4-1942", 1, false)]
        [InlineData("en-US", "Europe/Paris", "BirthDate", "like", "12-4-1942 18:05:02", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "BirthDate", "like", "12-4-1942", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "BirthDate", "like", "12-4-1942 18:05:02", 0, false)]

        // NULLABLE DATETIME tests
        [InlineData("nl-BE", "Europe/Paris", "JoinedNam", "like", "4/12/2000", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "JoinedNam", "like", "4/12/2000 18:05:02", 1, false)]
        [InlineData("en-US", "Europe/Paris", "JoinedNam", "like", "4/12/2000", 0, false)]
        [InlineData("en-US", "Europe/Paris", "JoinedNam", "like", "4/12/2000 18:05:02", 0, false)]
        [InlineData("en-US", "Europe/Paris", "JoinedNam", "like", "12-4-2000", 1, false)]
        [InlineData("en-US", "Europe/Paris", "JoinedNam", "like", "12-4-2000 18:05:02", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "JoinedNam", "like", "12-4-2000", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "JoinedNam", "like", "12-4-2000 18:05:02", 0, false)]
        public void ExpressionBuilder_GetExpression_ScalarProperty(
           string culture,
           string timezone,
           string property,
           string operation,
           string value,
           int expectedCount,
           bool? generatesNull = false)
        {
            ExpressionBuilder builder = new();
            builder.WithDateTimeParser(new DateTimeParser(timezone, new CultureInfo(culture)));
            builder.WithDoubleParser(new DoubleParser(culture));
            builder.WithDecimalParser(new DecimalParser(culture));

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)builder).GetExpression<Person>(property, operation, value);

            if (generatesNull == true)
            {
                Assert.Null(expr);
                return;
            }

            PeopleRepository repository = new();
            var db = repository.Create();
            ICollection<Person> items = db.Set<Person>().Where(expr.Compile()).ToList();
            Assert.True(items.Count == expectedCount);
        }

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "Characteristic", "like", "Well, You Know, That's Just, Like, Your Opinion, Man", 2, false)]
        public void ExpressionBuilder_GetExpression_NavigationProperty_Root_UseDefaultDisplayAttribute(
            string culture,
            string timezone,
            string property,
            string operation,
            string value,
            int expectedCount,
            bool? generatesNull = false)
        {
            ExpressionBuilder builder = new();
            builder.WithDateTimeParser(new DateTimeParser(timezone, new CultureInfo(culture)));
            builder.WithDoubleParser(new DoubleParser(culture));
            builder.WithDecimalParser(new DecimalParser(culture));

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)builder).GetExpression<Person>(property, operation, value);

            if (generatesNull == true)
            {
                Assert.Null(expr);
                return;
            }
           
            PeopleRepository repository = new();
            var db = repository.Create();
            ICollection<Person> items = db.Set<Person>().Where(expr.Compile()).ToList();
            Assert.True(items.Count == expectedCount);
        }

        [Theory]

        // ENUM Tests
        [InlineData("nl-BE", "Europe/Paris", "Type", "like", "1", 2, false)]
        [InlineData("en-US", "Europe/Paris", "Type", "like", "1", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Type", "like", "0", 1, false)]
        [InlineData("en-US", "Europe/Paris", "Type", "like", "0", 1, false)]

        // STRING Tests
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Jeffrey Lebowski", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Jeffrey", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Walter Sobchak", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Sobchak", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Theodore Donald 'Donny' Kerabatsos", 0, false)]

        // BOOLEAN tests
        [InlineData("nl-BE", "Europe/Paris", "IsGolfer", "like", "true", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "IsGolfer", "like", "false", 2, false)]

        // NULLABLE BOOLEAN tests
        [InlineData("nl-BE", "Europe/Paris", "IsPederast", "like", "true", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "IsPederast", "like", "false", 2, false)]

        // DOUBLE tests
        [InlineData("nl-BE", "Europe/Paris", "Height", "like", "185,25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "like", "185.25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "like", "185,25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "like", "185.25", 2, false)]
        [InlineData("sv-SE", "Europe/Paris", "Height", "like", "185.25", 0, true)]
        [InlineData("sv-SE", "Europe/Paris", "Height", "like", "185,25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "gt", "185,25", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "gte", "185,25", 3, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "gt", "185,25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "gte", "185,25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "gt", "185.25", 1, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "gte", "185.25", 3, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "gt", "185.25", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "gte", "185.25", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "lt", "190,25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "lte", "185,25", 2, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "lt", "190,25", 3, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "lte", "185,25", 3, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "lt", "190.25", 2, false)]
        [InlineData("en-US", "Europe/Paris", "Height", "lte", "185.25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "lt", "190.25", 3, false)]
        [InlineData("nl-BE", "Europe/Paris", "Height", "lte", "185.25", 3, false)]

        // DECIMAL tests
        [InlineData("nl-BE", "Europe/Paris", "Width", "like", "185,25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Width", "like", "185.25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Width", "like", "185,25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Width", "like", "185.25", 2, false)]
        [InlineData("sv-SE", "Europe/Paris", "Width", "like", "185.25", 0, true)]
        [InlineData("sv-SE", "Europe/Paris", "Width", "like", "185,25", 2, false)]

        // NULLABLE DECIMAL tests
        [InlineData("nl-BE", "Europe/Paris", "Length", "like", "185,25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Length", "like", "185.25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Length", "like", "185,25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Length", "like", "185.25", 2, false)]
        [InlineData("sv-SE", "Europe/Paris", "Length", "like", "185.25", 0, true)]
        [InlineData("sv-SE", "Europe/Paris", "Length", "like", "185,25", 2, false)]

        // DATETIME tests
        [InlineData("nl-BE", "Europe/Paris", "BirthDate", "like", "4/12/1942", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "BirthDate", "like", "4/12/1942 18:05:02", 1, false)]
        [InlineData("en-US", "Europe/Paris", "BirthDate", "like", "4/12/1942", 0, false)]
        [InlineData("en-US", "Europe/Paris", "BirthDate", "like", "4/12/1942 18:05:02", 0, false)]
        [InlineData("en-US", "Europe/Paris", "BirthDate", "like", "12-4-1942", 1, false)]
        [InlineData("en-US", "Europe/Paris", "BirthDate", "like", "12-4-1942 18:05:02", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "BirthDate", "like", "12-4-1942", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "BirthDate", "like", "12-4-1942 18:05:02", 0, false)]

        // NULLABLE DATETIME tests
        [InlineData("nl-BE", "Europe/Paris", "JoinedNam", "like", "4/12/2000", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "JoinedNam", "like", "4/12/2000 18:05:02", 1, false)]
        [InlineData("en-US", "Europe/Paris", "JoinedNam", "like", "4/12/2000", 0, false)]
        [InlineData("en-US", "Europe/Paris", "JoinedNam", "like", "4/12/2000 18:05:02", 0, false)]
        [InlineData("en-US", "Europe/Paris", "JoinedNam", "like", "12-4-2000", 1, false)]
        [InlineData("en-US", "Europe/Paris", "JoinedNam", "like", "12-4-2000 18:05:02", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "JoinedNam", "like", "12-4-2000", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "JoinedNam", "like", "12-4-2000 18:05:02", 0, false)]
        public void ExpressionBuilder_GetExpression_NavigationProperty(
            string culture,
            string timezone,
            string property,
            string operation,
            string value,
            int expectedCount,
            bool? generatesNull = false)
        {
            ExpressionBuilder builder = new();
            builder.WithDateTimeParser(new DateTimeParser(timezone, new CultureInfo(culture)));
            builder.WithDoubleParser(new DoubleParser(culture));
            builder.WithDecimalParser(new DecimalParser(culture));

            IDictionary<int, string> navigationProperty = new Dictionary<int, string>();
            navigationProperty.Add(new KeyValuePair<int, string>(1, "Characteristic"));
            navigationProperty.Add(new KeyValuePair<int, string>(2, property));

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)builder).GetExpression<Person>(navigationProperty, operation, value, "");
            if (generatesNull == true)
            {
                Assert.Null(expr);
                return;
            }

            PeopleRepository repository = new();
            var db = repository.Create();
            ICollection<Person> items = db.Set<Person>().Where(expr.Compile()).ToList();
            Assert.True(items.Count == expectedCount);
        }

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "JoinedNam", "like", "4/12/2000", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "JoinedNam", "like", "4/12/2000 18:05:02", 1, false)]
        [InlineData("en-US", "Europe/Paris", "JoinedNam", "like", "4/12/2000", 0, false)]
        [InlineData("en-US", "Europe/Paris", "JoinedNam", "like", "4/12/2000 18:05:02", 0, false)]
        [InlineData("en-US", "Europe/Paris", "JoinedNam", "like", "12-4-2000", 1, false)]
        [InlineData("en-US", "Europe/Paris", "JoinedNam", "like", "12-4-2000 18:05:02", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "JoinedNam", "like", "12-4-2000", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "JoinedNam", "like", "12-4-2000 18:05:02", 0, false)]
        public void ExpressionBuilder_GetExpression_NavigationProperty_NullableDateTime(
            string culture,
            string timezone,
            string property,
            string operation,
            string value,
            int expectedCount,
            bool? generatesNull = false)
        {
            ExpressionBuilder builder = new();
            builder.WithDateTimeParser(new DateTimeParser(timezone, new CultureInfo(culture)));
            builder.WithDoubleParser(new DoubleParser(culture));
            builder.WithDecimalParser(new DecimalParser(culture));

            IDictionary<int, string> navigationProperty = new Dictionary<int, string>();
            navigationProperty.Add(new KeyValuePair<int, string>(1, "Characteristic"));
            navigationProperty.Add(new KeyValuePair<int, string>(2, property));

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)builder).GetExpression<Person>(navigationProperty, operation, value, "");
            if (generatesNull == true)
            {
                Assert.Null(expr);
                return;
            }

            PeopleRepository repository = new();
            var db = repository.Create();
            ICollection<Person> items = db.Set<Person>().Where(expr.Compile()).ToList();
            Assert.True(items.Count == expectedCount);
        }

        [Fact]
        public void ExpressionBuilder_GetExpression_MissingOperator_ThrowsArgumentExceptionException()
        {
            ExpressionBuilder builder = new();
            builder.WithDateTimeParser(new DateTimeParser("", new CultureInfo("")));
            builder.WithDoubleParser(new DoubleParser(""));
            builder.WithDecimalParser(new DecimalParser(""));

            Assert.Throws<ArgumentException>(() =>
                ((IFilterExpressionBuilder)builder).GetExpression<Person>("Type", "", "1"));
        }
    }
}