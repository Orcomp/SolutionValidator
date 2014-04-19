namespace SolutionValidator.Core.Infrastructure.Configuration
{
    using Catel.Logging;
    using Properties;
    using System;
    using System.Configuration;
    using System.IO;
    using SolutionValidator.Properties;

    [UsedImplicitly]
	public class FolderStructureElement : ConfigurationElement
	{
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

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
                Logger.Error(e, Resources.FolderStructureElement_EvaluatedDefinitionFilePath_Error_getting_EvaluatedDefinitionFilePath);
				return
					Path.GetFullPath(
						Resources.FolderStructureElement_EvaluatedDefinitionFilePath_Error_getting_EvaluatedDefinitionFilePath2);
			}
		}
	}
}