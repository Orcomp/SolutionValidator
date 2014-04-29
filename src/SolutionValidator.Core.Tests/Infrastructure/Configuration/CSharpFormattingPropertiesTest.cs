// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationTest.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Tests.Configuration
{
    using System.IO;
    using System.Linq;
    using ICSharpCode.NRefactory.CSharp;
    using NUnit.Framework;
    using SolutionValidator.Configuration;

    [TestFixture]
    public class CSharpFormattingPropertiesTest
    {
        private const string TestFolder = @"TestData\TestFormattiongOptions";

        [Test]
        public void TestDefault()
        {
            var actual = CSharpFormattingProperties.GetOptions();
            var expected = FormattingOptionsFactory.CreateAllman();
            CheckOptions(expected, actual);
            CheckOptions(expected, actual, 0);
        }


        [Test]
        public void TestLoadFullFromSharpDevelopXml()
        {
            var actual = CSharpFormattingProperties.GetOptions(Path.Combine(TestFolder, "SharpDevelopProperties.xml"));
            var expected = FormattingOptionsFactory.CreateSharpDevelop();
            CheckOptions(expected, actual, "");

            expected = FormattingOptionsFactory.CreateEmpty();
            CheckOptions(expected, actual, 82);

            //actual.ElseIfNewLinePlacement
        }

        [Test]
        public void TestLoadFullFromSimpleXml()
        {
            var actual = CSharpFormattingProperties.GetOptions(Path.Combine(TestFolder, "Simple.xml"));
            var expected = FormattingOptionsFactory.CreateSharpDevelop();
            CheckOptions(expected, actual, "");

            expected = FormattingOptionsFactory.CreateEmpty();
            CheckOptions(expected, actual, 82);
        }

        [Test]
        public void TestAll165IsOverWriting()
        {
            var actual = CSharpFormattingProperties.GetOptions(Path.Combine(TestFolder, "SimpleComplement.xml"));
            var expected = FormattingOptionsFactory.CreateSharpDevelop();
            CheckOptions(expected, actual, 165);

            expected = CSharpFormattingProperties.GetOptions(Path.Combine(TestFolder, "Simple.xml"));
            CheckOptions(expected, actual, 165);

        }

        private void CheckOptions(CSharpFormattingOptions expected, CSharpFormattingOptions actual, string expectedNotMatching = null)
        {
            expectedNotMatching = expectedNotMatching ?? "";
            var notMatchings = expectedNotMatching.Split(';');

            foreach (var propertyInfo in typeof(CSharpFormattingOptions).GetProperties())
            {
                var expectedValue = propertyInfo.GetValue(expected, new object[0]);
                var actualValue = propertyInfo.GetValue(actual, new object[0]);
                if (notMatchings.Contains(propertyInfo.Name))
                {
                    Assert.AreNotEqual(expectedValue, actualValue, string.Format("Property '{0}' equals (but should not)", propertyInfo.Name));
                }
                else
                {
                    Assert.AreEqual(expectedValue, actualValue, string.Format("Property '{0}' does not equal", propertyInfo.Name));

                }
            }
        }

        private void CheckOptions(CSharpFormattingOptions expected, CSharpFormattingOptions actual, int expectedNotMatch, int expectedNotNull = 165)
        {
            var actualNotMatch = 0;
            var actualNotNull = 0;

            foreach (var propertyInfo in typeof(CSharpFormattingOptions).GetProperties())
            {
                var expectedValue = propertyInfo.GetValue(expected, new object[0]);
                var actualValue = propertyInfo.GetValue(actual, new object[0]);

                if (actualValue == null)
                {
                    continue;
                }
                actualNotNull++;
                if (Equals(expectedValue, actualValue) && expectedValue.GetType() == actualValue.GetType())
                {
                    continue;
                }
                actualNotMatch++;

            }
            Assert.AreEqual(expectedNotNull, actualNotNull, "expectedNotNull <> actualNotNull");
            Assert.AreEqual(expectedNotMatch, actualNotMatch, "expectedNotMatch <> actualNotMatch");

        }
    }
}
