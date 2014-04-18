using System.Configuration;

namespace SolutionValidator.Core.Infrastructure.Configuration
{
	public class SolutionValidatorConfigurationSection : ConfigurationSection
	{
		public const string SectionName = "solutionValidatorConfigSection";
		public const string CheckAttributeName = "check";


		private const string FolderStructureElementName = "folderStructure";

		private const string ProjectFileElementName = "projectFile";
		public static string ConfigFilePath { get; set; }

		[ConfigurationProperty(FolderStructureElementName)]
		public FolderStructureElement FolderStructure
		{
			get { return (FolderStructureElement) base[FolderStructureElementName]; }
		}

		[ConfigurationProperty(ProjectFileElementName)]
		public ProjectFileElement ProjectFile
		{
			get { return (ProjectFileElement) base[ProjectFileElementName]; }
		}
	}
}