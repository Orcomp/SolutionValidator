using System;
using System.Collections.Generic;
using SolutionValidator.Core.Infrastructure.Logging;
using SolutionValidator.Core.Validator.Common;

namespace SolutionValidator.Core.Validator.ProjectFile.Rules
{
	public class OutPutPathProjectFileRule : ProjectFileRule
	{
		private readonly string expectedOutputPath;

		public OutPutPathProjectFileRule(string expectedOutputPath, IProjectFileHelper projectFileHelper, ILogger logger)
			: base(projectFileHelper, logger)
		{
			this.expectedOutputPath = expectedOutputPath;
		}

		public override ValidationResult Validate(RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
		{
			var result = new ValidationResult(this);
			IEnumerable<string> projectFilePaths = projectFileHelper.GetAllProjectFilePath(repositoryInfo.RootPath);

			foreach (string projectFilePath in projectFilePaths)
			{
				try
				{
					projectFileHelper.LoadProject(projectFilePath);
					projectFileHelper.CheckOutputPath(repositoryInfo.RootPath, expectedOutputPath, result, notify);
				}
				catch (Exception e)
				{
					logger.Error(e);
				}
			}
			return result;
		}

		protected override void DoValidation(ValidationResult result, RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
		{
			projectFileHelper.CheckOutputPath(repositoryInfo.RootPath, expectedOutputPath, result, notify);
		}
	}
}