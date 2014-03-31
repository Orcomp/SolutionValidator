using System;
using SolutionValidator.Core.Validator.Common;

namespace SolutionValidator.Core.Validator.FolderStructure
{
	public abstract class FileSystemRule : Rule
	{
		public const string RecursionToken = "**";
		//Not used currently: public const string OneLevelWildCardToken = "*";

		protected readonly CheckType CheckType;
		protected readonly bool IsRecursive;
		protected readonly string RelativePath;

		protected FileSystemRule(string relativePath, CheckType checkType)
		{
			RelativePath = relativePath;
			CheckType = checkType;
			IsRecursive = relativePath.IndexOf(RecursionToken, StringComparison.InvariantCulture) != -1;
		}

		public override string ToString()
		{
			return string.Format("{0} Path: {1}, Recursive: {2}, CheckType: {3}", GetType().Name, RelativePath, IsRecursive,
				CheckType);
		}
	}
}