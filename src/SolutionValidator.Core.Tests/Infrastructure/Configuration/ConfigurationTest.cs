using System.IO;
using NUnit.Framework;
using SolutionValidator.Core.Infrastructure.Configuration;

namespace SolutionValidator.Core.Tests.Infrastructure.Configuration
{
	[TestFixture]
	public class ConfigurationTest
	{
		private const string TestFolder = @"TestData\TestConfigurations";

		[Test]
		public void LoadFullFromAppConfig()
		{
			var configuration = ConfigurationHelper.Load();
			CheckCustomConfiguration(configuration);
		}

		[Test]
		[TestCase("Empty1.config")]
		[TestCase("Empty2.config")]
		[TestCase("Empty3.config")]
		[TestCase("Empty4.config")]
		public void LoadEmpty(string configFileName)
		{
			var configuration = ConfigurationHelper.Load(string.Format(@"{0}\{1}", TestFolder, configFileName));
			var x = configuration.FolderStructure.EvaluatedDefinitionFilePath();
			CheckDefaultConfiguration(configuration);

		}

		[Test]
		[TestCase("Empty1.config", TestFolder + @"\.folderStructure")]
		[TestCase("Full.config", TestFolder + @"\definitionFilePath")]
		[TestCase(null, "definitionFilePath")]
		public void TestEvaluatedDefinitionFilePath(string configFileName, string expected )
		{
			var configuration = ConfigurationHelper.Load(configFileName == null ? null : string.Format(@"{0}\{1}", TestFolder, configFileName));
			var actualFullPath = configuration.FolderStructure.EvaluatedDefinitionFilePath();
			var expectedFullPath = Path.Combine(Directory.GetCurrentDirectory(), expected);
			Assert.AreEqual(expectedFullPath.ToLower(), actualFullPath.ToLower());
		}

		[Test]
		public void TestNotExist()
		{
			var configuration = ConfigurationHelper.Load("Not Existing File Name");
			CheckDefaultConfiguration(configuration);
		}

		private void CheckDefaultConfiguration(SolutionValidatorConfigurationSection configuration)
		{
			Assert.AreEqual(".folderStructure", configuration.FolderStructure.DefinitionFilePath);
			Assert.IsTrue(configuration.FolderStructure.Check);
			Assert.AreEqual("output", configuration.ProjectFile.outputPath.Value);
			Assert.IsTrue(configuration.ProjectFile.outputPath.Check);
		}

		private void CheckCustomConfiguration(SolutionValidatorConfigurationSection configuration)
		{
			Assert.AreEqual("definitionFilePath", configuration.FolderStructure.DefinitionFilePath);
			Assert.IsFalse(configuration.FolderStructure.Check);
			Assert.AreEqual("outputPath", configuration.ProjectFile.outputPath.Value);
			Assert.IsFalse(configuration.ProjectFile.outputPath.Check);
		}

		//[Test]
		//public void LoadConfigurationFull()
		//{
		//	var configuration = ConfigurationHelper.Load("TestConfigurations\\Full.config");
		//	Assert.IsTrue(configuration.DisplayUI);

		//	Assert.AreEqual(2, configuration.SearchFolders.Count);
		//	Assert.AreEqual("SFI1", configuration.SearchFolders[0].Include);
		//	Assert.AreEqual("SFE1", configuration.SearchFolders[0].Exclude);
		//	Assert.AreEqual("SF1", configuration.SearchFolders[0].Folder);
		//	Assert.AreEqual("SFI2", configuration.SearchFolders[1].Include);
		//	Assert.AreEqual("SFE2", configuration.SearchFolders[1].Exclude);
		//	Assert.AreEqual("SF2", configuration.SearchFolders[1].Folder);

		//	Assert.AreEqual(2, configuration.ImplementationFilters.Count);
		//	Assert.AreEqual("IFI1", configuration.ImplementationFilters[0].Include);
		//	Assert.AreEqual("IFE1", configuration.ImplementationFilters[0].Exclude);
		//	Assert.AreEqual("IFI2", configuration.ImplementationFilters[1].Include);
		//	Assert.AreEqual("IFE2", configuration.ImplementationFilters[1].Exclude);

		//	Assert.AreEqual(2, configuration.TestCaseFilters.Count);
		//	Assert.AreEqual("TFI1", configuration.TestCaseFilters[0].Include);
		//	Assert.AreEqual("TFE1", configuration.TestCaseFilters[0].Exclude);
		//	Assert.AreEqual("TFI2", configuration.TestCaseFilters[1].Include);
		//	Assert.AreEqual("TFE2", configuration.TestCaseFilters[1].Exclude);
		//}

		//[Test]
		//public void LoadConfigurationEmpty()
		//{
		//	var configuration = ConfigurationHelper.Load("TestConfigurations\\Empty.config");
		//	Assert.IsFalse(configuration.DisplayUI);

		//	Assert.AreEqual(1, configuration.SearchFolders.Count);
		//	Assert.AreEqual(0, configuration.ImplementationFilters.Count);
		//	Assert.AreEqual(0, configuration.TestCaseFilters.Count);
		//}

		//[Test]
		//public void LoadConfigurationMissingSection()
		//{
		//	var configuration = ConfigurationHelper.Load("TestConfigurations\\MissingSection.config");
		//	Assert.IsFalse(configuration.DisplayUI);

		//	Assert.AreEqual(1, configuration.SearchFolders.Count);
		//	Assert.AreEqual(0, configuration.ImplementationFilters.Count);
		//	Assert.AreEqual(0, configuration.TestCaseFilters.Count);
		//}

		//[Test]
		//public void LoadConfigurationMissingDefinition()
		//{
		//	var configuration = ConfigurationHelper.Load("TestConfigurations\\MissingDefinition.config");
		//	Assert.AreEqual(1, configuration.SearchFolders.Count);
		//	Assert.AreEqual(0, configuration.ImplementationFilters.Count);
		//	Assert.AreEqual(0, configuration.TestCaseFilters.Count);

		//}
	}
}
