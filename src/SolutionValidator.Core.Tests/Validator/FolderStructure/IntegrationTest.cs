using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SolutionValidator.Core.Infrastructure.Logging.Log4Net;
using SolutionValidator.Core.Validator.Common;
using SolutionValidator.Core.Validator.FolderStructure;
using SolutionValidator.Core.Validator.FolderStructure.Rules;

namespace SolutionValidator.Core.Tests.Validator.FolderStructure
{
	[TestFixture]
	public class IntegrationTest
	{
		private string rootPath;
		private FileSystemHelper fileSystemHelper;
		private FileSystemRuleParser fileSystemRuleParser;
		private RepositoryInfo repositoryInfo;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			rootPath = TestUtils.CreateFoldersAndFiles(TestUtils.MockFileSystemDefinition);
			repositoryInfo = new RepositoryInfo(rootPath);

			fileSystemHelper = new FileSystemHelper();
			fileSystemRuleParser = new FileSystemRuleParser(fileSystemHelper, new Log4NetLogger());
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			TestUtils.DeleteFolder(rootPath);
		}

		private struct PatternAndResult
		{
			public readonly string Pattern;
			public readonly bool Result;

			public PatternAndResult(string pattern, bool result)
			{
				Pattern = pattern;
				Result = result;
			}
		}

		private Stream CreateTestStream(IEnumerable<PatternAndResult> testPatterns)
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			int index = 0;
			foreach (PatternAndResult pr in testPatterns)
			{
				writer.WriteLine(pr.Pattern);
				if (index%2 == 0)
				{
					writer.WriteLine("    \t   "); // Write some empty line
				}
				if (index%3 == 0)
				{
					writer.WriteLine("# Some comment");
				}
				index++;
			}
			writer.Flush();
			stream.Seek(0, SeekOrigin.Begin);
			return stream;
		}

		private readonly PatternAndResult[] testPatterns =
		{
			new PatternAndResult("*.*", true),
			new PatternAndResult("*.txt", true),
			new PatternAndResult("file*.txt", true),
			new PatternAndResult("file001.txt", true),
			new PatternAndResult(".file.txt", true),
			new PatternAndResult("**/*.*", true),
			new PatternAndResult("**/*.txt", true),
			new PatternAndResult("**/**/*.txt", true),
			new PatternAndResult("**/file.txt", true),
			new PatternAndResult("**/**/file.txt", true),
			new PatternAndResult("folderWithFile100/*.*", true),
			new PatternAndResult("folderWithFile100/*.txt", true),
			new PatternAndResult("folderWithFile100/file.txt", true),
			new PatternAndResult("**/folderWithFile100/*.*", false),
			new PatternAndResult("**/folderWithFile100/*.txt", false),
			new PatternAndResult("**/folderWithFile100/file.txt", false),
			new PatternAndResult("*.qwe", false),
			new PatternAndResult("fileqwe*.txt", false),
			new PatternAndResult("file004.txt", false),
			new PatternAndResult(".file4.txt", false),
			new PatternAndResult("**/*.qwe", false),
			new PatternAndResult("**/**/*.qwe", false),
			new PatternAndResult("**/fileqwe.txt", false),
			new PatternAndResult("**/**/fileqwe.txt", false),
			new PatternAndResult("folderWithFile100/*.qwe", false),
			new PatternAndResult("folderWithFile100/file4.txt", false),
			new PatternAndResult("**/folderWithFile100/*.*", false),
			new PatternAndResult("**/folderWithFile100/*.txt", false),
			new PatternAndResult("**/folderWithFile100/file.txt", false),
			new PatternAndResult("**/folderWithFile100/**/*.*", false),
			new PatternAndResult("**/folderWithFile100/**/*.txt", false),
			new PatternAndResult("**/folderWithFile100/**/file.txt", false),
			new PatternAndResult("**/", true),
			new PatternAndResult("**/**/", true),
			new PatternAndResult("**/**/**/", true),
			new PatternAndResult("folder100/", true),
			new PatternAndResult("folder100/**/", true),
			new PatternAndResult("folder100/**/**/", true),
			new PatternAndResult("**/folder010/", true),
			new PatternAndResult("**/folder010/**/", true),
			new PatternAndResult("xxx/", false),
			new PatternAndResult("**/xxx/", false),
			new PatternAndResult("**/**/xxx/", false),
			new PatternAndResult("**/xxx/**/", false)
		};

		[Test]
		[TestCase("*.*", true)]
		[TestCase("*.txt", true)]
		[TestCase("file*.txt", true)]
		[TestCase("file001.txt", true)]
		[TestCase(".file.txt", true)]
		[TestCase("**/*.*", true)]
		[TestCase("**/*.txt", true)]
		[TestCase("**/**/*.txt", true)]
		[TestCase("**/file.txt", true)]
		[TestCase("**/**/file.txt", true)]
		[TestCase("folderWithFile100/*.*", true)]
		[TestCase("folderWithFile100/*.txt", true)]
		[TestCase("folderWithFile100/file.txt", true)]
		[TestCase("**/folderWithFile100/*.*", false)]
		[TestCase("**/folderWithFile100/*.txt", false)]
		[TestCase("**/folderWithFile100/file.txt", false)]
		[TestCase("*.qwe", false)]
		[TestCase("fileqwe*.txt", false)]
		[TestCase("file004.txt", false)]
		[TestCase(".file4.txt", false)]
		[TestCase("**/*.qwe", false)]
		[TestCase("**/**/*.qwe", false)]
		[TestCase("**/fileqwe.txt", false)]
		[TestCase("**/**/fileqwe.txt", false)]
		[TestCase("folderWithFile100/*.qwe", false)]
		[TestCase("folderWithFile100/file4.txt", false)]
		[TestCase("**/folderWithFile100/*.*", false)]
		[TestCase("**/folderWithFile100/*.txt", false)]
		[TestCase("**/folderWithFile100/file.txt", false)]
		[TestCase("**/folderWithFile100/**/*.*", false)]
		[TestCase("**/folderWithFile100/**/*.txt", false)]
		[TestCase("**/folderWithFile100/**/file.txt", false)]
		public void TestFileProcessing(string folderPattern, bool expectedIsValid)
		{
			FileSystemRule rule = fileSystemRuleParser.ParseLine(folderPattern);
			Assert.IsTrue(rule is FileRule);
			ValidationResult validationResult = rule.Validate(repositoryInfo);
			Assert.AreEqual(expectedIsValid, validationResult.IsValid);

			rule = fileSystemRuleParser.ParseLine("!" + folderPattern);
			Assert.IsTrue(rule is FileRule);
			validationResult = rule.Validate(repositoryInfo);
			Assert.AreEqual(expectedIsValid, !validationResult.IsValid);
		}


		[Test]
		[TestCase("**/", true)]
		[TestCase("**/**/", true)]
		[TestCase("**/**/**/", true)]
		[TestCase("folder100/", true)]
		[TestCase("folder100/**/", true)]
		[TestCase("folder100/**/**/", true)]
		[TestCase("**/folder010/", true)]
		[TestCase("**/folder010/**/", true)]
		[TestCase("xxx/", false)]
		[TestCase("**/xxx/", false)]
		[TestCase("**/**/xxx/", false)]
		[TestCase("**/xxx/**/", false)]
		public void TestFolderProcessing(string folderPattern, bool expectedIsValid)
		{
			FileSystemRule rule = fileSystemRuleParser.ParseLine(folderPattern);
			Assert.IsTrue(rule is FolderRule);
			ValidationResult validationResult = rule.Validate(repositoryInfo);
			Assert.AreEqual(expectedIsValid, validationResult.IsValid);

			rule = fileSystemRuleParser.ParseLine("!" + folderPattern);
			Assert.IsTrue(rule is FolderRule);
			validationResult = rule.Validate(repositoryInfo);
			Assert.AreEqual(expectedIsValid, !validationResult.IsValid);
		}

		[Test]
		public void TestWithStream()
		{
			Stream reader = CreateTestStream(testPatterns);
			FileSystemRule[] rules = fileSystemRuleParser.Parse(reader).ToArray();
			Assert.AreEqual(testPatterns.Length, rules.Count());

			for (int index = 0; index < testPatterns.Length; index++)
			{
				PatternAndResult pr = testPatterns[index];
				FileSystemRule rule = rules[index];
				ValidationResult validationResult = rule.Validate(repositoryInfo);
				Assert.AreEqual(pr.Result, validationResult.IsValid);
			}
		}
	}
}