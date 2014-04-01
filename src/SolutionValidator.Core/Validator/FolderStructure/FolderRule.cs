using System.Linq;
using SolutionValidator.Core.Infrastructure.DependencyInjection;
using SolutionValidator.Core.Validator.Common;

namespace SolutionValidator.Core.Validator.FolderStructure
{
	public class FolderRule : FileSystemRule
	{
		public FolderRule(string relativePath, CheckType checkType, IFileSystemHelper fileSystemHelper) : base(relativePath, checkType, fileSystemHelper )
		{
		}

		public override ValidationResult Validate(ProjectInfo projectInfo)
		{
			var result = new ValidationResult(true, ToString());

			bool exist = FileSystemHelper.GetFolders(projectInfo.ProjectFullPath, RelativePath).Any();

			if (!exist && CheckType == CheckType.MustExist || exist && CheckType == CheckType.MustNotExist)
			{
				result.IsValid = false;
			}
			return result;
		}
	}
}