using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SolutionValidator.Core.Properties;
using SolutionValidator.Core.Validator.Common;

namespace SolutionValidator.Core.Validator.FolderStructure.Rules
{
	public class FileRule : FileSystemRule
	{
		public FileRule(string relativePath, CheckType checkType, IFileSystemHelper fileSystemHelper)
			: base(relativePath, checkType, fileSystemHelper)
		{
		}

		public override ValidationResult Validate(RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
		{
			var result = new ValidationResult(this);
			string searchPattern = RelativePath.Split('\\').Last();
			string folderPattern = RelativePath.Replace(searchPattern, "");

			var foldersToCheck = new List<string>();


			if (!IsRecursive)
			{
				string folder = Path.Combine(repositoryInfo.RootPath, folderPattern);
				foldersToCheck.Add(folder.TrimEnd('\\'));
			}
			else
			{
				foldersToCheck.AddRange(FileSystemHelper.GetFolders(repositoryInfo.RootPath, folderPattern));
			}

			bool exist = false;
			foreach (string folder in foldersToCheck)
			{
				exist = exist || FileSystemHelper.Exists(folder, searchPattern);
			}


			string message;
			if (!exist && CheckType == CheckType.MustExist || exist && CheckType == CheckType.MustNotExist)
			{
				message = string.Format("File '{0}' {1}.", RelativePath, exist ? Resources.FileRule_Validate_exists__This_file_should_not_exist_ : Resources.FileRule_Validate_does_not_exist__This_file_must_exist_);
				result.AddResult(ResultLevel.Error, message);
				return result;

			}
			message = string.Format("File '{0}' {1}.", RelativePath, exist ? Resources.FileRule_Validate_exists_ : Resources.FileRule_Validate_does_not_exist_);
			result.AddResult(ResultLevel.Passed, message);
			return result;
		}
	}
}