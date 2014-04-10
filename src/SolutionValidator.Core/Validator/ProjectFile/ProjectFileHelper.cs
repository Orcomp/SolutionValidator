using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Evaluation;

namespace SolutionValidator.Core.Validator.ProjectFile
{
	public class ProjectFileHelper : IProjectFileHelper
	{
		private const string SearchPattern = "*.csproj";
		private ProjectCollection collection;
		private string fullPath;
		private Project project;

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
				throw new OutputPathException(string.Format("GetAllProjectPath was called with bad argument. root: {0}", root), e);
			}

			return result;
		}

		public void LoadProject(string path)
		{
			LoadProject(path, null);
		}

		public bool Check(string root, string expectedOutputPath)
		{
			foreach (string configurationName in project.ConditionedProperties["Configuration"])
			{
				if (!CheckOne(configurationName, root, expectedOutputPath))
				{
					return false;
				}
			}
			return true;
		}

		public bool Modify(string expectedOutputPath)
		{
			throw new NotImplementedException();
		}

		#endregion

		private void LoadProject(string path, string configuration)
		{
			collection = new ProjectCollection {DefaultToolsVersion = "4.0"};
			fullPath = Path.GetFullPath(path);
			if (configuration != null)
			{
				collection.SetGlobalProperty("Configuration", configuration);
			}
			project = collection.LoadProject(fullPath);
		}

		private bool CheckOne(string configuration, string root, string expectedOutputPath)
		{
			LoadProject(fullPath, configuration);

			ProjectItem item = project.GetItems("_OutputPathItem").FirstOrDefault();


			Dump();
			return false;
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
	}
}