using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using Dime.Expressions.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Expressions.Tests
{
    [TestClass]
    public class ParserDescriptorTests
    {
        [TestMethod]
        public void ParserDescriptor_AddParser_AddsDateTimeParser()
        {
            // Arrange
            ParserDescriptor descriptor = new();
            IParser<DateTime> parser = new DateTimeParser("Europe/Paris", new CultureInfo("en-US"));

            // Act
            descriptor.AddParser(parser);

            // Assert
            IParser result = descriptor.GetParser(new DateTimeConverter());
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IParser<DateTime>));
        }

        [TestMethod]
        public void ParserDescriptor_AddParser_AddsDoubleParser()
        {
            // Arrange
            ParserDescriptor descriptor = new();
            IParser<double> parser = new DoubleParser("en-US");

            // Act
            descriptor.AddParser(parser);

            // Assert
            IParser result = descriptor.GetParser(new DoubleConverter());
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IParser<double>));
        }

        [TestMethod]
        public void ParserDescriptor_AddParser_AddsDecimalParser()
        {
            // Arrange
            ParserDescriptor descriptor = new();
            IParser<decimal> parser = new DecimalParser("en-US");

            // Act
            descriptor.AddParser(parser);

            // Assert
            IParser result = descriptor.GetParser(new DecimalConverter());
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IParser<decimal>));
        }

        [TestMethod]
        public void ParserDescriptor_GetParser_WithNullableDateTime_ReturnsDateTimeParser()
        {
            // Arrange
            ParserDescriptor descriptor = new();
            IParser<DateTime> parser = new DateTimeParser("Europe/Paris", new CultureInfo("en-US"));
            descriptor.AddParser(parser);

            // Act
            IParser result = descriptor.GetParser(new NullableConverter(typeof(DateTime?)));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IParser<DateTime>));
        }

        [TestMethod]
        public void ParserDescriptor_GetParser_WithNullableDouble_ReturnsDoubleParser()
        {
            // Arrange
            ParserDescriptor descriptor = new();
            IParser<double> parser = new DoubleParser("en-US");
            descriptor.AddParser(parser);

            // Act
            IParser result = descriptor.GetParser(new NullableConverter(typeof(double?)));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IParser<double>));
        }

        [TestMethod]
        public void ParserDescriptor_GetParser_WithNullableDecimal_ReturnsDecimalParser()
        {
            // Arrange
            ParserDescriptor descriptor = new();
            IParser<decimal> parser = new DecimalParser("en-US");
            descriptor.AddParser(parser);

            // Act
            IParser result = descriptor.GetParser(new NullableConverter(typeof(decimal?)));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IParser<decimal>));
        }

        [TestMethod]
        public void ParserDescriptor_GetParser_WithStringConverter_ReturnsNull()
        {
            // Arrange
            ParserDescriptor descriptor = new();

            // Act
            IParser result = descriptor.GetParser(new StringConverter());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ParserDescriptor_Indexer_Get_ReturnsParser()
        {
            // Arrange
            ParserDescriptor descriptor = new();
            IParser<double> parser = new DoubleParser("en-US");
            descriptor.AddParser(parser);

            // Act
            IParser result = descriptor[0];

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(parser, result);
        }

        [TestMethod]
        public void ParserDescriptor_Indexer_Set_UpdatesParser()
        {
            // Arrange
            ParserDescriptor descriptor = new();
            IParser<double> oldParser = new DoubleParser("en-US");
            IParser<double> newParser = new DoubleParser("nl-BE");
            descriptor.AddParser(oldParser);

            // Act
            descriptor[0] = newParser;

            // Assert
            IParser result = descriptor[0];
            Assert.AreEqual(newParser, result);
        }

        [TestMethod]
        public void ParserDescriptor_GetParser_WithDateOnlyConverter_ReturnsDateOnlyParser()
        {
            // Arrange
            ParserDescriptor descriptor = new();
            IParser<DateOnly> parser = new DateOnlyParser("Europe/Paris", new CultureInfo("en-US"));
            descriptor.AddParser(parser);

            // Act
            IParser result = descriptor.GetParser(new DateOnlyConverter());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IParser<DateOnly>));
        }

        [TestMethod]
        public void ParserDescriptor_GetParser_WithNullableDateOnly_ReturnsDateOnlyParser()
        {
            // Arrange
            ParserDescriptor descriptor = new();
            IParser<DateOnly> parser = new DateOnlyParser("Europe/Paris", new CultureInfo("en-US"));
            descriptor.AddParser(parser);

            // Act
            IParser result = descriptor.GetParser(new NullableConverter(typeof(DateOnly?)));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IParser<DateOnly>));
        }

        [TestMethod]
        public void ParserDescriptor_GetParser_WithNullableIntConverter_ReturnsNull()
        {
            // Arrange
            ParserDescriptor descriptor = new();

            // Act
            IParser result = descriptor.GetParser(new NullableConverter(typeof(int?)));

            // Assert
            Assert.IsNull(result);
        }
    }
}

