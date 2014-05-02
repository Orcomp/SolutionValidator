#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleProcessor.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Common
{
	#region using...
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using Catel.Logging;
	using CodeInspection;
	using Configuration;
	using DependencyInjection;
	using FolderStructure;
	using ProjectFile;
	using Properties;

	#endregion

	public class RuleProcessor
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

		private readonly RepositoryInfo _repositoryInfo;
		private readonly List<Rule> _rules;
		private List<ValidationResult> _allValidationResults;
		private int totalCheckCount;
		private int totalErrorCount;

		public RuleProcessor(string repoRootPath, SolutionValidatorConfigurationSection configuration, bool isReformatEnabled)
		{
			var projectFileHelper = Dependency.Resolve<IProjectFileHelper>();

			_rules = new List<Rule>();

			if (!Directory.Exists(repoRootPath))
			{
				Logger.Error(Resources.RuleProcessor_RuleProcessor_Repository_root_folder_does_not_exists, repoRootPath);
				return;
			}
			_repositoryInfo = new RepositoryInfo(repoRootPath);

			if (configuration.FolderStructure.Check)
			{
				var definitionFilePath = configuration.FolderStructure.EvaluatedDefinitionFilePath();
				if (File.Exists(definitionFilePath))
				{
					var fileSystemHelper = Dependency.Resolve<IFileSystemHelper>();
					var fileSystemRuleParser = new FileSystemRuleParser(fileSystemHelper);
					_rules.AddRange(fileSystemRuleParser.Parse(definitionFilePath));
				}
				else
				{
					throw new ParseException(
						string.Format(Resources.RuleProcessor_RuleProcessor_Folder_structure_definition_file_not_found,
							definitionFilePath), 0, 0);
				}
			}

			if (configuration.ProjectFile.OutputPath.Check)
			{
				var rule = new OutPutPathProjectFileRule(configuration.ProjectFile.OutputPath.Value, projectFileHelper);
				_rules.Add(rule);
			}

			if (configuration.ProjectFile.RequiredConfigurations.Check)
			{
				foreach (var requiredConfigurationName in
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

			if (configuration.CSharpFormatting.Check && isReformatEnabled)
			{
				var fileSystemHelper = Dependency.Resolve<IFileSystemHelper>();
				ReformatRule rule;
				var optionsFilePath = configuration.CSharpFormatting.EvaluatedOptionsFilePath();
				if (File.Exists(optionsFilePath))
				{
					rule = new ReformatRule(optionsFilePath, fileSystemHelper);
				}
				else
				{
					if (configuration.CSharpFormatting.IsDefaultOptionsFilePath)
					{
						rule = new ReformatRule(null, fileSystemHelper);
					}
					else
					{
						throw new SolutionValidatorException(
							string.Format(Resources.RuleProcessor_RuleProcessor_CSharp_formatting_check_was_set_to_true, optionsFilePath));
					}
				}
				_rules.Add(rule);
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
			_allValidationResults = new List<ValidationResult>();

			OnNotifyInfo(notify, Resources.RuleProcessor_Process_Checking_repository, _repositoryInfo.RepositoryRootPath);

			foreach (var rule in _rules)
			{
				try
				{
					var validationResult = rule.Validate(_repositoryInfo, notify);
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
			totalCheckCount += validationResult.CheckCount;
			totalErrorCount += validationResult.ErrorCount;
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