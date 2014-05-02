#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectFileHelperTest.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Tests.Validator.ProjectFile
{
	#region using...
	using System;
	using System.IO;
	using System.Linq;
	using Common;
	using NUnit.Framework;
	using SolutionValidator.ProjectFile;

	#endregion

	[TestFixture]
	public class ProjectFileHelperTest
	{
		[SetUp]
		public void SetUp()
		{
			testee = new ProjectFileHelper("*.csprojfortest");
			result = new ValidationResult(null);
		}

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
			var path = string.Format(@"{0}\{1}", TestDataPath, subPath);

			var result = testee.GetAllProjectFilePath(path).ToList();

			Assert.AreEqual(expectedCount, result.Count());
			Assert.IsTrue(result.All(p => p.Contains(".csproj")));
			Assert.IsTrue(result.All(File.Exists));
		}

		[Test]
		public void TestLoad()
		{
			var p1Csproj = "TestData\\p2.csprojfortest";
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