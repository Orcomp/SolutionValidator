using CommandLine;
using CommandLine.Text;

namespace SolutionValidator.UI.Console.CommanLineParsing
{
	internal class Options
	{
		public const string ConfigFilePathDefaultValue = "%app.config%";

		[Option('r', "repoRoot", DefaultValue = ".", HelpText = "Path to repository root.")]
		public string RepoRootPath { get; set; }

		[Option('c', "config", DefaultValue = ConfigFilePathDefaultValue,
			HelpText = "Path to configuration file.")]
		public string ConfigFilePath { get; set; }

		[Option('v', "verbose", DefaultValue = false,
			HelpText = "Prints verbose messages to standard output.")]
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