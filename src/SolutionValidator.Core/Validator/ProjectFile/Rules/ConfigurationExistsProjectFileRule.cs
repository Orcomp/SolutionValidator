#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationExistsProjectFileRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.ProjectFile
{
	#region using...
	using System;
	using System.Linq;
	using Common;
	using Properties;

	#endregion

	public class ConfigurationExistsProjectFileRule : ProjectFileRule
	{
		private readonly string _expectedConfiguration;

		public ConfigurationExistsProjectFileRule(string expectedConfiguration, IProjectFileHelper projectFileHelper)
			: base(projectFileHelper)
		{
			_expectedConfiguration = expectedConfiguration;
		}

		protected override void DoValidation(ValidationResult result, RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
		{
			var configurationNames = _projectFileHelper.GetConfigurations();
			if (configurationNames.Any(cn => String.CompareOrdinal(cn, _expectedConfiguration) == 0))
			{
				result.AddResult(ResultLevel.Passed,
					string.Format(Resources.ConfigurationExistsProjectFileRule_Validate_Project_contains_expected_configuration,
						_projectFileHelper.GetProjectShortName(),
						_expectedConfiguration,
						_projectFileHelper.GetProjectInfo(_expectedConfiguration)), notify);
			}
			else
			{
				result.AddResult(ResultLevel.Invalid,
					string.Format("Project '{0}' does not contain expected configuration '{1}'. {2}",
						_projectFileHelper.GetProjectShortName(),
						_expectedConfiguration,
						_projectFileHelper.GetProjectInfo(_expectedConfiguration)), notify);
			}
		}
	}
}