
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using NUnit.Framework;
using SolutionValidator.Core.Validator.FolderStructure;

namespace SolutionValidator.Core.Tests.Validator.FolderStructure
{
	[TestFixture]
	public class FileSystemHelperTest
	{
		private string root;
		private FileSystemHelper fileSystemHelper;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			root = TestUtils.CreateFoldersAndFiles(mockFileSystem);
			fileSystemHelper = new FileSystemHelper();
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			TestUtils.DeleteFolder(root);
		}

		[Test]
		[TestCase("**/", 19)]
		[TestCase("folder100/", 1)]
		[TestCase("folder100/**/", 6)]
		public void TestGetFolders(string folderPattern, int expectedCount)
		{
			var folders = fileSystemHelper.GetFolders(root, folderPattern);
			Assert.AreEqual(expectedCount, folders.Count()); 
		}

		private readonly string[] mockFileSystem = 
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

			"folderWithFile100/folderWithFile010/001/file.txt",
			"folderWithFile100/folderWithFile010/002/file.txt",
			"folderWithFile100/folderWithFile010/003/file.txt",

			"file001.txt",
			"file001.xxx",
			"file002.txt",
			".file.txt",

			"fileFolder/file001.txt",
			"fileFolder/file001.xxx",
			"fileFolder/file002.txt",
			"fileFolder/.file.txt",
		};

		
	}
}