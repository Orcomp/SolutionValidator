using System;
using System.IO;
using System.Linq;
using CommandLine;
using SolutionValidator.Core.Validator.Common;
using SolutionValidator.Core.Validator.FolderStructure;
using SolutionValidator.UI.Console.CommanLineParsing;

namespace SolutionValidator.UI.Console
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var options = new Options();

			var parser = new Parser(with => with.HelpWriter = System.Console.Error);

			if (parser.ParseArgumentsStrict(args, options, () => Environment.Exit(-1)))
			{
				Run(options);
			}
		}

		private static void Run(Options options)
		{
			string repoRootPath = string.Empty;
			string folderCheckRulesPath = string.Empty;
			try
			{
				repoRootPath = Path.GetFullPath(options.RepoRootPath);
				if (!Directory.Exists(repoRootPath))
				{
					// TODO: Move message to resource
					System.Console.WriteLine("Repository root path >{0}< does not exist.", repoRootPath);
					Environment.Exit(-2);
				}
			}
			catch (Exception e)
			{
				// TODO: Move message to resource
				System.Console.WriteLine("Error when processing Repository root path: >{0}<.", options.RepoRootPath);
				System.Console.WriteLine(e.ToString());
				Environment.Exit(-2);
			}

			try
			{
				folderCheckRulesPath = Path.GetFullPath(options.FolderCheckFile);
				if (!File.Exists(folderCheckRulesPath))
				{
					// TODO: Move message to resource
					System.Console.WriteLine("Folder and file check rule file: >{0}< does not exist.", folderCheckRulesPath);
					Environment.Exit(-3);
				}
			}
			catch (Exception e)
			{
				// TODO: Move message to resource
				System.Console.WriteLine("Error when processing 'folder and file check rule file' path >{0}<.",
					options.FolderCheckFile);
				System.Console.WriteLine(e.ToString());
				Environment.Exit(-3);
			}

			var fileSystemRuleParser = new FileSystemRuleParser(new FileSystemHelper());
			var rules = fileSystemRuleParser.Parse(folderCheckRulesPath);

			var repositoryInfo = new RepositoryInfo(repoRootPath);

			System.Console.WriteLine("Found {0} rules, processing...", rules.Count());
			int errorCount = 0;
			foreach (var rule in rules)
			{
				var validationResult = rule.Validate(repositoryInfo);
				if (!validationResult.IsValid)
				{
					System.Console.Error.WriteLine("Error, the following rule is not satisfied: {0}", validationResult.Description);
					errorCount++;
				}
			}

			System.Console.WriteLine("{0} rules, processed. Errors found: {1}", rules.Count(), errorCount);
			// TODO: Move message to resource
			System.Console.WriteLine("Press any key to continue...");
			System.Console.ReadKey(true);
		}
	}
}
