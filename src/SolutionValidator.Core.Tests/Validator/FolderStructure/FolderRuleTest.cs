using Moq;
using NUnit.Framework;
using SolutionValidator.Core.Infrastructure.Logging.Log4Net;
using SolutionValidator.Core.Validator.Common;
using SolutionValidator.Core.Validator.FolderStructure;
using SolutionValidator.Core.Validator.FolderStructure.Rules;

namespace SolutionValidator.Core.Tests.Validator.FolderStructure
{
	[TestFixture]
	public class FolderRuleTest
	{
		#region Setup/Teardown

		[SetUp]
		public void SetUp()
		{
			fshMock = new Mock<IFileSystemHelper>();
		}

		#endregion

		private const string RootPath = "should not matter what is this content";
		private const string FolderPattern = "also should not matter what is this content";
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
		[TestCase(0, false)]
		[TestCase(1, true)]
		[TestCase(2, true)]
		public void TestValidateExist(int countIndex, bool expectedIsValid)
		{
			// Arrange:
			fshMock.Setup(f => f.GetFolders(RootPath, FolderPattern)).Returns(Counts[countIndex]);

			// Act:
			var rule = new FolderRule(FolderPattern, CheckType.MustExist, fshMock.Object, new Log4NetLogger());
			ValidationResult validationResult = rule.Validate(repositoryInfo);

			// Assert:
			fshMock.Verify(f => f.GetFolders(RootPath, FolderPattern), Times.Once);
			Assert.AreEqual(expectedIsValid, validationResult.IsValid);
		}

		[Test]
		[TestCase(0, true)]
		[TestCase(1, false)]
		[TestCase(2, false)]
		public void TestValidateNotExist(int countIndex, bool expectedIsValid)
		{
			// Arrange:
			fshMock.Setup(f => f.GetFolders(RootPath, FolderPattern)).Returns(Counts[countIndex]);

			// Act:
			var rule = new FolderRule(FolderPattern, CheckType.MustNotExist, fshMock.Object, new Log4NetLogger());
			ValidationResult validationResult = rule.Validate(repositoryInfo);

			// Assert:
			fshMock.Verify(f => f.GetFolders(RootPath, FolderPattern), Times.Once);
			Assert.AreEqual(expectedIsValid, validationResult.IsValid);
		}
	}
}