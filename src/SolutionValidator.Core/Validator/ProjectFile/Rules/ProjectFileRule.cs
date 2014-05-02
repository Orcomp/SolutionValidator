#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectFileRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.ProjectFile
{
	#region using...
	using System;
	using Catel.Logging;
	using Common;

	#endregion

	public abstract class ProjectFileRule : Rule
	{
		protected const string EmptyPropertyName = "<empty>";
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

		protected readonly IProjectFileHelper _projectFileHelper;

		protected ProjectFileRule(IProjectFileHelper projectFileHelper)
		{
			_projectFileHelper = projectFileHelper;
		}

		// Custom Whitebox sorry...
		//public dynamic UnitTestPeek
		//{
		//	get
		//	{
		//		dynamic result = new ExpandoObject();
		//		return result;
		//	}
		//}
		public override ValidationResult Validate(RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
		{
			var result = new ValidationResult(this);
			var projectFilePaths = _projectFileHelper.GetAllProjectFilePath(repositoryInfo.RepositoryRootPath);

			foreach (var projectFilePath in projectFilePaths)
			{
				try
				{
					_projectFileHelper.LoadProject(projectFilePath);
					DoValidation(result, repositoryInfo, notify);
				}
				catch (Exception ex)
				{
					Logger.Error(ex);
				}
			}
			return result;
		}

		protected abstract void DoValidation(ValidationResult result, RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null);
	}
}