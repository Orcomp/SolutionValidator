#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.FolderStructure
{
	#region using...
	using System;
	using System.Dynamic;
	using Common;

	#endregion

	public abstract class FileSystemRule : Rule
	{
		public const string RecursionToken = "**";
		public const string FileWildCardToken = "*";
		// Not used currently: public const string OneLevelWildCardToken = "*";

		protected readonly CheckType CheckType;
		protected readonly IFileSystemHelper FileSystemHelper;
		protected readonly bool IsRecursive;
		protected readonly string RelativePath;

		protected FileSystemRule(string relativePath, CheckType checkType, IFileSystemHelper fileSystemHelper)
		{
			RelativePath = relativePath.Replace("/", @"\");
			CheckType = checkType;
			FileSystemHelper = fileSystemHelper;
			IsRecursive = relativePath.IndexOf(RecursionToken, StringComparison.InvariantCulture) != -1;
		}

		// Custom Whitebox sorry...
		public dynamic UnitTestPeek
		{
			get
			{
				dynamic result = new ExpandoObject();
				result.CheckType = CheckType;
				result.IsRecursive = IsRecursive;
				return result;
			}
		}

		public override string ToString()
		{
			return string.Format("{0} Path: {1}, Recursive: {2}, CheckType: {3}", GetType().Name, RelativePath, IsRecursive,
				CheckType);
		}
	}
}