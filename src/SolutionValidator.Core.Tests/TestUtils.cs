#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="TestUtils.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Tests
{
	#region using...
	using System;
	using System.Collections.Generic;
	using System.IO;

	#endregion

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
			var result = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			DeleteFolder(result);
			Directory.CreateDirectory(result);
			return result;
		}

		public static string CreateFoldersAndFiles(IEnumerable<string> names, string extension = null)
		{
			var root = CreateTempRootFolder();

			foreach (var name in names)
			{
				var fullName = Path.Combine(root, name.Replace("/", @"\"));

				if (fullName.EndsWith(@"\"))
				{
					Directory.CreateDirectory(fullName);
				}
				else
				{
					var fileName = Path.GetFileName(fullName);
					var folder = fullName.Replace(fileName, "");
					if (!string.IsNullOrEmpty(folder))
					{
						Directory.CreateDirectory(folder);
					}

					if (extension != null)
					{
						fullName = Path.ChangeExtension(fullName, "cs");
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