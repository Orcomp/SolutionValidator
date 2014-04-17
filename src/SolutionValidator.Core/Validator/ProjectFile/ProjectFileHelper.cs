using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Evaluation;
using SolutionValidator.Core.Properties;
using SolutionValidator.Core.Validator.Common;
using Project = Microsoft.Build.Evaluation.Project;

namespace SolutionValidator.Core.Validator.ProjectFile
{
	public class ProjectFileHelper : IProjectFileHelper
	{
		private const string SearchPattern = "*.csproj";
		private ProjectCollection collection;
		private string projectFileFullName;
		private Project project;
		private string assemblyName;

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
				throw new ProjectFileException(string.Format(Resources.ProjectFileHelper_GetAllProjectFilePath_GetAllProjectPath_was_called_with_bad_argument, root), e);
			}

			return result;
		}

		public void LoadProject(string path)
		{
			LoadProject(path, null);
		}



		public void CheckOutputPath(string repoRoot, string expectedOutputPath, ValidationResult result, Action<ValidationResult> notify = null)
		{
			assemblyName = project.GetPropertyValue("AssemblyName");
			var configurationNames = project.ConditionedProperties["Configuration"];
			
			foreach (string configurationName in configurationNames)
			{
				CheckOneOutputPath(configurationName, repoRoot, expectedOutputPath, result, notify);
			}
		}

		#endregion

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

		private void CheckOneOutputPath(string configuration, string repoRoot, string expectedOutputPath, ValidationResult result, Action<ValidationResult> notify)
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
				var outputPath = item.EvaluatedInclude;
				if (Path.IsPathRooted(outputPath) || !outputPath.StartsWith("."))
				{
					message = string.Format(Resources.ProjectFileHelper_CheckOne_Output_path_must_be_a_relative_path, outputPath, GetProjectInfo(configuration));
					result.AddResult(ResultLevel.Error, message, notify);
					return;
				}
				//($repoRoot)\($expectedOutputPath)\($configuration)\($targetFrameworkVersion)\($projectName)

				repoRoot = repoRoot.Trim('\\');
				expectedOutputPath = expectedOutputPath.Trim('\\');
				var targetFrameworkVersion = GetTargetFrameworkVersion(project);
				
				// We are using the assembly name here for the sake of simplicity. However please note
				// that other rules are forcing project file name to be identical with assembly name and root namespace name

				var projectName = assemblyName;
				var projectFolder = Path.GetDirectoryName(projectFileFullName).Trim('\\');
				
				var expectedValue = string.Format(@"{0}\{1}\{2}\{3}\{4}", repoRoot, expectedOutputPath, configuration,
					targetFrameworkVersion, projectName).ToLower();

				var actualValue = Path.GetFullPath(Path.Combine(projectFolder, outputPath)).Trim('\\').ToLower();

				
				if (String.Compare(expectedValue, actualValue, StringComparison.Ordinal) != 0)
				{
					message = string.Format(Resources.ProjectFileHelper_CheckOne_Output_path_was_evaluated_to, actualValue, expectedValue, GetProjectInfo(configuration));
					result.AddResult(ResultLevel.Error, message, notify);
					return;

				}
				message = string.Format(Resources.ProjectFileHelper_CheckOneOutputPath_OutputPath_conforms_to_the_required_standards, GetProjectInfo(configuration));
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
				var value = project.GetProperty("TargetFrameworkVersion").EvaluatedValue;
				return string.Format("NET{0}", value.Replace(".", "").Replace("v", ""));
			}
			catch (Exception e)
			{
				return null;
			}
		}

		private void Dump()
		{
			var sb = new StringBuilder();
			foreach (ProjectProperty p in project.Properties)
			{
				string text = string.Format("{0}: {1}", p.Name, p.EvaluatedValue);
				sb.AppendLine(text);
			}
			string x = sb.ToString();

			foreach (ProjectItem item in project.Items)
			{
				string text = string.Format("Type: {0}, Count {1}, E: {2}", item.ItemType, item.DirectMetadataCount,
					item.EvaluatedInclude);
				sb.AppendLine(text);
				if (text.Contains("_Out"))
				{
					int i = 1;
				}

				foreach (ProjectMetadata md in item.DirectMetadata)
				{
					text = string.Format("\tName: {0}, Value {1}", md.Name, md.EvaluatedValue);
					sb.AppendLine(text);
				}
			}
			string r = sb.ToString();
			Debug.WriteLine(r);
		}

		private string GetProjectInfo(string configuration = "N/A")
		{
			return string.Format("Project File: '{0}', Configuration '{1}'", projectFileFullName, configuration);
		}
	}
}