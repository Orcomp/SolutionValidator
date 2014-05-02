#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="CSharpFormattingPropertiesTest.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Tests.Configuration
{
	#region using...
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using ICSharpCode.NRefactory.CSharp;
	using NUnit.Framework;
	using SolutionValidator.Configuration;

	#endregion

	[TestFixture]
	public class CSharpFormattingPropertiesTest
	{
		private const int PropertyCount = 164;
		private const string TestFolder = @"TestData\TestFormattiongOptions";

		private void CheckOptions(CSharpFormattingOptions expected, CSharpFormattingOptions actual, int expectedNotMatch, int expectedNotNull = PropertyCount)
		{
			var actualNotMatch = 0;
			var actualNotNull = 0;

			foreach (var propertyInfo in GetCSharpFormattingOptionsProperties())
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

		private void ChangeAllProperties(CSharpFormattingOptions options)
		{
			foreach (var propertyInfo in GetCSharpFormattingOptionsProperties())
			{
				ChangeProperty(options, propertyInfo);
			}
		}

		private string CreateTempPropertyFileWithSingleProperty(PropertyInfo propertyInfo, CSharpFormattingOptions options)
		{
			var result = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			var properties = new CSharpFormattingProperties();
			properties.Set(propertyInfo.Name, propertyInfo.GetValue(options, new object[0]));
			properties.Save(result);
			return result;
		}

		private void ChangeProperty(CSharpFormattingOptions options, PropertyInfo propertyInfo)
		{
			if (propertyInfo.PropertyType.IsEnum)
			{
				var originalValue = propertyInfo.GetValue(options, new object[0]);
				var values = Enum.GetValues(propertyInfo.PropertyType);
				foreach (var value in values)
				{
					if (!originalValue.Equals(value))
					{
						propertyInfo.SetValue(options, value, new object[0]);
						return;
					}
				}
			}

			if (propertyInfo.PropertyType == typeof (bool))
			{
				propertyInfo.SetValue(options, !((bool) propertyInfo.GetValue(options, new object[0])), new object[0]);
				return;
			}

			if (propertyInfo.PropertyType == typeof (int))
			{
				propertyInfo.SetValue(options, ((int) propertyInfo.GetValue(options, new object[0])) + 1, new object[0]);
			}
		}

		private IList<string> GetMatchingPropertyNames(CSharpFormattingOptions options, CSharpFormattingOptions otherOptions)
		{
			return GetPropertyNames(options, otherOptions, Matching.Match);
		}

		private IList<string> GetNotMatchingPropertyNames(CSharpFormattingOptions options, CSharpFormattingOptions otherOptions)
		{
			return GetPropertyNames(options, otherOptions, Matching.DoesNotMatch);
		}

		private IList<string> GetPropertyNames(CSharpFormattingOptions options, CSharpFormattingOptions otherOptions, Matching matching)
		{
			var result = new List<string>();
			foreach (var propertyInfo in GetCSharpFormattingOptionsProperties())
			{
				var value = propertyInfo.GetValue(options, new object[0]);
				var otherValue = propertyInfo.GetValue(otherOptions, new object[0]);
				if (Equals(value, otherValue) == (matching == Matching.Match))
				{
					result.Add(propertyInfo.Name);
				}
			}
			return result;
		}

		private IEnumerable<PropertyInfo> GetCSharpFormattingOptionsProperties()
		{
			return typeof (CSharpFormattingOptions).GetProperties()
				.Where(propertyInfo =>
					!propertyInfo.Name.Equals("Name")
					&& !propertyInfo.Name.Equals("IsBuiltIn"));
		}

		[Test]
		public void TestAllIsOverWriting()
		{
			var actual = CSharpFormattingProperties.GetOptions(Path.Combine(TestFolder, "SimpleComplement.xml"));
			var expected = FormattingOptionsFactory.CreateSharpDevelop();

			var x = GetMatchingPropertyNames(expected, actual);
			CheckOptions(expected, actual, PropertyCount);

			expected = CSharpFormattingProperties.GetOptions(Path.Combine(TestFolder, "Simple.xml"));
			CheckOptions(expected, actual, PropertyCount);
		}

		[Test]
		public void TestDefault()
		{
			var actual = CSharpFormattingProperties.GetOptions();
			var expected = CSharpFormattingProperties.CreateOrcompOptions();
			CheckOptions(expected, actual, 0);
		}

		[Test]
		[Explicit]
		public void TestGetoptionsWithSinglePropertyOverWrite()
		{
			const string propertyFileName = "SharpDevelopProperties.xml";
			var expected = FormattingOptionsFactory.CreateSharpDevelop();

			foreach (var propertyInfo in GetCSharpFormattingOptionsProperties())
			{
				var loaded = CSharpFormattingProperties.GetOptions(Path.Combine(TestFolder, propertyFileName));

				// Save the original property value to a one setting temp property file
				var tempFileName = CreateTempPropertyFileWithSingleProperty(propertyInfo, loaded);
				try
				{
					CheckOptions(expected, loaded, 0);

					ChangeAllProperties(loaded);
					CheckOptions(expected, loaded, PropertyCount);

					loaded = CSharpFormattingProperties.GetOptions(tempFileName, loaded);
				}
				finally
				{
					File.Delete(tempFileName);
				}

				// Check we got back the original property
				var matchingPropertyNames = GetMatchingPropertyNames(expected, loaded);
				Assert.AreEqual(1, matchingPropertyNames.Count, matchingPropertyNames.ToString());
				Assert.AreEqual(propertyInfo.Name, matchingPropertyNames.FirstOrDefault());
			}
		}

		[Test]
		public void TestLoadFullFromSharpDevelopXml()
		{
			var actual = CSharpFormattingProperties.GetOptions(Path.Combine(TestFolder, "SharpDevelopProperties.xml"));
			var expected = FormattingOptionsFactory.CreateSharpDevelop();
			CheckOptions(expected, actual, 0);

			expected = FormattingOptionsFactory.CreateEmpty();
			CheckOptions(expected, actual, 82);
		}

		[Test]
		public void TestLoadFullFromSimpleXml()
		{
			var actual = CSharpFormattingProperties.GetOptions(Path.Combine(TestFolder, "Simple.xml"));
			var expected = FormattingOptionsFactory.CreateSharpDevelop();
			CheckOptions(expected, actual, 0);

			expected = FormattingOptionsFactory.CreateEmpty();
			CheckOptions(expected, actual, 82);
		}
	}

	internal enum Matching
	{
		Match,
		DoesNotMatch
	}
}