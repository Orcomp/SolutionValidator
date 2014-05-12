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
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;
	using Catel.IoC;
	using Catel.Logging;
	using Configuration;
	using FolderStructure;
	using Microsoft.CodeAnalysis.Text;

	#endregion

	public class RemoveRedundantThisQualifierSourceRefactorRule : SourceRefactorRule<RemoveRedundantThisQualifierChangeCollector>
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

		public RemoveRedundantThisQualifierSourceRefactorRule(IncludeExcludeCollection sourceFileFilters, IFileSystemHelper fileSystemHelper, string fileNamePattern = "*.cs", bool isBackupEnabled = true)
			: base(sourceFileFilters, fileSystemHelper, fileNamePattern, isBackupEnabled)
		{
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


		protected override IEnumerable<TextChange> CreateChanges(SourceText sourceText, IEnumerable<TextSpan> spans)
		{
			return spans.Select(span => new TextChange(span, "")).ToList();;
		}
	}
}