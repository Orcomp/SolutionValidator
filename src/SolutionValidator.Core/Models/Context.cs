// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Context.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator
{
    using System;
    using System.IO;
    using Catel;
    using Catel.Data;
    using Configuration;
    using Models;

    public class Context : ModelBase
    {
        #region
        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class.
        /// </summary>
        /// <param name="repositoryRootPath">The repository root path.</param>
        /// <param name="configFilePath">The configuration file path.</param>
        public Context(string repositoryRootPath, string configFilePath = null)
        {
            Argument.IsNotNullOrWhitespace(() => repositoryRootPath);

            RepositoryRootPath = repositoryRootPath;
            ConfigFilePath = configFilePath;

            ValidateContext();

            var validatorContext = CreateValidatorContext();
            Initialize(validatorContext);
        }

        public Context(string repositoryRootPath, ValidatorContext validatorContext)
        {
            Argument.IsNotNullOrWhitespace(() => repositoryRootPath);
            Argument.IsNotNull(() => validatorContext);

            RepositoryRootPath = repositoryRootPath;
            ValidatorContext = validatorContext;

            ValidateContext();

            Initialize(validatorContext);
        }

        #endregion

        #region properties
        public string WorkingDirectory { get; set; }

        public string RepositoryRootPath { get; private set; }

        public ValidatorContext ValidatorContext { get; private set; }

        private string ConfigFilePath { get; set; }
        #endregion

        #region Methods
        private void Initialize(ValidatorContext validatorContext)
        {
            Argument.IsNotNull(() => validatorContext);

            // Note: decide what directory to use
            //WorkingDirectory = Environment.CurrentDirectory;
            WorkingDirectory = RepositoryRootPath;

            ValidatorContext = validatorContext;
        }

        /// <summary>
        /// Validates the context.
        /// </summary>
        /// <exception cref="SolutionValidatorException">
        /// Repository path '{0}' does not exist
        /// or
        /// Configuration file '{0}' does not exist
        /// </exception>
        public void ValidateContext()
        {
            var repositoryPath = Path.GetFullPath(RepositoryRootPath);

            if (!Directory.Exists(repositoryPath))
            {
                throw new SolutionValidatorException("Repository path '{0}' does not exist", repositoryPath);
            }

            if (!string.IsNullOrWhiteSpace(ConfigFilePath) && !string.Equals(ConfigFilePath, SolutionValidatorEnvironment.ConfigFilePathDefaultValue))
            {
                ConfigFilePath = Path.GetFullPath(ConfigFilePath);

                if (!File.Exists(ConfigFilePath))
                {
                    throw new SolutionValidatorException("Configuration file '{0}' does not exist", ConfigFilePath);
                }
            }
        }

        private ValidatorContext CreateValidatorContext()
        {
            if (!string.IsNullOrWhiteSpace(ConfigFilePath))
            {
                var configuration = ConfigurationHelper.Load(ConfigFilePath);
                return configuration.ToContext();
            }

            return new ValidatorContext();
        }
        #endregion
    }
}