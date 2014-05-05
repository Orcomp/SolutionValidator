#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="Options.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.CommandLineParsing
{
	#region using...
	using CommandLine;
	using CommandLine.Text;

	#endregion

	internal class Options
	{
		public const string ConfigFilePathDefaultValue = "%app.config%";

		#region Properties
		[Option('r', "repoRoot", DefaultValue = ".", HelpText = "Path to repository root.")]
		public string RepoRootPath { get; set; }

		[Option('c', "config", DefaultValue = SolutionValidatorEnvironment.ConfigFilePathDefaultValue, HelpText = "Path to configuration file.")]
		public string ConfigFilePath { get; set; }

		[Option('v', "verbose", DefaultValue = false, HelpText = "Prints verbose messages to standard output. (does nothing currently)")]
		public bool Verbose { get; set; }

		[Option('F', "reFormat code", DefaultValue = false, HelpText = "Reformats code. Backups all overwritten files in a .zip file in the repository root.")]
		public bool Reformat { get; set; }

		[ParserState]
		public IParserState LastParserState { get; set; }
		#endregion

		#region Methods
		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
		}
		#endregion
	}
}