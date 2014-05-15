#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveRedundantThisQualifierRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Tests.Validator.CodeInspection
{
	#region using...
	using System.Text;
	using Common;
	using Moq;
	using NUnit.Framework;
	using SolutionValidator.CodeInspection.Refactoring;
	using SolutionValidator.FolderStructure;

	#endregion

	[TestFixture]
	public class RemoveRedundantThisQualifierRuleTest
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

		private const string InputSourceMustRemoveOnField = @"
		class C
		{
			private int xxx, aaa,  bbb  = 1;
			private int yyy;
			public int Xxx
			{
				get {return this.xxx;} // this qualifier must removed here
				set {this.xxx = value;} // this qualifier must removed here
			}

			static void Main()
			{
				var local = 10;
				WriteLine(""Hello, World! {0}"", yyy, local);
				this.xxx = 3; // this qualifier must removed here
				this.xxx = 4; // this qualifier must removed here
				xxx = this.xxx; // this qualifier must removed here
				var any = this.xxx.ToString(); // this qualifier must removed here
				// this qualifier must removed below
				AnyMethod(xxx.ToString(),  this.xxx.ToString(),  this.xxx,  this.xxx  +  1);
			}
		}";

		private const string OutputSourceMustRemoveOnField = @"
		class C
		{
			private int xxx, aaa,  bbb  = 1;
			private int yyy;
			public int Xxx
			{
				get {return xxx;} // this qualifier must removed here
				set {xxx = value;} // this qualifier must removed here
			}

			static void Main()
			{
				var local = 10;
				WriteLine(""Hello, World! {0}"", yyy, local);
				xxx = 3; // this qualifier must removed here
				xxx = 4; // this qualifier must removed here
				xxx = xxx; // this qualifier must removed here
				var any = xxx.ToString(); // this qualifier must removed here
				// this qualifier must removed below
				AnyMethod(xxx.ToString(),  xxx.ToString(),  xxx,  xxx  +  1);
			}
		}";

		private const string InputSourceMustRemoveOnMethod = @"
		class C
		{
			private int xxx = 1;

			void xxx()
			{
				this.xxx(); // this qualifier must removed here
				this.xxx = 3 // this qualifier must removed here
			}
		}";

		private const string OutputSourceMustRemoveOnMethod = @"
		class C
		{
			private int xxx = 1;

			void xxx()
			{
				xxx(); // this qualifier must removed here
				xxx = 3 // this qualifier must removed here
			}
		}";

		private const string InputSourceMustNotRemoveBecauseOfParameter = @"
		class C
		{
			private int xxx = 1;

			void M1(int xxx)
			{
				this.xxx(); // this qualifier must removed here
				this.xxx = xxx; // this qualifier must NOT removed here
			}
		}";

		private const string OutputSourceMustNotRemoveBecauseOfParameter = @"
		class C
		{
			private int xxx = 1;

			void M1(int xxx)
			{
				xxx(); // this qualifier must removed here
				this.xxx = xxx; // this qualifier must NOT removed here
			}
		}";

		private const string InputSourceMustNotRemoveBecauseOfLocal = @"
		class C
		{
			private int xxx = 1;

			void M2()
			{
				this.xxx(); // this qualifier must removed here
				this.xxx = xxx; // this qualifier must NOT removed here
				var xxx = 3;
			}
		}";

		private const string OutputSourceMustNotRemoveBecauseOfLocal = @"
		class C
		{
			private int xxx = 1;

			void M2()
			{
				xxx(); // this qualifier must removed here
				this.xxx = xxx; // this qualifier must NOT removed here
				var xxx = 3;
			}
		}";

		[Test]
		[TestCase("Must Remove On Field", InputSourceMustRemoveOnField, OutputSourceMustRemoveOnField)]
		[TestCase("Must Remove On Method", InputSourceMustRemoveOnMethod, OutputSourceMustRemoveOnMethod)]
		[TestCase("Must Not Remove Because Of Parameter", InputSourceMustNotRemoveBecauseOfParameter, OutputSourceMustNotRemoveBecauseOfParameter)]
		[TestCase("Must Not Remove Because Of Local", InputSourceMustNotRemoveBecauseOfLocal, OutputSourceMustNotRemoveBecauseOfLocal)]
		public void TestRemoveRedundantThisQualifierRule(string dummy, string inputSource, string outputSource)
		{
			if (outputSource == null)
			{
				outputSource = inputSource;
			}

			// Arrange:
			fshMock.Setup(f => f.GetFiles(It.IsAny<string>(), It.IsAny<string>(), null)).Returns(new[] {"dummy"});
			fshMock.Setup(f => f.ReadAllText(It.IsAny<string>())).Returns(inputSource);
			fshMock.Setup(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Encoding>())).Callback((string s1, string s2, Encoding e) =>
			{
				//	Assert.AreEqual(outputSource, s2);
			});

			// Act:
			var rule = new RemoveRedundantThisQualifierRule(null, fshMock.Object, "*.cs", false);
			var validationResult = rule.Validate(repositoryInfo);

			// Assert:
			fshMock.Verify(f => f.WriteAllText(It.IsAny<string>(), outputSource, It.IsAny<Encoding>()), Times.Once());
		}
	}
}