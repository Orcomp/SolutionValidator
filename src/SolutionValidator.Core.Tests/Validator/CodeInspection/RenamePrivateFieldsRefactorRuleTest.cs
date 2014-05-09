#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRuleTest.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Tests.Validator.CodeInspection
{
	#region using...
	using System;
	using System.Configuration;
	using System.Text;
	using System.Text.RegularExpressions;
	using Common;
	using Moq;
	using NUnit.Framework;
	using SolutionValidator.CodeInspection.Refactoring;
	using SolutionValidator.Configuration;
	using SolutionValidator.FolderStructure;

	#endregion

	[TestFixture]
	public class RenamePrivateFieldsRefactorRuleTest
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
		[TestCase("qweAsd", "_qweAsd")]
		public void TestRegex(string input, string expected)
		{
			const string pattern = "^([a-zA-Z][a-zA-Z0-9_]*$)";
			const string replacement = @"_$1";

			var result = Regex.Replace(input, pattern, replacement, RegexOptions.None);
			Assert.AreEqual(expected, result);
		}

		[Test]
		public void TestProto()
		{
			var configuration = ConfigurationHelper.Load("Not Existing File Name");
			// Arrange:
			fshMock.Setup(f => f.GetFiles(It.IsAny<string>(), It.IsAny<string>(), null)).Returns(new String[] { "dummy" });
			fshMock.Setup(f => f.ReadAllText(It.IsAny<string>())).Returns(sourceString);
			fshMock.Setup(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Encoding>())).Callback((string s1,string s2, Encoding e) =>
			{
				var x = s2;
			});

			// Act:
			var rule = new RenamePrivateFieldsRefactorRule(
				configuration.CSharpFormatting.PrivateFieldRename.Find,
				configuration.CSharpFormatting.PrivateFieldRename.Replace, null, fshMock.Object, "*.cs", false);
			
			var validationResult = rule.Validate(repositoryInfo);

			// Assert:
			fshMock.Verify(f => f.WriteAllText(It.IsAny<string>(), sourceString, It.IsAny<Encoding>()), Times.Once());
		}


		const string sourceString = @"
		class C
		{
			private int xxx, aaa,  bbb;
			private int yyy;
			public int Xxx{
				get {return xxx;}
				set {xxx = value;}
			}

			static void Main()
			{
				var local = 10;
				WriteLine(""Hello, World! {0}"", yyy, local);
				this.xxx = 3;
				xxx = 4;
				xxx = 5;
			}
		}";
	}
}