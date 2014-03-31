namespace SolutionValidator.Core.Validator.Common
{
	public class ProjectInfo
	{
		public ProjectInfo(string solutionPath, string projectPath)
		{
			SolutionPath = solutionPath.Trim().Replace("/", "\\");
			ProjectFullPath = projectPath.Trim().Replace("/", "\\");
			;
		}

		public string SolutionPath { get; set; }
		public string ProjectFullPath { get; set; }
	}
}