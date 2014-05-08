#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ReformatRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.CodeInspection
{
	#region using...
	using System;
	using Common;
	using Configuration;
	using FolderStructure;
	using ICSharpCode.NRefactory.CSharp;
	using Refactoring;

	#endregion

	public class ReformatRule : TransformRule
	{
		private readonly CSharpFormattingOptions _options;

		public ReformatRule(string optionsFilePath, IncludeExcludeCollection sourceFileFilters, IFileSystemHelper fileSystemHelper, string fileNamePattern = "*.cs", bool isBackupEnabled = true) 
			: base(sourceFileFilters, fileSystemHelper, fileNamePattern, isBackupEnabled)
		{
			_options = CSharpFormattingProperties.GetOptions(optionsFilePath);
		}

		protected override string Transform(string code, string fileName, Action<ValidationResult> notify)
		{
			var formatter = new CSharpFormatter(_options);
			return formatter.Format(code);
		}

		#region Message overrides
		protected override string TransformedMessage
		{
			get { return "Formatted"; }
		}

		protected override string TransformingMessage
		{
			get { return "Formatting"; }
		}

		protected override string TransformerMessage
		{
			get { return "Formatter"; }
		}

		protected override string TransformMessage
		{
			get { return "Format"; }
		}
		#endregion
	}
}