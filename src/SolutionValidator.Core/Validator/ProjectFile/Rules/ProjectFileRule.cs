using System;
using System.Collections.Generic;
using SolutionValidator.Core.Infrastructure.Logging;
using SolutionValidator.Core.Validator.Common;

namespace SolutionValidator.Core.Validator.ProjectFile.Rules
{
	public abstract class ProjectFileRule : Rule
	{
		protected const string EmptyPropertyName = "<empty>";
		protected readonly IProjectFileHelper projectFileHelper;

		protected ProjectFileRule(IProjectFileHelper projectFileHelper, ILogger logger) : base(logger)
		{
			this.projectFileHelper = projectFileHelper;
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
			IEnumerable<string> projectFilePaths = projectFileHelper.GetAllProjectFilePath(repositoryInfo.RootPath);

			foreach (string projectFilePath in projectFilePaths)
			{
				try
				{
					projectFileHelper.LoadProject(projectFilePath);
					DoValidation(result, repositoryInfo, notify);
				}
				catch (Exception e)
				{
					logger.Error(e);
				}
			}
			return result;
		}

		protected abstract void DoValidation(ValidationResult result,
			RepositoryInfo repositoryInfo,
			Action<ValidationResult> notify = null);
	}
}