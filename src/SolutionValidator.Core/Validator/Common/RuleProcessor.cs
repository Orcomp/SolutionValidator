// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleProcessor.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Validator.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Catel.Logging;
    using Configuration;
    using Infrastructure.DependencyInjection;
    using ProjectFile;
    using ProjectFile.Rules;
    using Properties;
    using FolderStructure;

    public class RuleProcessor
    {
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        private readonly RepositoryInfo _repositoryInfo;
        private readonly List<Rule> _rules;
        private List<ValidationResult> _allValidationResults;
        private SolutionValidatorConfigurationSection _configuration;
        private int _totalCheckCount;
        private int _totalErrorCount;

        public RuleProcessor(string repoRootPath, SolutionValidatorConfigurationSection configuration)
        {
            _configuration = configuration;
			_repositoryInfo = new RepositoryInfo(repoRootPath);

            var projectFileHelper = Dependency.Resolve<IProjectFileHelper>();

            _rules = new List<Rule>();

            if (!Directory.Exists(repoRootPath))
            {
                Logger.Error(Resources.RuleProcessor_RuleProcessor_Repository_root_folder_does_not_exists, repoRootPath);
                return;
            }

            if (configuration.FolderStructure.Check)
            {
                if (File.Exists(configuration.FolderStructure.EvaluatedDefinitionFilePath()))
                {
                    var fileSystemRuleParser = new FileSystemRuleParser(Dependency.Resolve<IFileSystemHelper>());
                    _rules.AddRange(fileSystemRuleParser.Parse(configuration.FolderStructure.DefinitionFilePath));
                }
                else
                {
                    throw new ParseException(
                        string.Format(Resources.RuleProcessor_RuleProcessor_Folder_structure_definition_file_not_found,
                            configuration.FolderStructure.EvaluatedDefinitionFilePath()), 0, 0);
                }
            }

            if (configuration.ProjectFile.OutputPath.Check)
            {
                var rule = new OutPutPathProjectFileRule(configuration.ProjectFile.OutputPath.Value, projectFileHelper);
                _rules.Add(rule);
            }

            if (configuration.ProjectFile.RequiredConfigurations.Check)
            {
                foreach (string requiredConfigurationName in
                    configuration.ProjectFile.RequiredConfigurations.Cast<ConfigurationNameElement>().Select(e => e.Name))
                {
                    var rule = new ConfigurationExistsProjectFileRule(requiredConfigurationName, projectFileHelper);
                    _rules.Add(rule);
                }
            }

            if (configuration.ProjectFile.CheckIdentical.Check)
            {
                foreach (var propertiesToMatch in
                    configuration.ProjectFile.CheckIdentical.Cast<PropertiesToMatchElement>())
                {
                    var rule = new CheckIdenticalProjectFileRule(propertiesToMatch.PropertyName, propertiesToMatch.OtherPropertyName, projectFileHelper);
                    _rules.Add(rule);
                }
            }

            if (configuration.ProjectFile.CheckForValue.Check)
            {
                foreach (var propertyToCheck in
                    configuration.ProjectFile.CheckForValue.Cast<PropertyToCheckElement>())
                {
                    var rule = new CheckForValueProjectFileRule(propertyToCheck.PropertyName, propertyToCheck.Value, projectFileHelper);
                    _rules.Add(rule);
                }
            }
        }

        public int TotalCheckCount
        {
            get { return _totalCheckCount; }
        }

        public int TotalErrorCount
        {
            get { return _totalErrorCount; }
        }

        public void Process(Action<ValidationResult> notify = null)
        {
            _totalCheckCount = 0;
            _totalErrorCount = 0;
            _allValidationResults = new List<ValidationResult>();

            OnNotifyInfo(notify, Resources.RuleProcessor_Process_Checking_repository, _repositoryInfo.RootPath);

            foreach (Rule rule in _rules)
            {
                try
                {
                    ValidationResult validationResult = rule.Validate(_repositoryInfo, notify);
                    OnNotify(notify, validationResult);
                }
                catch (Exception e)
                {
                    Logger.Error(e, Resources.RuleProcessor_Process_Unexpected_error_while_processing_rule, rule);
                }
            }
        }

        private void OnNotify(Action<ValidationResult> notify, ValidationResult validationResult)
        {
            if (notify != null)
            {
                notify(validationResult);
            }
            _totalCheckCount += validationResult.CheckCount;
            _totalErrorCount += validationResult.ErrorCount;
            _allValidationResults.Add(validationResult);
        }

        private void OnNotifyInfo(Action<ValidationResult> notify, string message, params object[] args)
        {
            var validationResult = new ValidationResult(null);
            validationResult.AddResult(ResultLevel.Info, string.Format(message, args));
            _allValidationResults.Add(validationResult);
            if (notify != null)
            {
                notify(validationResult);
            }
        }

        public IEnumerable<ValidationResult> GetFinalResults()
        {
            return _allValidationResults;
        }
    }
}