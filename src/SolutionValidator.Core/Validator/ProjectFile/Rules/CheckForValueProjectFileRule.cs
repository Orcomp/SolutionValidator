#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckForValueProjectFileRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.ProjectFile
{
	#region using...
	using System;
	using Common;

	#endregion

	public class CheckForValueProjectFileRule : ProjectFileRule
	{
		#region Fields
		private readonly string _propertyName;
		private readonly string _value;
		#endregion

		#region Constructors
		public CheckForValueProjectFileRule(string propertyName, string value, IProjectFileHelper projectFileHelper)
			: base(projectFileHelper)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				propertyName = EmptyPropertyName;
			}

			_propertyName = propertyName;
			_value = value;
		}
		#endregion

		#region Methods
		protected override void DoValidation(ValidationResult result, RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
		{
			var propertyValue = _projectFileHelper.GetPropertyValue(_propertyName);
			if (string.CompareOrdinal(propertyValue, _value) == 0 && _propertyName != EmptyPropertyName)
			{
				result.AddResult(ResultLevel.Passed,
					string.Format("Property '{0}' in project {1} has the expected value. ('{2}'). {3}",
						_propertyName,
						_projectFileHelper.GetProjectShortName(),
						propertyValue,
						_projectFileHelper.GetProjectInfo()), notify);
			}
			else
			{
				result.AddResult(ResultLevel.NotPassed,
					string.Format("Property '{0}' in project {1} has unexpected value. Was '{2}' but expected {3}. {4}",
						_propertyName,
						_projectFileHelper.GetProjectShortName(),
						propertyValue,
						_value,
						_projectFileHelper.GetProjectInfo()), notify);
			}
		}
		#endregion
	}
}