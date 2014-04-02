using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace SolutionValidator.UI.Console.CommanLineParsing
{
	internal class Options
	{
		[Option('r', "repoRoot", DefaultValue = ".", HelpText = "Path to repository root.")]
		public string RepoRootPath { get; set; }

		[Option('f', "folderCheckRules", DefaultValue = ".folderCheckRules", HelpText = "Path to folder and file check rule file")]
		public string FolderCheckFile { get; set; }

		[Option('v', "verbose", DefaultValue = true, HelpText = "Prints verbose messages to standard output. (does nothing currently)")]
		public bool Verbose { get; set; }

		[ParserState]
		public IParserState LastParserState { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
				
		}
	}

}
