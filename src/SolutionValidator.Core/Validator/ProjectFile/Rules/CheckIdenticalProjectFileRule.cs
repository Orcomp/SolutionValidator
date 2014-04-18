using System;
using SolutionValidator.Core.Infrastructure.Logging;
using SolutionValidator.Core.Properties;
using SolutionValidator.Core.Validator.Common;

namespace SolutionValidator.Core.Validator.ProjectFile.Rules
{
	public class CheckIdenticalProjectFileRule : ProjectFileRule
	{
		private readonly string propertyName;
		private readonly string otherPropertyName;

		public CheckIdenticalProjectFileRule(string propertyName, string otherPropertyName, IProjectFileHelper projectFileHelper, ILogger logger) : base(projectFileHelper, logger)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				propertyName = EmptyPropertyName;
			}
			if (string.IsNullOrEmpty(otherPropertyName))
			{
				otherPropertyName = EmptyPropertyName;
			}

			this.propertyName = propertyName;
			this.otherPropertyName = otherPropertyName;
		}

		protected override void DoValidation(ValidationResult result, RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
		{
			var propertyValue = projectFileHelper.GetPropertyValue(propertyName);
			var otherPropertyValue = projectFileHelper.GetPropertyValue(otherPropertyName);

			if (string.CompareOrdinal(propertyValue, otherPropertyValue) == 0 && propertyValue.Length > 0)
			{
				result.AddResult(ResultLevel.Passed,
					string.Format(Resources.CheckIdenticalProjectFileRule_DoValidation_Properties_in_project_are_identical,
						projectFileHelper.GetProjectShortName(),
						propertyName,
						otherPropertyName,
						propertyValue,
						projectFileHelper.GetProjectInfo()), notify);
			}
			else
			{
				result.AddResult(ResultLevel.Error,
					string.Format(Resources.CheckIdenticalProjectFileRule_DoValidation_Properties_in_project_are_not_identical,
						projectFileHelper.GetProjectShortName(),
						propertyName,
						otherPropertyName,
						propertyValue,
						otherPropertyValue,
						projectFileHelper.GetProjectInfo()), notify);
			}
		}
	}
}