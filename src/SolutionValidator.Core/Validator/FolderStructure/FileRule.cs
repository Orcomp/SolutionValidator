using System.Collections.Generic;
using System.Linq;
using SolutionValidator.Core.Infrastructure.DependencyInjection;
using SolutionValidator.Core.Validator.Common;

namespace SolutionValidator.Core.Validator.FolderStructure
{
	public class FileRule : FileSystemRule
	{
		public FileRule(string relativePath, CheckType checkType) : base(relativePath, checkType)
		{
		}

		public override ValidationResult Validate(ProjectInfo projectInfo)
		{
			var result = new ValidationResult(true, ToString());
			string searchPattern = RelativePath.Split('\\').Last();
			string folderPattern = RelativePath.Replace(searchPattern, "");

			var foldersToCheck = new List<string>();
			var fileSystemHelper = Dependency.Resolve<FileSystemHelper>();

			if (!IsRecursive)
			{
				foldersToCheck.Add(folderPattern.TrimEnd('\\'));
			}
			else
			{
				fileSystemHelper.GetFolders(projectInfo.ProjectFullPath, folderPattern);
			}

			bool exist = false;
			foreach (string folder in foldersToCheck)
			{
				exist = exist || fileSystemHelper.Exists(folder, searchPattern);
			}


			if (!exist && CheckType == CheckType.MustExist || exist && CheckType == CheckType.MustNotExist)
			{
				result.IsValid = false;
			}
			return result;
		}
	}
}