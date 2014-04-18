using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Evaluation;
using SolutionValidator.Core.Properties;
using SolutionValidator.Core.Validator.Common;

namespace SolutionValidator.Core.Validator.ProjectFile
{
	public class ProjectFileHelper : IProjectFileHelper
	{
		private const string SearchPattern = "*.csproj";
		private const string SpecialPropertyProjectName = "ProjectName";

		private string assemblyName;
		private ProjectCollection collection;
		private Project project;
		private string projectFileFullName;

		#region IProjectFileHelper Members

		public IEnumerable<string> GetAllProjectFilePath(string root)
		{
			if (root == null)
			{
				throw new ArgumentNullException("root");
			}
			var result = new List<string>();

			try
			{
				string rootFullPath = Path.GetFullPath(root);
				var directoryInfo = new DirectoryInfo(rootFullPath);
				FileInfo[] fileInfos = directoryInfo.GetFiles(SearchPattern, SearchOption.AllDirectories);
				result.AddRange(fileInfos.Select(fileInfo => fileInfo.FullName));
			}
			catch (Exception e)
			{
				throw new ProjectFileException(
					string.Format(Resources.ProjectFileHelper_GetAllProjectFilePath_GetAllProjectPath_was_called_with_bad_argument,
						root), e);
			}

			return result;
		}

		public void LoadProject(string path)
		{
			LoadProject(path, null);
		}


		public void CheckOutputPath(string repoRoot,
			string expectedOutputPath,
			ValidationResult result,
			Action<ValidationResult> notify = null)
		{
			assemblyName = project.GetPropertyValue("AssemblyName");

			foreach (string configurationName in GetConfigurations())
			{
				CheckOneOutputPath(configurationName, repoRoot, expectedOutputPath, result, notify);
			}
		}

		public IEnumerable<string> GetConfigurations()
		{
			return project.ConditionedProperties["Configuration"];
		}

		public string GetProjectInfo(string configuration = "N/A")
		{
			return string.Format("Project File: '{0}', Configuration '{1}'", projectFileFullName, configuration);
		}

		public string GetProjectShortName()
		{
			return Path.GetFileNameWithoutExtension(projectFileFullName);
		}

		public string GetPropertyValue(string propertyName)
		{
			if (propertyName == SpecialPropertyProjectName)
			{
				return GetProjectShortName();
			}
			return project.GetPropertyValue(propertyName);
		}


		#endregion

		// TODO: Performance tuning: Implement caching loaded projects
		private void LoadProject(string path, string configuration)
		{
			collection = new ProjectCollection {DefaultToolsVersion = "4.0"};
			projectFileFullName = Path.GetFullPath(path);
			if (configuration != null)
			{
				collection.SetGlobalProperty("Configuration", configuration);
			}
			project = collection.LoadProject(projectFileFullName);
		}

		private void CheckOneOutputPath(string configuration,
			string repoRoot,
			string expectedOutputPath,
			ValidationResult result,
			Action<ValidationResult> notify)
		{
			// Must reload the project to make the evaluated values in sync with 
			// the configuration under test:

			try
			{
				LoadProject(projectFileFullName, configuration);

				ProjectItem item = project.GetItems("_OutputPathItem").FirstOrDefault();
				string message;
				if (item == null)
				{
					message = string.Format(Resources.ProjectFileHelper_CheckOne_Can_not_get_output_path, GetProjectInfo(configuration));
					result.AddResult(ResultLevel.Error, message, notify);
					return;
				}
				string outputPath = item.EvaluatedInclude;
				if (Path.IsPathRooted(outputPath) || !outputPath.StartsWith("."))
				{
					message = string.Format(Resources.ProjectFileHelper_CheckOne_Output_path_must_be_a_relative_path, outputPath,
						GetProjectInfo(configuration));
					result.AddResult(ResultLevel.Error, message, notify);
					return;
				}
				//($repoRoot)\($expectedOutputPath)\($configuration)\($targetFrameworkVersion)\($projectName)

				repoRoot = repoRoot.Trim('\\');
				expectedOutputPath = expectedOutputPath.Trim('\\');
				string targetFrameworkVersion = GetTargetFrameworkVersion(project);

				// We are using the assembly name here for the sake of simplicity. However please note
				// that other rules are forcing project file name to be identical with assembly name and root namespace name

				string projectName = assemblyName;
				string projectFolder = Path.GetDirectoryName(projectFileFullName).Trim('\\');

				string expectedValue = string.Format(@"{0}\{1}\{2}\{3}\{4}", repoRoot, expectedOutputPath, configuration,
					targetFrameworkVersion, projectName).ToLower();

				string actualValue = Path.GetFullPath(Path.Combine(projectFolder, outputPath)).Trim('\\').ToLower();


				if (String.Compare(expectedValue, actualValue, StringComparison.Ordinal) != 0)
				{
					message = string.Format(Resources.ProjectFileHelper_CheckOne_Output_path_was_evaluated_to, actualValue,
						expectedValue, GetProjectInfo(configuration));
					result.AddResult(ResultLevel.Error, message, notify);
					return;
				}
				message = string.Format(
					Resources.ProjectFileHelper_CheckOneOutputPath_OutputPath_conforms_to_the_required_standards,
					GetProjectInfo(configuration));
				result.AddResult(ResultLevel.Passed, message, notify);
			}
			catch (ProjectFileException e)
			{
				result.AddResult(ResultLevel.Error, e.Message, notify);
			}
			catch (Exception e)
			{
				result.AddResult(ResultLevel.Error, string.Format("Unexpected exception: {0}", e.Message), notify);
			}
		}

		private string GetTargetFrameworkVersion(Project project)
		{
			try
			{
				string value = project.GetProperty("TargetFrameworkVersion").EvaluatedValue;
				return string.Format("NET{0}", value.Replace(".", "").Replace("v", ""));
			}
			catch 
			{
				return null;
			}
		}
	}
}