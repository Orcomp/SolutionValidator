// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectFileHelperTest.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Core.Tests.Validator.ProjectFile
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;
    using SolutionValidator.Validator.Common;
    using SolutionValidator.Validator.ProjectFile;

    [TestFixture]
	public class ProjectFileHelperTest
	{
		#region Setup/Teardown

		[SetUp]
		public void SetUp()
		{
			testee = new ProjectFileHelper("*.csprojfortest");
			result = new ValidationResult(null);
		}

		#endregion

		private const string TestDataPath = "TestData";
		private ProjectFileHelper testee;
		private ValidationResult result;

		[Test]
		[TestCase(@"GetAllProjectFilePath\F0", 0)]
		[TestCase(@"GetAllProjectFilePath\F0\", 0)]
		[TestCase(@"GetAllProjectFilePath\F1Flat", 1)]
		[TestCase(@"GetAllProjectFilePath\F1Sub", 1)]
		[TestCase(@"GetAllProjectFilePath\F2Flat", 2)]
		[TestCase(@"GetAllProjectFilePath\F2FlatAndSub", 2)]
		[TestCase(@"GetAllProjectFilePath\F2Sub", 2)]
		public void TestGetAllProjectFilePath(string subPath, int expectedCount)
		{
			string path = string.Format(@"{0}\{1}", TestDataPath, subPath);

			List<string> result = testee.GetAllProjectFilePath(path).ToList();

			Assert.AreEqual(expectedCount, result.Count());
			Assert.IsTrue(result.All(p => p.Contains(".csproj")));
			Assert.IsTrue(result.All(File.Exists));
		}

		[Test]
		public void TestLoad()
		{
			string p1Csproj = "TestData\\p2.csprojfortest";
			testee.LoadProject(p1Csproj);
			testee.CheckOutputPath("", "", result);
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException), ExpectedMessage = "root",
			MatchType = MessageMatch.Contains)]
		public void TestParseLineWithArgumentNull()
		{
			testee.GetAllProjectFilePath(null);
		}

		[Test]
		[TestCase(@"Bad?Path*")]
		[TestCase(@"Not Existing Path")]
		[ExpectedException(typeof (ProjectFileException))]
		public void TestParseLineWithBadArgument(string path)
		{
			testee.GetAllProjectFilePath(path);
		}
	}
}