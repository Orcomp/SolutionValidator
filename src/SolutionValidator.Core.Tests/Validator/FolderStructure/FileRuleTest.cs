//using Moq;
//using NUnit.Framework;
//using SolutionValidator.Core.Validator.Common;
//using SolutionValidator.Core.Validator.FolderStructure;

//namespace SolutionValidator.Core.Tests.Validator.FolderStructure
//{
//	[TestFixture]
//	public class FileRuleTest
//	{
//		const string Root = "should not matter what is this content";
//		const string FolderPattern = "also should not matter what is this content";
//		static readonly string[] Count0 = new string[0];
//		static readonly string[] Count1 = { "anything" };
//		static readonly string[] Count2 = { "anything", "something" };
//		static readonly string[][] Counts = {Count0, Count1, Count2};

//		private ProjectInfo projectInfo;
//		Mock<IFileSystemHelper> fshMock;
		
//		[TestFixtureSetUp]
//		public void TestFixtureSetUp()
//		{
//			projectInfo = new ProjectInfo(Root, Root);
//		}

//		[SetUp]
//		public void SetUp()
//		{
//			fshMock = new Mock<IFileSystemHelper>();
//		}


//		[Test]
//		[TestCase(0, false)]
//		[TestCase(1, true)]
//		[TestCase(2, true)]
//		public void TestValidateExist(int countIndex, bool existReturnValue, bool expectedIsValid)
//		{
//			// Arrange:
//			fshMock.Setup(f => f.GetFolders(Root, FolderPattern)).Returns(Counts[countIndex]);
			
			
//			// Act:
//			var rule = new FolderRule(FolderPattern, CheckType.MustExist, fshMock.Object);
//			ValidationResult validationResult = rule.Validate(projectInfo);
			
//			// Assert:
//			fshMock.Verify(f => f.GetFolders(Root, FolderPattern), Times.Once);
//			Assert.AreEqual(expectedIsValid, validationResult.IsValid);
			
//		}

//		[Test]
//		[TestCase(0, true)]
//		[TestCase(1, false)]
//		[TestCase(2, false)]
//		public void TestValidateNotExist(int countIndex, bool expectedIsValid)
//		{
//			// Arrange:
//			fshMock.Setup(f => f.GetFolders(Root, FolderPattern)).Returns(Counts[countIndex]);

//			// Act:
//			var rule = new FolderRule(FolderPattern, CheckType.MustNotExist, fshMock.Object);
//			ValidationResult validationResult = rule.Validate(projectInfo);

//			// Assert:
//			Assert.AreEqual(expectedIsValid, validationResult.IsValid);
//		}
//	}
//}