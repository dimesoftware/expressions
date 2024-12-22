using System.Globalization;
using System.Linq.Expressions;
using Dime.Expressions.EF.Tests.Mock;
using Dime.Expressions.Tests;
using Dime.Expressions.Tests.Mock;

namespace Dime.Expressions.EF.Tests
{
    public partial class ExpressionBuilderTests
    {
        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "Characteristic", "like", "Well, You Know, That's Just, Like, Your Opinion, Man", 2, false)]
        public void ExpressionBuilder_GetExpression_NavigationProperty_Root_UseDefaultDisplayAttribute(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
        {
            ExpressionBuilder builder = new();
            builder.WithDateTimeParser(new DateTimeParser(timezone, new CultureInfo(culture)));
            builder.WithDoubleParser(new DoubleParser(culture));
            builder.WithDecimalParser(new DecimalParser(culture));

            IDictionary<int, string> navigationProperty = new Dictionary<int, string>();
            navigationProperty.Add(new KeyValuePair<int, string>(1, property));

            Expression<Func<Person, bool>> expr = ((IFilterExpressionBuilder)builder).GetExpression<Person>(navigationProperty, operation, value, "");
            if (generatesNull == true)
            {
                Assert.Null(expr);
                return;
            }

            PeopleRepository repository = new();
            CustomerDbContext db = repository.Create();
            ICollection<Person> items = db.Set<Person>().Where(expr.Compile()).ToList();
            Assert.True(items.Count == expectedCount);
        }

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "Type", "like", "1", 2, false)]
        [InlineData("en-US", "Europe/Paris", "Type", "like", "1", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Type", "like", "0", 1, false)]
        [InlineData("en-US", "Europe/Paris", "Type", "like", "0", 1, false)]
        public void ExpressionBuilder_GetExpression_NavigationProperty_Enum(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
            => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "SecondaryType", "like", "0", 2, false)]
        [InlineData("en-US", "Europe/Paris", "SecondaryType", "like", "0", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "SecondaryType", "like", "1", 1, false)]
        [InlineData("en-US", "Europe/Paris", "SecondaryType", "like", "1", 1, false)]
        [InlineData("en-US", "Europe/Paris", "SecondaryType", "like", null, 0, false)]
        public void ExpressionBuilder_GetExpression_NavigationProperty_NullableEnum(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
            => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Jeffrey Lebowski", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Jeffrey", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Walter Sobchak", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Sobchak", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Theodore Donald 'Donny' Kerabatsos", 0, false)]
        public void ExpressionBuilder_GetExpression_NavigationProperty_String(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
            => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "IsGolfer", "like", "true", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "IsGolfer", "like", "false", 2, false)]
        public void ExpressionBuilder_GetExpression_NavigationProperty_Boolean(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
            => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "IsPederast", "like", "true", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "IsPederast", "like", "false", 2, false)]
        public void ExpressionBuilder_GetExpression_NavigationProperty_NullableBoolean(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
            => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        [Theory]
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
        public void ExpressionBuilder_GetExpression_NavigationProperty_Double(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
            => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "Score", "like", "185,25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Score", "like", "185.25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Score", "like", "185,25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Score", "like", "185.25", 2, false)]
        [InlineData("sv-SE", "Europe/Paris", "Score", "like", "185.25", 0, true)]
        [InlineData("sv-SE", "Europe/Paris", "Score", "like", "185,25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Score", "gt", "185,25", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "Score", "gte", "185,25", 3, false)]
        [InlineData("en-US", "Europe/Paris", "Score", "gt", "185,25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Score", "gte", "185,25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Score", "gt", "185.25", 1, false)]
        [InlineData("en-US", "Europe/Paris", "Score", "gte", "185.25", 3, false)]
        [InlineData("nl-BE", "Europe/Paris", "Score", "gt", "185.25", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "Score", "gte", "185.25", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "Score", "lt", "190,25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Score", "lte", "185,25", 2, false)]
        [InlineData("en-US", "Europe/Paris", "Score", "lt", "190,25", 3, false)]
        [InlineData("en-US", "Europe/Paris", "Score", "lte", "185,25", 3, false)]
        [InlineData("en-US", "Europe/Paris", "Score", "lt", "190.25", 2, false)]
        [InlineData("en-US", "Europe/Paris", "Score", "lte", "185.25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Score", "lt", "190.25", 3, false)]
        [InlineData("nl-BE", "Europe/Paris", "Score", "lte", "185.25", 3, false)]
        public void ExpressionBuilder_GetExpression_NavigationProperty_NullableDouble(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
            => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "Width", "like", "185,25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Width", "like", "185.25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Width", "like", "185,25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Width", "like", "185.25", 2, false)]
        [InlineData("sv-SE", "Europe/Paris", "Width", "like", "185.25", 0, true)]
        [InlineData("sv-SE", "Europe/Paris", "Width", "like", "185,25", 2, false)]
        public void ExpressionBuilder_GetExpression_NavigationProperty_Decimal(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
            => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "Length", "like", "185,25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Length", "like", "185.25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Length", "like", "185,25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Length", "like", "185.25", 2, false)]
        [InlineData("sv-SE", "Europe/Paris", "Length", "like", "185.25", 0, true)]
        [InlineData("sv-SE", "Europe/Paris", "Length", "like", "185,25", 2, false)]
        public void ExpressionBuilder_GetExpression_NavigationProperty_NullableDecimal(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
            => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        [Theory]
        // DATETIME tests
        [InlineData("nl-BE", "Europe/Paris", "BirthDate", "like", "4/12/1942", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "BirthDate", "like", "4/12/1942 18:05:02", 1, false)]
        [InlineData("en-US", "Europe/Paris", "BirthDate", "like", "4/12/1942", 0, false)]
        [InlineData("en-US", "Europe/Paris", "BirthDate", "like", "4/12/1942 18:05:02", 0, false)]
        [InlineData("en-US", "Europe/Paris", "BirthDate", "like", "12-4-1942", 1, false)]
        [InlineData("en-US", "Europe/Paris", "BirthDate", "like", "12-4-1942 18:05:02", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "BirthDate", "like", "12-4-1942", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "BirthDate", "like", "12-4-1942 18:05:02", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "BirthDate", "gt", "12-4-1935 18:05:02", 3, false)]
        public void ExpressionBuilder_GetExpression_NavigationProperty_DateTime(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
            => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "JoinedNam", "like", "4/12/2000", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "JoinedNam", "like", "4/12/2000 18:05:02", 1, false)]
        [InlineData("en-US", "Europe/Paris", "JoinedNam", "like", "4/12/2000", 0, false)]
        [InlineData("en-US", "Europe/Paris", "JoinedNam", "like", "4/12/2000 18:05:02", 0, false)]
        [InlineData("en-US", "Europe/Paris", "JoinedNam", "like", "12-4-2000", 1, false)]
        [InlineData("en-US", "Europe/Paris", "JoinedNam", "like", "12-4-2000 18:05:02", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "JoinedNam", "like", "12-4-2000", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "JoinedNam", "like", "12-4-2000 18:05:02", 0, false)]
        public void ExpressionBuilder_GetExpression_NavigationProperty_NullableDateTime(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
            => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        private static void Test(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull)
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
            CustomerDbContext db = repository.Create();
            if (expr == null)
            {
                Assert.Equal(0, expectedCount);
                return;
            }

            ICollection<Person> items = db.Set<Person>().Where(expr.Compile()).ToList();
            Assert.True(items.Count == expectedCount);
        }
    }
}