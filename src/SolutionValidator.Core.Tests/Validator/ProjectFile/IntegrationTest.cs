using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using SolutionValidator.Core.Validator.Common;
using SolutionValidator.Core.Validator.ProjectFile;
using SolutionValidator.Core.Validator.ProjectFile.Rules;

namespace SolutionValidator.Core.Tests.Validator.ProjectFile
{
	[TestFixture]
	public class IntegrationTest
	{
		#region Setup/Teardown

		[SetUp]
		public void SetUp()
		{
			helper = new ProjectFileHelper();
			messages = new StringBuilder();
			
		}

		[TearDown]
		public void TearDown()
		{
			try
			{
				if (tempRepoRoot == null)
				{
					return;
				}
				if (Directory.Exists(tempRepoRoot))
				{
					Directory.Delete(tempRepoRoot, true);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		#endregion

		private const string TestDataPath = "TestData";
		private ProjectFileHelper helper;
		private StringBuilder messages;
		private OutPutPathProjectFileRule rule;
		private string tempRepoRoot;

		[Test]
		[TestCase("output", "p2.csproj", @"level1\level2", null, null, 0, null)]
		[TestCase("notoutput", "p2.csproj", @"level1\level2", null, null, 2, "notoutput;release;debug")]
		[TestCase("output", "p2.csproj", @"level1", @"..\..", @"..", 0, null)]
		[TestCase("output", "p2.csproj", @"level1\level2\level3", @"..\..", @"..\..\..", 0, null)]
		[TestCase("output", "p2.csproj", @"level1\level2", @"..\..", @"c:\\", 2, "must be a relative path")]
		[TestCase("output", "p2.csproj", @"level1\level2", @"..\..\output\Debug", @"..\..\output\xxx", 1, "debug;xxx")]
		public void TestAndCount(string outputPath, string projectNames, string subFolder, string patchFrom, string patchTo, int expectedCount, string expectedContains)
		{
			// Arrange:
			tempRepoRoot = PrepareSolution(projectNames, subFolder, patchFrom, patchTo);
			rule = new OutPutPathProjectFileRule(outputPath, helper);

			
			// Act:
			var result = rule.Validate(new RepositoryInfo(tempRepoRoot));

			// Assert:
			Assert.AreEqual(expectedCount, result.InternalErrorCount, result.Description);
			Assert.AreEqual(expectedCount == 0, result.IsValid);
			if (!string.IsNullOrEmpty(expectedContains))
			{
				foreach (var expected in expectedContains.Split(';'))
				{
					Assert.IsTrue(result.Description.Contains(expected), string.Format("Expected contains: {0}, but was: {1}", expected, result.Description));
				}
			}
		}

		private string PrepareSolution(string projectNames, string subFolders, string patchFrom, string patchTo)
		{
			var tempFolder = TestUtils.CreateTempRootFolder();
			foreach (var projectName in projectNames.Split(';'))
			{
				var fileName = Path.GetFileName(projectName);
				var targetFolder = Path.Combine(tempFolder, subFolders);
				Directory.CreateDirectory(targetFolder);

				var targetFullName = Path.Combine(targetFolder, fileName);
				var sourceFullName = Path.GetFullPath(Path.Combine(TestDataPath, projectName));
				File.Copy(sourceFullName, targetFullName);
				PatchFile(targetFullName, patchFrom, patchTo);
			}
			return tempFolder;
		}

		private void PatchFile(string fileName, string patchFrom, string patchTo)
		{
			if (patchFrom == null || patchTo == null)
			{
				return;
			}
			string text = File.ReadAllText(fileName);
			text = text.Replace(patchFrom, patchTo);
			File.WriteAllText(fileName, text);
		}
	}
}