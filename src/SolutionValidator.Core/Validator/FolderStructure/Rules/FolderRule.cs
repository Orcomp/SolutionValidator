using System;
using System.Linq;
using SolutionValidator.Core.Properties;
using SolutionValidator.Core.Validator.Common;

namespace SolutionValidator.Core.Validator.FolderStructure.Rules
{
	public class FolderRule : FileSystemRule
	{
		public FolderRule(string relativePath, CheckType checkType, IFileSystemHelper fileSystemHelper)
			: base(relativePath, checkType, fileSystemHelper)
		{
		}

		public override ValidationResult Validate(RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
		{
			var result = new ValidationResult(this);

			bool exist = FileSystemHelper.GetFolders(repositoryInfo.RootPath, RelativePath).Any();

			string message;
			if (!exist && CheckType == CheckType.MustExist || exist && CheckType == CheckType.MustNotExist)
			{
				message = string.Format("Folder '{0}' {1}.", RelativePath, exist ? Resources.FolderRule_Validate_exists_This_folder_should_not_exist : Resources.FolderRule_Validate_does_not_exist_This_folder_must_exist);
				result.AddResult(ResultLevel.Error, message);
				return result;
			}
			message = string.Format("Folder '{0}' {1}.", RelativePath, exist ? Resources.FolderRule_Validate_exists : Resources.FolderRule_Validate_does_not_exist);
			result.AddResult(ResultLevel.Passed, message);
			return result;
		}
	}
}