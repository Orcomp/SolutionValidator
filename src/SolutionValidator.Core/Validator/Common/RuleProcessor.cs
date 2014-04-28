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
    using Catel;
    using Catel.Logging;
    using Configuration;
    using Infrastructure.DependencyInjection;
    using Models;
    using ProjectFile;
    using ProjectFile.Rules;
    using Properties;
    using FolderStructure;

    public class RuleProcessor
    {
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
        private readonly RepositoryInfo _repositoryInfo;
        private readonly List<Rule> _rules;
        private List<ValidationResult> _allValidationResults;
        private int _totalCheckCount;
        private int _totalErrorCount;

        public RuleProcessor(Context context)
        {
            Argument.IsNotNull(() => context);

            var projectFileHelper = Dependency.Resolve<IProjectFileHelper>();

            _rules = new List<Rule>();
            _repositoryInfo = new RepositoryInfo(context.RepositoryRootPath);

            var folderStructure = context.ValidatorContext.FolderStructure;
            if (folderStructure.Check)
            {
                var folderPath = context.GetFullPath(folderStructure.DefinitionFilePath);
                if (File.Exists(folderPath))
                {
                    var fileSystemRuleParser = new FileSystemRuleParser(Dependency.Resolve<IFileSystemHelper>());
                    _rules.AddRange(fileSystemRuleParser.Parse(context.ValidatorContext.FolderStructure.DefinitionFilePath));
                }
                else
                {
                    throw new ParseException(string.Format(Resources.RuleProcessor_RuleProcessor_Folder_structure_definition_file_not_found, folderPath), 0, 0);
                }
            }

            if (context.ValidatorContext.ProjectFile.CheckOutPutPath)
            {
                var rule = new OutPutPathProjectFileRule(context.ValidatorContext.ProjectFile.OutputPath, projectFileHelper);
                _rules.Add(rule);
            }

            if (context.ValidatorContext.ProjectFile.CheckRequiredConfigurations)
            {
                foreach (string requiredConfigurationName in context.ValidatorContext.ProjectFile.RequiredConfigurations)
                {
                    var rule = new ConfigurationExistsProjectFileRule(requiredConfigurationName, projectFileHelper);
                    _rules.Add(rule);
                }
            }

            if (context.ValidatorContext.ProjectFile.CheckIdentical)
            {
                foreach (var propertiesToMatch in context.ValidatorContext.ProjectFile.IdenticalChecks)
                {
                    var rule = new CheckIdenticalProjectFileRule(propertiesToMatch.PropertyName, propertiesToMatch.OtherPropertyName, projectFileHelper);
                    _rules.Add(rule);
                }
            }

            if (context.ValidatorContext.ProjectFile.CheckPropertyValues)
            {
                foreach (var propertyToCheck in context.ValidatorContext.ProjectFile.Properties)
                {
                    var rule = new CheckForValueProjectFileRule(propertyToCheck.Name, propertyToCheck.Value, projectFileHelper);
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