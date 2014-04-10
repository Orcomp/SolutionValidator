using System.Collections.Generic;

namespace SolutionValidator.Core.Validator.ProjectFile
{
	public interface IProjectFileHelper
	{
		IEnumerable<string> GetAllProjectFilePath(string root);
		void LoadProject(string path);
		bool Check(string root, string expectedOutputPath);
		bool Modify(string expectedOutputPath);
	}
}