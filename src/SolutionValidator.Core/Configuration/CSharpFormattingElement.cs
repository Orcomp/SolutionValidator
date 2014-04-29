// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CSharpFormattingElement.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SolutionValidator.Configuration
{
    using System.Configuration;
    using Catel.Logging;
    using Properties;

    [UsedImplicitly]
    public class CSharpFormattingElement : ConfigurationElement
    {
        #region Constants
        private const string OptionsFilePathAttributeName = "optionsFilePath";
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Properties
        [ConfigurationProperty(OptionsFilePathAttributeName, DefaultValue = "csharpformatting.xml")]
        public string OptionsFilePath
        {
            get { return (string)base[OptionsFilePathAttributeName]; }
        }

        [ConfigurationProperty(SolutionValidatorConfigurationSection.CheckAttributeName, DefaultValue = "true")]
        public bool Check
        {
            get { return (bool)base[SolutionValidatorConfigurationSection.CheckAttributeName]; }
        }
        #endregion
    }
}