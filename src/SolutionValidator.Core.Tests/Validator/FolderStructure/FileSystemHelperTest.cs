#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemHelperTest.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Tests.Validator.FolderStructure
{
	#region using...
	using System.Linq;
	using NUnit.Framework;
	using SolutionValidator.FolderStructure;

	#endregion

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
			var folders = fileSystemHelper.GetFolders(root, folderPattern);
			Assert.AreEqual(expectedCount, folders.Count());
		}
	}
}