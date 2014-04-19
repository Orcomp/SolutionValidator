using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SolutionValidator.Validator.FolderStructure;

namespace SolutionValidator.Tests.Validator.FolderStructure
{
	[TestFixture]
	public class FileSystemHelperTest
	{
		private string root;
		private FileSystemHelper fileSystemHelper;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			root = TestUtils.CreateFoldersAndFiles(TestUtils.MockFileSystemDefinition);
			fileSystemHelper = new FileSystemHelper();
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			TestUtils.DeleteFolder(root);
		}

		[Test]
		[TestCase("**/", 19)]
		[TestCase("**/**/", 12)]
		[TestCase("**/**/**/", 6)]
		[TestCase("folder100/", 1)]
		[TestCase("folder100/**/", 6)]
		[TestCase("folder100/**/**/", 3)]
		[TestCase("**/folder010/", 1)]
		[TestCase("**/folder010/**/", 3)]
		public void TestGetFolders(string folderPattern, int expectedCount)
		{
			IEnumerable<string> folders = fileSystemHelper.GetFolders(root, folderPattern);
			Assert.AreEqual(expectedCount, folders.Count());
		}
	}
}