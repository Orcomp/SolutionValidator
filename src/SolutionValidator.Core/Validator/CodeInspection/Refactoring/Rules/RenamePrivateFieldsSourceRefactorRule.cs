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

	public class RenamePrivateFieldsSourceRefactorRule : SourceRefactorRule<RenamePrivateFieldsChangeCollector>
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
		private readonly string _find;
		private readonly string _replace;

		public RenamePrivateFieldsSourceRefactorRule(string find, string replace, IncludeExcludeCollection sourceFileFilters, IFileSystemHelper fileSystemHelper, string fileNamePattern = "*.cs", bool isBackupEnabled = true)
			: base(sourceFileFilters, fileSystemHelper, fileNamePattern, isBackupEnabled)
		{
			_find = find;
			_replace = replace;
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

		private string GetChangedName(string oldName)
		{
			return Regex.Replace(oldName, _find, _replace, RegexOptions.None);
		}

		protected override IEnumerable<TextChange> CreateChanges(SourceText sourceText, IEnumerable<TextSpan> spans)
		{
			var textChanges = spans.Select(span => new TextChange(span, GetChangedName(sourceText.GetSubText(span).ToString()))).ToList();
			return textChanges;
		}
	}
}