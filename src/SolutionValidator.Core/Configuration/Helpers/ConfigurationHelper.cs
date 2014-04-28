// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationHelper.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Configuration
{
    using System;
    using System.Configuration;
    using System.Reflection;
    using Catel;
    using Models;

    /// <summary>
    ///     ConfigurationHelper is a simple static helper to ease loading SolutionValidator configuration
    /// </summary>
    public static class ConfigurationHelper
    {
        public static ValidatorContext CreateContext(string configFileName = null)
        {
            Argument.IsNotNullOrWhitespace(() => configFileName);

            var configSection = Load(configFileName);
            return configSection.ToContext();
        }

        /// <summary>
        ///     Loads the specified configuration from the given file name.
        /// </summary>
        /// <param name="configFileName">
        ///     Name of the configuration file. If not presented the standard
        ///     .NET config file will be loaded
        /// </param>
        /// <returns>The loaded SolutionValidatorConfigurationSection.</returns>
        public static SolutionValidatorConfigurationSection Load(string configFileName = null)
        {
            if (configFileName == null)
            {
                var defaultSection = (SolutionValidatorConfigurationSection)ConfigurationManager.GetSection(SolutionValidatorConfigurationSection.SectionName);
                return Check(defaultSection);
            }

            var configMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = configFileName
            };

            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            var section = (SolutionValidatorConfigurationSection) config.GetSection(SolutionValidatorConfigurationSection.SectionName);
            return Check(section);
        }

        /// <summary>
        ///     Checks the specified configuration for additional validation errors like invalid regex etc
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>SolutionValidatorConfigurationSection.</returns>
        private static SolutionValidatorConfigurationSection Check(SolutionValidatorConfigurationSection configuration)
        {
            if (configuration == null)
            {
                configuration = new SolutionValidatorConfigurationSection();
            }

            if (configuration.ProjectFile.RequiredConfigurations.Count == 0 && configuration.ProjectFile.RequiredConfigurations.Check)
            {
                configuration.ProjectFile.RequiredConfigurations.Add(new ConfigurationNameElement { Name = "Debug" });
                configuration.ProjectFile.RequiredConfigurations.Add(new ConfigurationNameElement { Name = "Release" });
            }

            if (configuration.ProjectFile.CheckIdentical.Count == 0 && configuration.ProjectFile.CheckIdentical.Check)
            {
				// These expectations are commented out because they will not satisfied after namespace refactorings
				// For example both SolutionValidator.Core and SolutionValidator.UI.Console root namespace is SolutionValidator
				// so obviously can not identical with AssemblyName (we end with two identical assembly name)
				// Also SolutionValidator.UI.Console's 
				
				//configuration.ProjectFile.CheckIdentical.Add(new PropertiesToMatchElement
				//{
				//	PropertyName = "AssemblyName",
				//	OtherPropertyName = "RootNamespace"
				//});
				//configuration.ProjectFile.CheckIdentical.Add(new PropertiesToMatchElement
				//{
				//	PropertyName = "AssemblyName",
				//	OtherPropertyName = "ProjectName"
				//});
            }

            if (configuration.ProjectFile.CheckForValue.Count == 0 && configuration.ProjectFile.CheckForValue.Check)
            {
                configuration.ProjectFile.CheckForValue.Add(new PropertyToCheckElement
                {
                    PropertyName = "AppDesignerFolder",
                    Value = "Properties"
                });

                // TODO: Check for more, such as "iPhoneSimulator"
                configuration.ProjectFile.CheckForValue.Add(new PropertyToCheckElement
                {
                    PropertyName = "Platform",
                    Value = "AnyCPU"
                });
            }

            return configuration;
        }
    }
}