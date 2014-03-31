using System.Collections.Generic;

namespace SolutionValidator.Core.Validator.FolderStructure
{
	public interface IFileSystemHelper
	{
		bool Exists(string folder, string searchPattern = null);
		IEnumerable<string> GetFolders(string root, string pattern);
	}
}