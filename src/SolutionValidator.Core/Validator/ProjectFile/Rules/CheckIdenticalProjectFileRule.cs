#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckIdenticalProjectFileRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.ProjectFile
{
	#region using...
	using System;
	using Common;
	using Properties;

	#endregion

	public class CheckIdenticalProjectFileRule : ProjectFileRule
	{
		#region Fields
		private readonly string _otherPropertyName;
		private readonly string _propertyName;
		#endregion

		#region Constructors
		public CheckIdenticalProjectFileRule(string propertyName, string otherPropertyName, IProjectFileHelper projectFileHelper) : base(projectFileHelper)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				propertyName = EmptyPropertyName;
			}

			if (string.IsNullOrEmpty(otherPropertyName))
			{
				otherPropertyName = EmptyPropertyName;
			}

			_propertyName = propertyName;
			_otherPropertyName = otherPropertyName;
		}
		#endregion

		#region Methods
		protected override void DoValidation(ValidationResult result, RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
		{
			var propertyValue = _projectFileHelper.GetPropertyValue(_propertyName);
			var otherPropertyValue = _projectFileHelper.GetPropertyValue(_otherPropertyName);

			if (string.CompareOrdinal(propertyValue, otherPropertyValue) == 0 && propertyValue.Length > 0)
			{
				result.AddResult(ResultLevel.Passed,
					string.Format(Resources.CheckIdenticalProjectFileRule_DoValidation_Properties_in_project_are_identical,
						_projectFileHelper.GetProjectShortName(),
						_propertyName,
						_otherPropertyName,
						propertyValue,
						_projectFileHelper.GetProjectInfo()), notify);
			}
			else
			{
				result.AddResult(ResultLevel.Invalid,
					string.Format(Resources.CheckIdenticalProjectFileRule_DoValidation_Properties_in_project_are_not_identical,
						_projectFileHelper.GetProjectShortName(),
						_propertyName,
						_otherPropertyName,
						propertyValue,
						otherPropertyValue,
						_projectFileHelper.GetProjectInfo()), notify);
			}
		}
		#endregion
	}
}