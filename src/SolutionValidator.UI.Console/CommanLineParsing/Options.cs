// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Options.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SolutionValidator.CommanLineParsing
{
    using CommandLine;
    using CommandLine.Text;

    internal class Options
    {
        #region Properties
        [Option('r', "repoRoot", DefaultValue = ".", HelpText = "Path to repository root.")]
        public string RepoRootPath { get; set; }

        [Option('f', "folderCheckRules", DefaultValue = ".folderCheckRules", HelpText = "Path to folder and file check rule file")]
        public string FolderCheckFile { get; set; }

        [Option('v', "verbose", DefaultValue = true, HelpText = "Prints verbose messages to standard output. (does nothing currently)")]
        public bool Verbose { get; set; }

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