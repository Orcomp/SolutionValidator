#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="RenamePrivateFieldsRefactorRuleTest.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Tests.Validator.CodeInspection
{
	#region using...
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

		private const string InputSourceString = @"
		class C
		{
			private int aaa, bbb = 1;
			private int xxx;
			private int yyy;

			public int Xxx
			{
				get { return xxx; }
				set { xxx = value; }
			}

			private void M()
			{
				var local = 10;
				Console.WriteLine(""Hello, World! {0} {1}"", yyy, local);
				xxx = 3;
				xxx = 4;
				xxx = xxx;
				var any = xxx.ToString();
				AnyMethod(xxx.ToString(), xxx.ToString(), xxx, xxx + 1);
			}
		}";

		private const string OutputSourceString = @"
		class C
		{
			private int _aaa,_bbb = 1;
			private int _xxx;
			private int _yyy;

			public int Xxx
			{
				get { return _xxx; }
				set { _xxx = value; }
			}

			private void M()
			{
				var local = 10;
				Console.WriteLine(""Hello, World! {0} {1}"", _yyy, local);
				_xxx = 3;
				_xxx = 4;
				_xxx = _xxx;
				var any = _xxx.ToString();
				AnyMethod(_xxx.ToString(), _xxx.ToString(), _xxx, _xxx + 1);
			}
		}";

		[Test]
		[TestCase("Current scenarios", InputSourceString, OutputSourceString)]
		public void TestRenamePrivateFieldsRefactorRuleTest(string dummy, string inputSource, string outputSource)
		{
			var configuration = ConfigurationHelper.Load("Not Existing File Name");
			// Arrange:
			fshMock.Setup(f => f.GetFiles(It.IsAny<string>(), It.IsAny<string>(), null)).Returns(new[] {"dummy"});
			fshMock.Setup(f => f.ReadAllText(It.IsAny<string>())).Returns(inputSource);
			fshMock.Setup(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Encoding>())).Callback((string s1, string s2, Encoding e) =>
			{
				//Assert.AreEqual(outputSource, s2);
			});


			// Act:
			var rule = new RenamePrivateFieldsRefactorRule(
				configuration.CSharpFormatting.PrivateFieldRename.Find,
				configuration.CSharpFormatting.PrivateFieldRename.Replace, null, fshMock.Object, "*.cs", false);

			var validationResult = rule.Validate(repositoryInfo);

			// Assert:
			fshMock.Verify(f => f.WriteAllText(It.IsAny<string>(), outputSource, It.IsAny<Encoding>()), Times.Once());
		}

		//[Test]
		//[TestCase("qweAsd", "_qweAsd")]
		//public void TestRegex(string input, string expected)
		//{
		//	const string pattern = "^([a-zA-Z][a-zA-Z0-9_]*$)";
		//	const string replacement = @"_$1";

		//	var result = Regex.Replace(input, pattern, replacement, RegexOptions.None);
		//	Assert.AreEqual(expected, result);
		//}
	}
}