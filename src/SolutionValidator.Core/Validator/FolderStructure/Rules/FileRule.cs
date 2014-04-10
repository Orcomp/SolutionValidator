using System.Collections.Generic;
using System.IO;
using System.Linq;
using SolutionValidator.Core.Validator.Common;

namespace SolutionValidator.Core.Validator.FolderStructure.Rules
{
	public class FileRule : FileSystemRule
	{
		public FileRule(string relativePath, CheckType checkType, IFileSystemHelper fileSystemHelper)
			: base(relativePath, checkType, fileSystemHelper)
		{
		}

		public override ValidationResult Validate(RepositoryInfo repositoryInfo)
		{
			var result = new ValidationResult(true, ToString());
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


			if (!exist && CheckType == CheckType.MustExist || exist && CheckType == CheckType.MustNotExist)
			{
				result.IsValid = false;
			}
			return result;
		}
	}
}