using System.Collections.Generic;
using System.Linq;
using SolutionValidator.Core.Validator.Common;

namespace SolutionValidator.Core.Validator.FolderStructure
{
	public class FileRule : FileSystemRule
	{
		public FileRule(string relativePath, CheckType checkType, IFileSystemHelper fileSystemHelper) : base(relativePath, checkType, fileSystemHelper )
		{
		}

		public override ValidationResult Validate(ProjectInfo projectInfo)
		{
			var result = new ValidationResult(true, ToString());
			string searchPattern = RelativePath.Split('\\').Last();
			string folderPattern = RelativePath.Replace(searchPattern, "");

			var foldersToCheck = new List<string>();
		

			if (!IsRecursive)
			{
				foldersToCheck.Add(folderPattern.TrimEnd('\\'));
			}
			else
			{
				foldersToCheck.AddRange(FileSystemHelper.GetFolders(projectInfo.ProjectFullPath, folderPattern));
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