using System;
using System.Collections.Generic;
using SolutionValidator.Core.Validator.Common;

namespace SolutionValidator.Core.Validator.ProjectFile
{
	public interface IProjectFileHelper
	{
		IEnumerable<string> GetAllProjectFilePath(string root);
		void LoadProject(string path);

		void CheckOutputPath(string repoRoot,
			string expectedOutputPath,
			ValidationResult result,
			Action<ValidationResult> notify = null);

		IEnumerable<string> GetConfigurations();
		string GetProjectInfo(string configuration = "N/A");
		string GetProjectShortName();
		string GetPropertyValue(string propertyName);
	}
}