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
	using System.Text.RegularExpressions;
	using NUnit.Framework;
	using SolutionValidator.Configuration;
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
		[TestCase(@"", @"", 15)]

		[TestCase(@"^fileFolder\\.*", @"", 3)]
		[TestCase(@"^fileFolder\\.*;^folderWithFile100\\.*", @";", 10)]
		[TestCase(@"^fileFolder\\.*;^folderWithFile200\\.*", @";", 4)]
		[TestCase(@"^fileFolder\\.*;^folderWithFile200\\.*;^folderWithFile300\\.*", @";;", 5)]
		[TestCase(@"^fileFolder\\.*;^folderWithFile100\\.*;^folderWithFile200\\.*;^folderWithFile300\\.*", @";;;", 12)]

		[TestCase(@"", @"^fileFolder\\.*", 12)]
		[TestCase(@";", @"^fileFolder\\.*;^folderWithFile100\\.*", 5)]
		[TestCase(@";", @"^fileFolder\\.*;^folderWithFile200\\.*", 11)]
		[TestCase(@";;", @"^fileFolder\\.*;^folderWithFile200\\.*;^folderWithFile300\\.*", 10)]
		[TestCase(@";;;", @"^fileFolder\\.*;^folderWithFile100\\.*;^folderWithFile200\\.*;^folderWithFile300\\.*", 3)]

		[TestCase(@"^fileFolder\\.*", @"^fileFolder\\.*", 0)]
		[TestCase(@"^fileFolder\\.*;^folderWithFile100\\.*", @"^fileFolder\\.*;^folderWithFile100\\.*", 0)]
		[TestCase(@"^fileFolder\\.*;^folderWithFile200\\.*", @"^fileFolder\\.*;^folderWithFile200\\.*", 0)]
		[TestCase(@"^fileFolder\\.*;^folderWithFile200\\.*;^folderWithFile300\\.*", @"^fileFolder\\.*;^folderWithFile200\\.*;^folderWithFile300\\.*", 0)]
		[TestCase(@"^fileFolder\\.*;^folderWithFile100\\.*;^folderWithFile200\\.*;^folderWithFile300\\.*", @"^fileFolder\\.*;^folderWithFile100\\.*;^folderWithFile200\\.*;^folderWithFile300\\.*", 0)]

		[TestCase(@"", @".*", 0)]
		[TestCase(@".*", @"", 15)]
		[TestCase(@".*", @".*", 0)]

		[TestCase(@"^fileFolder\\.*", @".*002.*", 2)]
		[TestCase(@"^fileFolder\\.*;", @";.*002.*", 2)]
		[TestCase(@"^fileFolder\\.*;", @".*001.*;.*002.*", 1)]
		public void TestGetFiles(string includes, string excludes, int expectedCount)
		{
			var includeSplit = includes.Split(';');
			var excludeSplit = excludes.Split(';');
			var includeExcludeCollection = new IncludeExcludeCollection();
			
			Assert.AreEqual(includeSplit.Count(), excludeSplit.Count());
			for (int index = 0; index < includeSplit.Length; index++)
			{
				includeExcludeCollection.Add(new IncludeExcludeElement {Include = includeSplit[index], Exclude = excludeSplit[index]});	
			}

			string tempRoot = null;
			try
			{
				tempRoot = TestUtils.CreateFoldersAndFiles(TestUtils.MockFileSystemDefinition, "cs");
				var files = new FileSystemHelper().GetFiles(tempRoot, "*.cs", includeExcludeCollection);
				Assert.AreEqual(expectedCount, files.Count());
			}
			finally
			{
				if (tempRoot != null)
				{
					TestUtils.DeleteFolder(tempRoot);	
				}
			}
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