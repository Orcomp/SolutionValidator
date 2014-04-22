// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FolderStructureElement.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SolutionValidator.Configuration
{
    using System;
    using System.Configuration;
    using System.IO;
    using Catel.Logging;
    using Properties;

    [UsedImplicitly]
    public class FolderStructureElement : ConfigurationElement
    {
        #region Constants
        private const string DefinitionFilePathAttributeName = "definitionFilePath";

        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Properties
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
        #endregion

        #region Methods
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
        #endregion
    }
}