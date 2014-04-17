using System;
using System.IO;
using System.Linq;
using System.Text;
using Ninject;
using NUnit.Framework;
using SolutionValidator.Core.Infrastructure.DependencyInjection;
using SolutionValidator.Core.Infrastructure.Logging;
using SolutionValidator.Core.Infrastructure.Logging.Log4Net;
using SolutionValidator.Core.Validator.Common;
using SolutionValidator.Core.Validator.FolderStructure;
using SolutionValidator.Core.Validator.ProjectFile;
using SolutionValidator.Core.Validator.ProjectFile.Rules;

namespace SolutionValidator.Core.Tests.Validator.ProjectFile
{
	[TestFixture]
	public class IntegrationTest
	{
		#region Setup/Teardown

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			CreateKernel();
		}


		[SetUp]
		public void SetUp()
		{
			helper = new ProjectFileHelper();
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
		private OutPutPathProjectFileRule rule;
		private string tempRepoRoot;

		[Test]
		[TestCase("output", "p2.csproj", @"level1\level2", null, null, 0, null)]
		[TestCase("notoutput", "p2.csproj", @"level1\level2", null, null, 2, "notoutput;release;debug")]
		[TestCase("output", "p2.csproj", @"level1", @"..\..", @"..", 0, null)]
		[TestCase("output", "p2.csproj", @"level1\level2\level3", @"..\..", @"..\..\..", 0, null)]
		[TestCase("output", "p2.csproj", @"level1\level2", @"..\..", @"c:\\", 2, "must be a relative path")]
		[TestCase("output", "p2.csproj", @"level1\level2", @"..\..\output\Debug", @"..\..\output\xxx", 1, "debug;xxx")]
		[TestCase(@"output\custom", "p2.csproj", @"level1\level2", @"..\..\output", @"..\..\output\custom", 0, null)]
		public void TestAndCount(string outputPath, string projectNames, string subFolder, string patchFrom, string patchTo, int expectedCount, string expectedContains)
		{
			// Arrange:
			tempRepoRoot = PrepareSolution(projectNames, subFolder, patchFrom, patchTo);
			rule = new OutPutPathProjectFileRule(outputPath, helper);

			
			// Act:
			var result = rule.Validate(new RepositoryInfo(tempRepoRoot));

			// Assert:
			Assert.AreEqual(expectedCount, result.ErrorCount, result.RuleDescription);
			Assert.AreEqual(expectedCount == 0, result.IsValid);
			
			if (!string.IsNullOrEmpty(expectedContains))
			{
				foreach (var expected in expectedContains.Split(';'))
				{
					var errorMessages = string.Concat(result.Messages.Where(m=>m.ResultLevel == ResultLevel.Error).Select(m => m.Message)).Contains(expected);
					Assert.IsTrue(errorMessages, string.Format("Expected contains: {0}, but was: {1}", expected, result.RuleDescription));
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

		private static bool isInitialized;
		private static IKernel CreateKernel()
		{
			if (isInitialized)
			{
				isInitialized = true;
				return null;
			}
			var kernel = new StandardKernel();
			RegisterServices(kernel);
			Dependency.Initialize(new NinjectResolver(kernel));
			return kernel;
		}

		private static void RegisterServices(IKernel kernel)
		{
			kernel.Bind<ILogger>().To<Log4NetLogger>().InThreadScope();
		}

	}
}