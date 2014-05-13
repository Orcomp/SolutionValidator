#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="RewriterTestBase.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Tests.Validator.CodeInspection
{
	#region using...
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using Common;
	using Mono.CSharp.Linq;
	using Moq;
	using NUnit.Framework;
	using SolutionValidator.CodeInspection;
	using SolutionValidator.FolderStructure;

	#endregion

	public class TransformRuleBaseTest
	{
		private const string TestDataPath = "TestData";
		private const string RootPath = "should not matter what is this content";
		private Mock<IFileSystemHelper> fshMock;
		private RepositoryInfo repositoryInfo;
		private const string Extension = "cst";

		[SetUp]
		public void SetUp()
		{
			fshMock = new Mock<IFileSystemHelper>();
		}

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			repositoryInfo = new RepositoryInfo(RootPath);
		}

		protected void Test<T>(string testFolder, string original, string expected, bool isExactExpected, params object[] args)
			where T: TransformRule
		{
	
			var originalFileName = string.Format(@"{0}\Refactoring\{1}\{2}.original.{3}", TestDataPath, testFolder, original, Extension);
			var actualFileName = string.Format(@"{0}\Refactoring\{1}\{2}.actual.{3}", TestDataPath, testFolder, original, Extension);
			var expectedFileName = string.Format(@"{0}\Refactoring\{1}\{2}.expected.{3}", TestDataPath, testFolder, expected, Extension);

			if (expected == null)
			{
				expectedFileName = originalFileName;
			}

			// Arrange:
			fshMock.Setup(f => f.GetFiles(It.IsAny<string>(), It.IsAny<string>(), null)).Returns(new[] {originalFileName});
			fshMock.Setup(f => f.ReadAllText(It.IsAny<string>())).Returns(File.ReadAllText(originalFileName));
			fshMock.Setup(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Encoding>()))
				.Callback((string fileNameNotUsed, string content, Encoding encoding) =>
					File.WriteAllText(actualFileName, content, encoding));

			var constructorArguments = new List<object>();
			constructorArguments.AddRange(args); // Custom parameters if any
			constructorArguments.Add(null); // IncludeExcludeCollection sourceFileFilters
			constructorArguments.Add(fshMock.Object); // IFileSystemHelper fileSystemHelper
			constructorArguments.Add(string.Format("*.{0}", Extension)); // string fileNamePattern
			constructorArguments.Add(false); // isBackupEnabled
			var rule = (T)Activator.CreateInstance(typeof(T), constructorArguments.ToArray());

			// Act:
			var validationResult = rule.Validate(repositoryInfo);
			Assert.AreEqual(validationResult.ErrorCount, 0);

			// Assert:
			if (!File.Exists(expectedFileName))
			{
				expectedFileName = expectedFileName.Replace("expected", "original");
			}
			var expectedSource = File.ReadAllText(expectedFileName);
			var actualSource = File.ReadAllText(actualFileName);

			if (isExactExpected)
			{
				Assert.AreEqual(expectedSource, actualSource);	
			}
			
			Assert.AreEqual(StripWhiteSpaces(expectedSource), StripWhiteSpaces(actualSource));
			Assert.AreEqual(StripWhiteSpaces(expectedSource), StripWhiteSpaces(actualSource));
			Assert.AreEqual(File.ReadAllLines(expectedFileName).Length, File.ReadAllLines(actualFileName).Length);

		}

		string StripWhiteSpaces(string text)
		{
			return Regex.Replace(text, @"\s+", " ");
		}
	}
}