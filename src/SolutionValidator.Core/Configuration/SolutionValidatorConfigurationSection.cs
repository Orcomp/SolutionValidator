#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="SolutionValidatorConfigurationSection.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Configuration
{
	#region using...
	using System.Configuration;
	using Core.Infrastructure.Configuration;

	#endregion

	public class SolutionValidatorConfigurationSection : ConfigurationSection
	{
		#region Constants
		public const string SectionName = "solutionValidatorConfigSection";
		public const string CheckAttributeName = "check";

		private const string FolderStructureElementName = "folderStructure";
		private const string CSharpFormattingElementName = "csharpFormatting";

		private const string ProjectFileElementName = "projectFile";
		#endregion

		public static string ConfigFilePath { get; set; }

		#region Properties
		[ConfigurationProperty(FolderStructureElementName)]
		public FolderStructureElement FolderStructure
		{
			get { return (FolderStructureElement) base[FolderStructureElementName]; }
		}

		[ConfigurationProperty(CSharpFormattingElementName)]
		public CSharpFormattingElement CSharpFormatting
		{
			get { return (CSharpFormattingElement) base[CSharpFormattingElementName]; }
		}

		[ConfigurationProperty(ProjectFileElementName)]
		public ProjectFileElement ProjectFile
		{
			get { return (ProjectFileElement) base[ProjectFileElementName]; }
		}
		#endregion
	}
}