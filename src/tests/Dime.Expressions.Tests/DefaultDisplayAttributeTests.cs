using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Expressions.Tests
{
    [TestClass]
    public class DefaultDisplayAttributeTests
    {
        [TestMethod]
        public void DefaultDisplayAttribute_Constructor_SetsNameProperty()
        {
            // Arrange
            string expectedName = "TestProperty";

            // Act
            DefaultDisplayAttribute attribute = new(expectedName);

            // Assert
            Assert.AreEqual(expectedName, attribute.Name);
        }

        [TestMethod]
        public void DefaultDisplayAttribute_Name_IsReadOnly()
        {
            // Arrange
            string expectedName = "TestProperty";
            DefaultDisplayAttribute attribute = new(expectedName);

            // Act
            string actualName = attribute.Name;

            // Assert
            Assert.AreEqual(expectedName, actualName);
        }

        [TestMethod]
        public void DefaultDisplayAttribute_CanBeAppliedToClass()
        {
            // Arrange
            Type testType = typeof(TestClassWithDefaultDisplay);

            // Act
            var attributes = testType.GetCustomAttributes(typeof(DefaultDisplayAttribute), false);

            // Assert
            Assert.HasCount(1, attributes);
            DefaultDisplayAttribute attr = attributes[0] as DefaultDisplayAttribute;
            Assert.IsNotNull(attr);
            Assert.AreEqual("DisplayName", attr.Name);
        }
    }

    [DefaultDisplay("DisplayName")]
    public class TestClassWithDefaultDisplay
    {
        public string DisplayName { get; set; }
        public string OtherProperty { get; set; }
    }
}

