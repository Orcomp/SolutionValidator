using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SolutionValidator.Core.Validator.FolderStructure;

namespace SolutionValidator.Core.Tests.Validator.FolderStructure
{
	[TestFixture]
	public class FileSystemRuleParserTest
	{
		private FileSystemRuleParser parser;


		[TestFixtureSetUp]
		public void Setup()
		{
			parser = new FileSystemRuleParser();
			//projectInfo = new ProjectInfo("c:/solution", "c:/project");
		}

		[Test]
		public void Test()
		{
			//var output = new List<string>();
			//Find("D:\\2014Develop\\Elance\\Twitter\\Twitter\\", "**\\bin\\x.exe", output);
			//var x = Directory.GetDirectories(@"d:\\2014Develop\", "*", SearchOption.AllDirectories);
			//var y = new FileSystemHelper().GetFolders("d:\\2014Develop\\", "**\\AssemblaAPI\\**\\bin");
			//var y = new FileSystemHelper().GetFolders("d:\\2014Develop\\", "**\\AssemblaAPI\\**\\bin");
		}

		[Test]
		[TestCase("**/qwe")]
		[TestCase("qwe")]
		[TestCase("qwe/qwe")]
		[TestCase("qwe/qwe/asd.asd")]
		public void TestParseLineValidPath(string line)
		{
			FileSystemRule result = parser.ParseLine(line);

			result = parser.ParseLine("!" + line);
			result = parser.ParseLine(line + "/");
			result = parser.ParseLine("!" + line + "/");
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