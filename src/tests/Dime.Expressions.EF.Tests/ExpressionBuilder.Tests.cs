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
        [InlineData("nl-BE", "Europe/Paris", "Type", "like", "1", 2, false)]
        [InlineData("en-US", "Europe/Paris", "Type", "like", "1", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Type", "like", "0", 1, false)]
        [InlineData("en-US", "Europe/Paris", "Type", "like", "0", 1, false)]
        public void ExpressionBuilder_GetExpression_ScalarProperty_Enum(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
           => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Jeffrey Lebowski", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Jeffrey", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Walter Sobchak", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Sobchak", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "like", "Theodore Donald 'Donny' Kerabatsos", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "nullorempty", "", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "Name", "notnullorempty", "", 3, false)]
        public void ExpressionBuilder_GetExpression_ScalarProperty_String(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
           => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "IsGolfer", "like", "true", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "IsGolfer", "like", "false", 2, false)]
        public void ExpressionBuilder_GetExpression_ScalarProperty_Boolean(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
           => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "IsPederast", "like", "true", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "IsPederast", "like", "false", 2, false)]
        public void ExpressionBuilder_GetExpression_ScalarProperty_NullableBoolean(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
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
        public void ExpressionBuilder_GetExpression_ScalarProperty_Double(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
           => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "Id", "endswith", "1", 1, true)]
        [InlineData("nl-BE", "Europe/Paris", "Id", "startswith", "1", 1, true)]
        [InlineData("nl-BE", "Europe/Paris", "Id", "nullorempty", "1", 1, true)]
        public void ExpressionBuilder_GetExpression_ScalarProperty_Integer(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
           => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "Width", "like", "185,25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Width", "like", "185.25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Width", "like", "185,25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Width", "like", "185.25", 2, false)]
        [InlineData("sv-SE", "Europe/Paris", "Width", "like", "185.25", 0, true)]
        [InlineData("sv-SE", "Europe/Paris", "Width", "like", "185,25", 2, false)]
        public void ExpressionBuilder_GetExpression_ScalarProperty_Decimal(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
           => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "Length", "like", "185,25", 2, false)]
        [InlineData("nl-BE", "Europe/Paris", "Length", "like", "185.25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Length", "like", "185,25", 0, false)]
        [InlineData("en-US", "Europe/Paris", "Length", "like", "185.25", 2, false)]
        [InlineData("sv-SE", "Europe/Paris", "Length", "like", "185.25", 0, true)]
        [InlineData("sv-SE", "Europe/Paris", "Length", "like", "185,25", 2, false)]
        public void ExpressionBuilder_GetExpression_ScalarProperty_NullableDecimal(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
           => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

        [Theory]
        [InlineData("nl-BE", "Europe/Paris", "BirthDate", "like", "4/12/1942", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "BirthDate", "like", "4/12/1942 18:05:02", 1, false)]
        [InlineData("en-US", "Europe/Paris", "BirthDate", "like", "4/12/1942", 0, false)]
        [InlineData("en-US", "Europe/Paris", "BirthDate", "like", "4/12/1942 18:05:02", 0, false)]
        [InlineData("en-US", "Europe/Paris", "BirthDate", "like", "12-4-1942", 1, false)]
        [InlineData("en-US", "Europe/Paris", "BirthDate", "like", "12-4-1942 18:05:02", 1, false)]
        [InlineData("nl-BE", "Europe/Paris", "BirthDate", "like", "12-4-1942", 0, false)]
        [InlineData("nl-BE", "Europe/Paris", "BirthDate", "like", "12-4-1942 18:05:02", 0, false)]
        public void ExpressionBuilder_GetExpression_ScalarProperty_DateTime(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
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
        public void ExpressionBuilder_GetExpression_ScalarProperty_NullableDateTime(string culture, string timezone, string property, string operation, string value, int expectedCount, bool? generatesNull = false)
        => Test(culture, timezone, property, operation, value, expectedCount, generatesNull);

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