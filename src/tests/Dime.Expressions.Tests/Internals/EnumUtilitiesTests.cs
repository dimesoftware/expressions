using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Dime.Expressions.Tests.Internals
{
    [TestClass]
    public class EnumUtilitiesTests
    {
        [TestMethod]
        public void EnumUtilities_GetValueFromDescription_WithDescription_ReturnsCorrectEnum()
        {
            // Arrange
            string description = "==";

            // Act
            TestOperator result = description.GetValueFromDescription<TestOperator>();

            // Assert
            Assert.AreEqual(TestOperator.Equals, result);
        }

        [TestMethod]
        public void EnumUtilities_GetValueFromDescription_WithFieldNameNoDescription_ReturnsCorrectEnum()
        {
            // Arrange - using an enum with a member that has no Description attribute
            string name = "NoDescription";

            // Act
            TestOperator result = name.GetValueFromDescription<TestOperator>();

            // Assert
            Assert.AreEqual(TestOperator.NoDescription, result);
        }

        [TestMethod]
        public void EnumUtilities_GetValueFromDescription_WithNonEnumType_ThrowsInvalidOperationException()
        {
            // Arrange
            string description = "test";

            // Act & Assert
            try
            {
                description.GetValueFromDescription<string>();
                Assert.Fail("Expected InvalidOperationException was not thrown");
            }
            catch (InvalidOperationException)
            {
                // Expected
            }
        }

        [TestMethod]
        public void EnumUtilities_GetValueFromDescription_WithInvalidDescription_ThrowsArgumentException()
        {
            // Arrange
            string invalidDescription = "InvalidOperator";

            // Act & Assert
            try
            {
                invalidDescription.GetValueFromDescription<TestOperator>();
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException ex)
            {
                Assert.Contains("Could not extract enum from this value", ex.Message);
            }
        }

        [TestMethod]
        public void EnumUtilities_GetValueFromDescription_WithMultipleDescriptions_ReturnsCorrectEnums()
        {
            // Arrange
            (string Description, TestOperator Expected)[] tests =
            [
                (Description: "==", Expected: TestOperator.Equals),
                (Description: "!=", Expected: TestOperator.NotEquals),
                (Description: ">", Expected: TestOperator.GreaterThan),
                (Description: "<", Expected: TestOperator.LessThan)
            ];

            // Act & Assert
            foreach ((string Description, TestOperator Expected) in tests)
            {
                TestOperator result = Description.GetValueFromDescription<TestOperator>();
                Assert.AreEqual(Expected, result, $"Failed for description: {Description}");
            }
        }
    }

    internal enum TestOperator
    {
        [Description("==")]
        Equals,

        [Description("!=")]
        NotEquals,

        [Description(">")]
        GreaterThan,

        [Description("<")]
        LessThan,

        NoDescription
    }
}
