using System;
using System.Collections.Generic;
using System.IO;

namespace SolutionValidator.Tests
{
	internal static class TestUtils
	{
		public static readonly string[] MockFileSystemDefinition =
		{
			"folder100/",
			"folder200/",
			"folder300/",
			"folder100/folder010/",
			"folder100/folder020/",
			"folder100/folder030/",
			"folder100/folder010/folder001/",
			"folder100/folder010/folder002/",
			"folder100/folder010/folder003/",
			"folderWithFile100/file.txt",
			"folderWithFile200/file.txt",
			"folderWithFile300/file.txt",
			"folderWithFile100/folderWithFile010/file.txt",
			"folderWithFile100/folderWithFile020/file.txt",
			"folderWithFile100/folderWithFile030/file.txt",
			"folderWithFile100/folderWithFile010/folderWithFile001/file.txt",
			"folderWithFile100/folderWithFile010/folderWithFile002/file.txt",
			"folderWithFile100/folderWithFile010/folderWithFile003/file.txt",
			"file001.txt",
			"file001.xxx",
			"file002.txt",
			".file.txt",
			"fileFolder/file001.txt",
			"fileFolder/file001.xxx",
			"fileFolder/file002.txt",
			"fileFolder/.file.txt"
		};

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

			foreach (string name in names)
			{
				string fullName = Path.Combine(root, name.Replace("/", @"\"));

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

					using (StreamWriter writer = File.CreateText(fullName))
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