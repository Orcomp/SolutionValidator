using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SolutionValidator.Core.Infrastructure.Configuration;
using SolutionValidator.Core.Infrastructure.DependencyInjection;
using SolutionValidator.Core.Infrastructure.Logging;
using SolutionValidator.Core.Properties;
using SolutionValidator.Core.Validator.FolderStructure;
using SolutionValidator.Core.Validator.ProjectFile;
using SolutionValidator.Core.Validator.ProjectFile.Rules;

namespace SolutionValidator.Core.Validator.Common
{
	public class RuleProcessor
	{
		private readonly ILogger logger;
		private readonly RepositoryInfo repositoryInfo;
		private readonly List<Rule> rules;
		private List<ValidationResult> allValidationResults;
		private SolutionValidatorConfigurationSection configuration;
		private int totalCheckCount;
		private int totalErrorCount;

		public RuleProcessor(string repoRootPath, SolutionValidatorConfigurationSection configuration, ILogger logger)
		{
			this.logger = logger;
			this.configuration = configuration;

			var projectFileHelper = Dependency.Resolve<IProjectFileHelper>();

			rules = new List<Rule>();

			if (!Directory.Exists(repoRootPath))
			{
				logger.Error(Resources.RuleProcessor_RuleProcessor_Repository_root_folder_does_not_exists, repoRootPath);
				return;
			}
			repositoryInfo = new RepositoryInfo(repoRootPath);

			if (configuration.FolderStructure.Check)
			{
				if (File.Exists(configuration.FolderStructure.EvaluatedDefinitionFilePath()))
				{
					var fileSystemRuleParser = new FileSystemRuleParser(Dependency.Resolve<IFileSystemHelper>(), logger);
					rules.AddRange(fileSystemRuleParser.Parse(configuration.FolderStructure.DefinitionFilePath));
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
				var rule = new OutPutPathProjectFileRule(
					configuration.ProjectFile.OutputPath.Value,
					projectFileHelper, logger);
				rules.Add(rule);
			}

			if (configuration.ProjectFile.RequiredConfigurations.Check)
			{
				foreach (string requiredConfigurationName in
					configuration.ProjectFile.RequiredConfigurations.Cast<ConfigurationNameElement>().Select(e => e.Name))
				{
					var rule = new ConfigurationExistsProjectFileRule(requiredConfigurationName, projectFileHelper, logger);
					rules.Add(rule);
				}
			}

			if (configuration.ProjectFile.CheckIdentical.Check)
			{
				foreach (var propertiesToMatch in
					configuration.ProjectFile.CheckIdentical.Cast<PropertiesToMatchElement>())
				{
					var rule = new CheckIdenticalProjectFileRule(propertiesToMatch.PropertyName, propertiesToMatch.OtherPropertyName, projectFileHelper, logger);
					rules.Add(rule);
				}
			}

			if (configuration.ProjectFile.CheckForValue.Check)
			{
				foreach (var propertyToCheck in
					configuration.ProjectFile.CheckForValue.Cast<PropertyToCheckElement>())
				{
					var rule = new CheckForValueProjectFileRule(propertyToCheck.PropertyName, propertyToCheck.Value, projectFileHelper, logger);
					rules.Add(rule);
				}
			}
		}

		public int TotalCheckCount
		{
			get { return totalCheckCount; }
		}

		public int TotalErrorCount
		{
			get { return totalErrorCount; }
		}

		public void Process(Action<ValidationResult> notify = null)
		{
			totalCheckCount = 0;
			totalErrorCount = 0;
			allValidationResults = new List<ValidationResult>();

			OnNotifyInfo(notify, Resources.RuleProcessor_Process_Checking_repository, repositoryInfo.RootPath);

			foreach (Rule rule in rules)
			{
				try
				{
					ValidationResult validationResult = rule.Validate(repositoryInfo, notify);
					OnNotify(notify, validationResult);
				}
				catch (Exception e)
				{
					logger.Error(e, Resources.RuleProcessor_Process_Unexpected_error_while_processing_rule, rule);
				}
			}
		}

		private void OnNotify(Action<ValidationResult> notify, ValidationResult validationResult)
		{
			if (notify != null)
			{
				notify(validationResult);
			}
			totalCheckCount += validationResult.CheckCount;
			totalErrorCount += validationResult.ErrorCount;
			allValidationResults.Add(validationResult);
		}

		private void OnNotifyInfo(Action<ValidationResult> notify, string message, params object[] args)
		{
			var validationResult = new ValidationResult(null);
			validationResult.AddResult(ResultLevel.Info, string.Format(message, args));
			allValidationResults.Add(validationResult);
			if (notify != null)
			{
				notify(validationResult);
			}
		}

		public IEnumerable<ValidationResult> GetFinalResults()
		{
			return allValidationResults;
		}
	}
}