// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Configuration
{
    using Catel;
    using Models;

    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Create a <see cref="ValidatorContext"/> instance from a <see cref="ValidatorContext"/>.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        /// <returns>A new <see cref="ValidatorContext"/> instance.</returns>
        public static ValidatorContext ToContext(this SolutionValidatorConfigurationSection configuration)
        {
            Argument.IsNotNull(() => configuration);

            var validatorContext = new ValidatorContext();

            if (configuration.FolderStructure != null)
            {
                CreateFolderStructureContext(configuration, validatorContext);
            }

            if (configuration.ProjectFile != null)
            {
                CreateProjectFileContext(configuration, validatorContext);
            }
            
            return validatorContext;
        }

        private static void CreateFolderStructureContext(SolutionValidatorConfigurationSection configuration, ValidatorContext validatorContext)
        {
            Argument.IsNotNull(() => configuration);
            Argument.IsNotNull(() => validatorContext);

            validatorContext.FolderStructure.DefinitionFilePath = configuration.FolderStructure.DefinitionFilePath;
            validatorContext.FolderStructure.Check = configuration.FolderStructure.Check;
        }

        private static void CreateProjectFileContext(SolutionValidatorConfigurationSection configuration, ValidatorContext validatorContext)
        {
            Argument.IsNotNull(() => configuration);
            Argument.IsNotNull(() => validatorContext);

            validatorContext.ProjectFile.CheckOutPutPath = configuration.ProjectFile.OutputPath.Check;
            validatorContext.ProjectFile.OutputPath = configuration.ProjectFile.OutputPath.Value;
            validatorContext.ProjectFile.CheckRequiredConfigurations = configuration.ProjectFile.RequiredConfigurations.Check;
            validatorContext.ProjectFile.CheckIdentical = configuration.ProjectFile.CheckIdentical.Check;
            validatorContext.ProjectFile.CheckPropertyValues = configuration.ProjectFile.CheckForValue.Check;

            if (configuration.ProjectFile.RequiredConfigurations != null)
            {
                foreach (ConfigurationNameElement configurationNameElement in configuration.ProjectFile.RequiredConfigurations)
                {
                    validatorContext.ProjectFile.RequiredConfigurations.Add(configurationNameElement.Name);
                }
            }

            if (configuration.ProjectFile.CheckIdentical != null)
            {
                foreach (PropertiesToMatchElement propertiesToMatchElement in configuration.ProjectFile.CheckIdentical)
                {
                    validatorContext.ProjectFile.IdenticalChecks.Add(new IdenticalCheck() 
                        {PropertyName = propertiesToMatchElement.PropertyName, OtherPropertyName = propertiesToMatchElement.OtherPropertyName});
                }
            }

            if (configuration.ProjectFile.CheckForValue != null)
            {
                foreach (PropertyToCheckElement propertyToCheckElement in configuration.ProjectFile.CheckForValue)
                {
                    validatorContext.ProjectFile.Properties.Add(new Property() {Name = propertyToCheckElement.PropertyName, Value = propertyToCheckElement.Value});
                }
            }            
        }        
    }
}