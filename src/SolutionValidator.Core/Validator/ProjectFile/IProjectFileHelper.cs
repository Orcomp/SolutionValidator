using System.Collections.Generic;
using System.Text;

namespace SolutionValidator.Core.Validator.ProjectFile
{
	public interface IProjectFileHelper
	{
		IEnumerable<string> GetAllProjectFilePath(string root);
		void LoadProject(string path);
		int Check(string repoRoot, string expectedOutputPath, StringBuilder messages);
		bool Modify(string expectedOutputPath);
	}
}