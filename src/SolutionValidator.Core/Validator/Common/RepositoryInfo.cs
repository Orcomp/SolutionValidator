namespace SolutionValidator.Core.Validator.Common
{
	public class RepositoryInfo
	{
		public RepositoryInfo(string rootPath)
		{
			RootPath = rootPath.Trim().Replace("/", "\\");
		}

		public string RootPath { get; private set; }
	}
}