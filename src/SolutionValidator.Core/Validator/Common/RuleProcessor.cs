using System;
using System.Collections.Generic;
using System.IO;
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
		private int totalCheckCount;
		private int totalErrorCount;
		private ILogger logger;
		private readonly RepositoryInfo repositoryInfo;
		private SolutionValidatorConfigurationSection configuration;
		private readonly List<Rule> rules;
		private List<ValidationResult> allValidationResults;

		public RuleProcessor(string repoRootPath, SolutionValidatorConfigurationSection configuration)
		{
			logger = Dependency.Resolve<ILogger>();
			this.configuration = configuration;
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
					var fileSystemRuleParser = new FileSystemRuleParser(Dependency.Resolve<IFileSystemHelper>());
					rules.AddRange(fileSystemRuleParser.Parse(configuration.FolderStructure.DefinitionFilePath));
				}
				else
				{
					throw new ParseException(string.Format(Resources.RuleProcessor_RuleProcessor_Folder_structure_definition_file_not_found, configuration.FolderStructure.EvaluatedDefinitionFilePath()), 0, 0);
				}
			}

			if (configuration.ProjectFile.outputPath.Check)
			{
				var rule = new OutPutPathProjectFileRule(configuration.ProjectFile.outputPath.Value,
					Dependency.Resolve<IProjectFileHelper>());
				rules.Add(rule);
			}
		}

		public void Process(Action<ValidationResult> notify = null)
		{
			totalCheckCount = 0;
			totalErrorCount = 0;
			allValidationResults = new List<ValidationResult>();

			OnNotifyInfo(notify, Resources.RuleProcessor_Process_Checking_repository, repositoryInfo.RootPath);

			foreach (var rule in rules)
			{
				try
				{
					var validationResult = rule.Validate(repositoryInfo, notify);
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

		public int TotalCheckCount
		{
			get { return totalCheckCount; }
		}

		public int TotalErrorCount
		{
			get { return totalErrorCount; }
		}

	}
}