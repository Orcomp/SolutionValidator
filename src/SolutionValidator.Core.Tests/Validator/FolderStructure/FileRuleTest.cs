#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRuleTest.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Tests.Validator.FolderStructure
{
	#region using...
	using Common;
	using Moq;
	using NUnit.Framework;
	using SolutionValidator.FolderStructure;

	#endregion

	[TestFixture]
	public class FileRuleTest
	{
		[SetUp]
		public void SetUp()
		{
			fshMock = new Mock<IFileSystemHelper>();
		}

		private const string RootPath = "should not matter what is this content";
		private static readonly string[] Count0 = new string[0];
		private static readonly string[] Count1 = {"anything"};
		private static readonly string[] Count2 = {"anything", "something"};
		private static readonly string[][] Counts = {Count0, Count1, Count2};

		private RepositoryInfo repositoryInfo;
		private Mock<IFileSystemHelper> fshMock;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			repositoryInfo = new RepositoryInfo(RootPath);
		}

		[Test]
		[TestCase(0, true, "qwe.txt", false, true)]
		[TestCase(0, false, "qwe.txt", false, false)]
		[TestCase(0, true, "qwe/qwe.txt", false, true)]
		[TestCase(0, false, "qwe/qwe.txt", false, false)]
		[TestCase(0, true, "qwe/*.txt", false, true)]
		[TestCase(0, false, "qwe/*.txt", false, false)]
		[TestCase(0, true, "**/*.txt", true, false)]
		[TestCase(1, true, "**/*.txt", true, true)]
		[TestCase(2, true, "**/*.txt", true, true)]
		[TestCase(0, false, "**/*.txt", true, false)]
		[TestCase(1, false, "**/*.txt", true, false)]
		[TestCase(2, false, "**/*.txt", true, false)]
		public void TestValidateExist(int countIndex, bool existResult, string pattern, bool isRecursive, bool expectedIsValid)
		{
			// Arrange:
			fshMock.Setup(f => f.GetFolders(RootPath, It.IsAny<string>())).Returns(Counts[countIndex]);
			fshMock.Setup(f => f.Exists(It.IsAny<string>(), It.IsAny<string>())).Returns(existResult);

			// Act:
			var rule = new FileRule(pattern, CheckType.MustExist, fshMock.Object);
			var validationResult = rule.Validate(repositoryInfo);

			// Assert:
			fshMock.Verify(f => f.GetFolders(RootPath, It.IsAny<string>()), isRecursive ? Times.Once() : Times.Never());
			fshMock.Verify(f => f.Exists(It.IsAny<string>(), It.IsAny<string>()),
				isRecursive ? (countIndex == 0 ? Times.Never() : Times.AtLeastOnce()) : Times.Once());
			Assert.AreEqual(expectedIsValid, validationResult.IsValid);
		}

		[Test]
		[TestCase(0, true, "qwe.txt", false, false)]
		[TestCase(0, false, "qwe.txt", false, true)]
		[TestCase(0, true, "qwe/qwe.txt", false, false)]
		[TestCase(0, false, "qwe/qwe.txt", false, true)]
		[TestCase(0, true, "qwe/*.txt", false, false)]
		[TestCase(0, false, "qwe/*.txt", false, true)]
		[TestCase(0, true, "**/*.txt", true, true)]
		[TestCase(1, true, "**/*.txt", true, false)]
		[TestCase(2, true, "**/*.txt", true, false)]
		[TestCase(0, false, "**/*.txt", true, true)]
		[TestCase(1, false, "**/*.txt", true, true)]
		[TestCase(2, false, "**/*.txt", true, true)]
		public void TestValidateNotExist(int countIndex,
			bool existResult,
			string pattern,
			bool isRecursive,
			bool expectedIsValid)
		{
			// Arrange:
			fshMock.Setup(f => f.GetFolders(RootPath, It.IsAny<string>())).Returns(Counts[countIndex]);
			fshMock.Setup(f => f.Exists(It.IsAny<string>(), It.IsAny<string>())).Returns(existResult);

			// Act:
			var rule = new FileRule(pattern, CheckType.MustNotExist, fshMock.Object);
			var validationResult = rule.Validate(repositoryInfo);

			// Assert:
			fshMock.Verify(f => f.GetFolders(RootPath, It.IsAny<string>()), isRecursive ? Times.Once() : Times.Never());
			fshMock.Verify(f => f.Exists(It.IsAny<string>(), It.IsAny<string>()),
				isRecursive ? (countIndex == 0 ? Times.Never() : Times.AtLeastOnce()) : Times.Once());
			Assert.AreEqual(expectedIsValid, validationResult.IsValid);
		}
	}
}