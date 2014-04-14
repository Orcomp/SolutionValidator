using System;
using System.Dynamic;
using System.Text;
using SolutionValidator.Core.Validator.Common;
using SolutionValidator.Core.Validator.FolderStructure;

namespace SolutionValidator.Core.Validator.ProjectFile.Rules
{
	public class OutPutPathProjectFileRule : ProjectFileRule
	{
		private string expectedOutputPath;

		public OutPutPathProjectFileRule(string expectedOutputPath, IProjectFileHelper projectFileHelper) : base(projectFileHelper)
		{
			this.expectedOutputPath = expectedOutputPath;
		}

		public override ValidationResult Validate(RepositoryInfo repositoryInfo)
		{
			var result = new ValidationResult();
			var messages = new StringBuilder();

			var projectFilePaths = projectFileHelper.GetAllProjectFilePath(repositoryInfo.RootPath);

			foreach (var projectFilePath in projectFilePaths)
			{
				projectFileHelper.LoadProject(projectFilePath);
				result.InternalErrorCount += projectFileHelper.Check(repositoryInfo.RootPath, expectedOutputPath, messages);
			}
			result.Description = messages.ToString();
			result.IsValid = result.InternalErrorCount == 0;
			return result;
		}
	}
}