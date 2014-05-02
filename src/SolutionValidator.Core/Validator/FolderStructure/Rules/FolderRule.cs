#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FolderRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.FolderStructure
{
	#region using...
	using System;
	using System.Linq;
	using Common;
	using Properties;

	#endregion

	public class FolderRule : FileSystemRule
	{
		public FolderRule(string relativePath, CheckType checkType, IFileSystemHelper fileSystemHelper)
			: base(relativePath, checkType, fileSystemHelper)
		{
		}

		public override ValidationResult Validate(RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
		{
			var result = new ValidationResult(this);
			var exist = FileSystemHelper.GetFolders(repositoryInfo.RepositoryRootPath, RelativePath).Any();
			string message;

			if (!exist && CheckType == CheckType.MustExist || exist && CheckType == CheckType.MustNotExist)
			{
				message = string.Format("Folder '{0}' {1}.", RelativePath, exist ? Resources.FolderRule_Validate_exists_This_folder_should_not_exist
					: Resources.FolderRule_Validate_does_not_exist_This_folder_must_exist);
				result.AddResult(ResultLevel.Invalid, message);
				return result;
			}
			message = string.Format("Folder '{0}' {1}.", RelativePath, exist ? Resources.FolderRule_Validate_exists : Resources.FolderRule_Validate_does_not_exist);
			result.AddResult(ResultLevel.Passed, message);

			return result;
		}
	}
}