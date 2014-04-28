// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SolutionValidatorConfigurationSection.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SolutionValidator.Configuration
{
    using System.Configuration;

    public class SolutionValidatorConfigurationSection : ConfigurationSection
    {
        #region Constants
        public const string SectionName = "solutionValidatorConfigSection";
        public const string CheckAttributeName = "check";

        private const string FolderStructureElementName = "folderStructure";

        private const string ProjectFileElementName = "projectFile";
        #endregion

        #region Properties
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
        #endregion
    }
}