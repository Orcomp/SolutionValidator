// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckForValueProjectFileRule.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Validator.ProjectFile.Rules
{
    using System;
    using Common;

    public class CheckForValueProjectFileRule : ProjectFileRule
	{
		private readonly string propertyName;
		private readonly string value;

		public CheckForValueProjectFileRule(string propertyName, string value, IProjectFileHelper projectFileHelper)
			: base(projectFileHelper)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				propertyName = EmptyPropertyName;
			}

			this.propertyName = propertyName;
			this.value = value;
		}

		protected override void DoValidation(ValidationResult result, RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
		{
			var propertyValue = projectFileHelper.GetPropertyValue(propertyName);
			if (string.CompareOrdinal(propertyValue, value) == 0 && propertyName != EmptyPropertyName)
			{
				result.AddResult(ResultLevel.Passed,
					string.Format("Property '{0}' in project {1} has the expected value. ('{2}'). {3}",
						propertyName,	
						projectFileHelper.GetProjectShortName(),
						propertyValue,
						projectFileHelper.GetProjectInfo()), notify);
			}
			else
			{
				result.AddResult(ResultLevel.Error,
					string.Format("Property '{0}' in project {1} has unexpected value. Was '{2}' but expected {3}. {4}",
						propertyName,
						projectFileHelper.GetProjectShortName(),
						propertyValue,
						value,
						projectFileHelper.GetProjectInfo()), notify);
			}
		}
	}
}