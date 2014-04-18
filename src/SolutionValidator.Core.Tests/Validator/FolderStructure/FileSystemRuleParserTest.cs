using NUnit.Framework;
using SolutionValidator.Core.Infrastructure.Logging.Log4Net;
using SolutionValidator.Core.Validator.FolderStructure;
using SolutionValidator.Core.Validator.FolderStructure.Rules;

namespace SolutionValidator.Core.Tests.Validator.FolderStructure
{
	[TestFixture]
	public class FileSystemRuleParserTest
	{
		private FileSystemRuleParser parser;


		[TestFixtureSetUp]
		public void Setup()
		{
			parser = new FileSystemRuleParser(null, new Log4NetLogger());
		}

		[Test]
		[TestCase("**/qwe", true, true)]
		[TestCase("qwe", false, true)]
		[TestCase("qwe/qwe", false, true)]
		[TestCase("qwe/qwe/asd.asd", false, true)]
		[TestCase("qwe/**/asd.asd", true, true)]
		[TestCase("qwe/*.*", false, false)]
		public void TestParseLineValidPath(string line, bool expectedIsRecursive, bool canBeFolder)
		{
			FileSystemRule result = parser.ParseLine(line);
			Assert.IsTrue(result is FileRule);
			Assert.AreEqual(CheckType.MustExist, result.UnitTestPeek.CheckType);
			Assert.AreEqual(expectedIsRecursive, result.UnitTestPeek.IsRecursive);

			result = parser.ParseLine("!" + line);
			Assert.IsTrue(result is FileRule);
			Assert.AreEqual(CheckType.MustNotExist, result.UnitTestPeek.CheckType);
			Assert.AreEqual(expectedIsRecursive, result.UnitTestPeek.IsRecursive);

			if (canBeFolder)
			{
				result = parser.ParseLine(line + "/");
				Assert.IsTrue(result is FolderRule);
				Assert.AreEqual(CheckType.MustExist, result.UnitTestPeek.CheckType);
				Assert.AreEqual(expectedIsRecursive, result.UnitTestPeek.IsRecursive);

				result = parser.ParseLine("!" + line + "/");
				Assert.IsTrue(result is FolderRule);
				Assert.AreEqual(CheckType.MustNotExist, result.UnitTestPeek.CheckType);
				Assert.AreEqual(expectedIsRecursive, result.UnitTestPeek.IsRecursive);
			}
		}


		[Test]
		[TestCase("#")]
		[TestCase(" #")]
		[TestCase(" # abc")]
		public void TestParseLineWithCommen(string line)
		{
			Assert.IsNull(parser.ParseLine(line));
		}

		[Test]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("\n")]
		[TestCase("   \t   ")]
		public void TestParseLineWithEmpty(string line)
		{
			Assert.IsNull(parser.ParseLine(line));
		}

		[Test]
		[TestCase("folder1/ /folder2")]
		[TestCase("folder2//folder2")]
		[TestCase("folder2/ /folder2/f.ext")]
		[ExpectedException(typeof (ParseException), ExpectedMessage = "RelativePath can not contain empty parts",
			MatchType = MessageMatch.Contains)]
		public void TestParseLineWithEmptyPartPath(string line)
		{
			parser.ParseLine(line);
		}


		[Test]
		[TestCase("!**/q**we/")]
		[TestCase("!q**we/")]
		[TestCase("q**we/")]
		[ExpectedException(typeof (ParseException), ExpectedMessage = "Invalid use", MatchType = MessageMatch.Contains)]
		public void TestParseLineWithInvalidDoubleStar(string line)
		{
			parser.ParseLine(line);
		}

		[Test]
		[TestCase("q?we")]
		[TestCase("q*we/")]
		[ExpectedException(typeof (ParseException), ExpectedMessage = "Invalid path", MatchType = MessageMatch.Contains)]
		public void TestParseLineWithInvalidPath(string line)
		{
			parser.ParseLine(line);
		}

		[Test]
		[TestCase("!/we")]
		[TestCase("/we/")]
		[ExpectedException(typeof (ParseException), ExpectedMessage = "RelativePath must be relative",
			MatchType = MessageMatch.Contains)]
		public void TestParseLineWithNonRootedInvalidPath(string line)
		{
			parser.ParseLine(line);
		}
	}
}