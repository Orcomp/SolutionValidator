using System.Linq;
using SolutionValidator.Core.Validator.Common;

namespace SolutionValidator.Core.Validator.FolderStructure.Rules
{
	public class FolderRule : FileSystemRule
	{
		public FolderRule(string relativePath, CheckType checkType, IFileSystemHelper fileSystemHelper)
			: base(relativePath, checkType, fileSystemHelper)
		{
		}

		public override ValidationResult Validate(RepositoryInfo repositoryInfo)
		{
			var result = new ValidationResult(true, ToString());

			bool exist = FileSystemHelper.GetFolders(repositoryInfo.RootPath, RelativePath).Any();

			if (!exist && CheckType == CheckType.MustExist || exist && CheckType == CheckType.MustNotExist)
			{
				result.IsValid = false;
			}
			return result;
		}
	}
}