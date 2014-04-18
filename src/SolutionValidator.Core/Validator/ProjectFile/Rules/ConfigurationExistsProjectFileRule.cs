using System;
using System.Collections.Generic;
using System.Linq;
using SolutionValidator.Core.Infrastructure.Logging;
using SolutionValidator.Core.Properties;
using SolutionValidator.Core.Validator.Common;

namespace SolutionValidator.Core.Validator.ProjectFile.Rules
{
	public class ConfigurationExistsProjectFileRule : ProjectFileRule
	{
		private readonly string expectedConfiguration;

		public ConfigurationExistsProjectFileRule(string expectedConfiguration,
			IProjectFileHelper projectFileHelper,
			ILogger logger) : base(projectFileHelper, logger)
		{
			this.expectedConfiguration = expectedConfiguration;
		}

		protected override void DoValidation(ValidationResult result, RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
		{
			IEnumerable<string> configurationNames = projectFileHelper.GetConfigurations();
			if (configurationNames.Any(cn => String.CompareOrdinal(cn, expectedConfiguration) == 0))
			{
				result.AddResult(ResultLevel.Passed,
					string.Format(Resources.ConfigurationExistsProjectFileRule_Validate_Project_contains_expected_configuration,
						projectFileHelper.GetProjectShortName(),
						expectedConfiguration,
						projectFileHelper.GetProjectInfo(expectedConfiguration)), notify);
			}
			else
			{
				result.AddResult(ResultLevel.Error,
					string.Format("Project '{0}' does not contain expected configuration '{1}'. {2}",
						projectFileHelper.GetProjectShortName(),
						expectedConfiguration,
						projectFileHelper.GetProjectInfo(expectedConfiguration)), notify);
			}
		}
	}
}