using System;
using System.Dynamic;
using System.Text;
using log4net.Repository.Hierarchy;
using SolutionValidator.Core.Infrastructure.DependencyInjection;
using SolutionValidator.Core.Infrastructure.Logging;
using SolutionValidator.Core.Validator.Common;
using SolutionValidator.Core.Validator.FolderStructure;

namespace SolutionValidator.Core.Validator.ProjectFile.Rules
{
	public class OutPutPathProjectFileRule : ProjectFileRule
	{
		private string expectedOutputPath;
		private ILogger logger;

		public OutPutPathProjectFileRule(string expectedOutputPath, IProjectFileHelper projectFileHelper) : base(projectFileHelper)
		{
			logger = Dependency.Resolve<ILogger>();
			this.expectedOutputPath = expectedOutputPath;
		}

		public override ValidationResult Validate(RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
		{
			var result = new ValidationResult(this);
			var projectFilePaths = projectFileHelper.GetAllProjectFilePath(repositoryInfo.RootPath);

			foreach (var projectFilePath in projectFilePaths)
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
	}
}