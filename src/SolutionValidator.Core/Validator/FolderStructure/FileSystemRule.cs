using System;
using System.Dynamic;
using SolutionValidator.Core.Validator.Common;

namespace SolutionValidator.Core.Validator.FolderStructure
{
	public abstract class FileSystemRule : Rule
	{
		public const string RecursionToken = "**";
		public const string FileWildCardToken = "*";
		// Not used currently: public const string OneLevelWildCardToken = "*";

		protected readonly CheckType CheckType;
		protected readonly bool IsRecursive;
		protected readonly string RelativePath;
		protected readonly IFileSystemHelper FileSystemHelper;

		protected FileSystemRule(string relativePath, CheckType checkType, IFileSystemHelper fileSystemHelper)
		{
			RelativePath = relativePath;
			CheckType = checkType;
			FileSystemHelper = fileSystemHelper;
			IsRecursive = relativePath.IndexOf(RecursionToken, StringComparison.InvariantCulture) != -1;
		}

		public override string ToString()
		{
			return string.Format("{0} Path: {1}, Recursive: {2}, CheckType: {3}", GetType().Name, RelativePath, IsRecursive,
				CheckType);
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
	}
}