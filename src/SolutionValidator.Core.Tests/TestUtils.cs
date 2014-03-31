using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SolutionValidator.Core.Tests
{
	static class TestUtils
	{
		public static string CreateTempRootFolder()
		{
			string result = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			DeleteFolder(result);
			Directory.CreateDirectory(result);
			return result;
		}		
		public static string CreateFoldersAndFiles(IEnumerable<string> names)
		{
			string root = CreateTempRootFolder();

			foreach (var name in names)
			{
				var fullName = Path.Combine(root, name.Replace("/", @"\"));

				if (fullName.EndsWith(@"\"))
				{
					Directory.CreateDirectory(fullName);
				}
				else
				{
					string fileName = Path.GetFileName(fullName);
					string folder = fullName.Replace(fileName, "");
					if (!string.IsNullOrEmpty(folder))
					{
						Directory.CreateDirectory(folder);	
					}
					
					using (var writer = File.CreateText(fullName))
					{
						writer.WriteLine("Created: {0}", DateTime.Now);
					}
					
				}
			}
			return root;
		}

		public static void DeleteFolder(string root)
		{
			if (Directory.Exists(root))
			{
				Directory.Delete(root, true);
			}
		}
	}
}
