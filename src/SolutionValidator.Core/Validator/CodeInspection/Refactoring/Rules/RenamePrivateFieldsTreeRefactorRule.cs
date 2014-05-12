#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="RenamePrivateFieldsTreeRefactorRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.CodeInspection.Refactoring
{
	#region using...
	using System.Text.RegularExpressions;
	using Catel.Logging;
	using Configuration;
	using FolderStructure;

	#endregion

	public class RenamePrivateFieldsTreeRefactorRule : TreeRefactorRule<RenamePrivateFieldsRewriter>
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

		public RenamePrivateFieldsTreeRefactorRule(string find, string replace, IncludeExcludeCollection sourceFileFilters, IFileSystemHelper fileSystemHelper, string fileNamePattern = "*.cs", bool isBackupEnabled = true)
			: base(sourceFileFilters, fileSystemHelper, fileNamePattern, isBackupEnabled)
		{
			Parameter.Find = find;
			Parameter.Replace = replace;
		}

		#region Message overrides
		protected override string TransformedMessage
		{
			get { return "Renamed private fields"; }
		}

		protected override string TransformingMessage
		{
			get { return "Renaming private fields"; }
		}

		protected override string TransformerMessage
		{
			get { return "Private fields renamer"; }
		}

		protected override string TransformMessage
		{
			get { return "Rename private fields"; }
		}
		#endregion
	}
}