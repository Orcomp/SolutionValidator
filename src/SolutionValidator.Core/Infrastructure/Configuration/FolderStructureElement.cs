using System;
using System.Configuration;
using System.IO;
using SolutionValidator.Core.Infrastructure.DependencyInjection;
using SolutionValidator.Core.Infrastructure.Logging;
using SolutionValidator.Core.Properties;

namespace SolutionValidator.Core.Infrastructure.Configuration
{
	[UsedImplicitly]
	public class FolderStructureElement : ConfigurationElement
	{
		private const string DefinitionFilePathAttributeName = "definitionFilePath";

		[ConfigurationProperty(DefinitionFilePathAttributeName, DefaultValue = ".folderStructure")]
		public string DefinitionFilePath
		{
			get { return (string) base[DefinitionFilePathAttributeName]; }
		}

		[ConfigurationProperty(SolutionValidatorConfigurationSection.CheckAttributeName, DefaultValue = "true")]
		public bool Check
		{
			get { return (bool) base[SolutionValidatorConfigurationSection.CheckAttributeName]; }
		}

		public string EvaluatedDefinitionFilePath()
		{
			try
			{
				string folder = Path.GetDirectoryName(SolutionValidatorConfigurationSection.ConfigFilePath);
				string combine = Path.Combine(folder, DefinitionFilePath);
				return Path.GetFullPath(combine);
			}
			catch (Exception e)
			{
				Dependency.Resolve<ILogger>()
					.Error(e, Resources.FolderStructureElement_EvaluatedDefinitionFilePath_Error_getting_EvaluatedDefinitionFilePath);
				return
					Path.GetFullPath(
						Resources.FolderStructureElement_EvaluatedDefinitionFilePath_Error_getting_EvaluatedDefinitionFilePath2);
			}
		}
	}
}