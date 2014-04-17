using System;
using System.Collections.Generic;
using System.Text;
using SolutionValidator.Core.Validator.Common;

namespace SolutionValidator.Core.Validator.ProjectFile
{
	public interface IProjectFileHelper
	{
		IEnumerable<string> GetAllProjectFilePath(string root);
		void LoadProject(string path);
		void CheckOutputPath(string repoRoot, string expectedOutputPath, ValidationResult result, Action<ValidationResult> notify = null);
	}
}