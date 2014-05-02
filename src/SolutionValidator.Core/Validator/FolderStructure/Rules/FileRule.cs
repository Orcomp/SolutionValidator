#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.FolderStructure
{
	#region using...
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using Common;
	using Properties;

	#endregion

	public class FileRule : FileSystemRule
	{
		public FileRule(string relativePath, CheckType checkType, IFileSystemHelper fileSystemHelper)
			: base(relativePath, checkType, fileSystemHelper)
		{
		}

		public override ValidationResult Validate(RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
		{
			var result = new ValidationResult(this);
			var searchPattern = RelativePath.Split('\\').Last();
			var folderPattern = RelativePath.Replace(searchPattern, string.Empty);

			var foldersToCheck = new List<string>();

			if (!IsRecursive)
			{
				var folder = Path.Combine(repositoryInfo.RepositoryRootPath, folderPattern);
				foldersToCheck.Add(folder.TrimEnd('\\'));
			}
			else
			{
				foldersToCheck.AddRange(FileSystemHelper.GetFolders(repositoryInfo.RepositoryRootPath, folderPattern));
			}

			var exist = false;

			foreach (var folder in foldersToCheck)
			{
				exist = exist || FileSystemHelper.Exists(folder, searchPattern);
			}

			string message;

			if (!exist && CheckType == CheckType.MustExist || exist && CheckType == CheckType.MustNotExist)
			{
				message = string.Format("File '{0}' {1}.", RelativePath, exist ? Resources.FileRule_Validate_exists__This_file_should_not_exist_
					: Resources.FileRule_Validate_does_not_exist__This_file_must_exist_);
				result.AddResult(ResultLevel.Invalid, message);
				return result;
			}

			message = string.Format("File '{0}' {1}.", RelativePath, exist ? Resources.FileRule_Validate_exists_ : Resources.FileRule_Validate_does_not_exist_);
			result.AddResult(ResultLevel.Passed, message);
			return result;
		}
	}
}