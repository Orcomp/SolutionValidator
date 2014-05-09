#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="RenamePrivateFieldsRefactorRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.CodeInspection.Refactoring
{
	#region using...
	using System;
	using System.Diagnostics;
	using System.Threading;
	using System.Threading.Tasks;
	using Catel.Logging;
	using Common;
	using Configuration;
	using FolderStructure;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.Formatting;
	using Microsoft.CodeAnalysis.Options;

	#endregion

	public class RenamePrivateFieldsRefactorRule : RefactorRule
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
		private string _find;
		private string _replace;

		public RenamePrivateFieldsRefactorRule(string find, string replace, IncludeExcludeCollection sourceFileFilters, IFileSystemHelper fileSystemHelper, string fileNamePattern = "*.cs", bool isBackupEnabled = true)
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
			get { return "Renamed private fields"; }
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

		protected override async Task Refactor(Document document, Action<ValidationResult> notify, CancellationToken cancellationToken)
		{
			var tree = await document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
			var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

			var rewriter = new RenamePrivateFieldsRewriter(_find, _replace, semanticModel, Workspace);
			var root = await tree.GetRootAsync(cancellationToken).ConfigureAwait(true);
			var newRoot = rewriter.Visit(root);
			
			//var newRoot = Formatter.Format(rewriter.Visit(root), Workspace, OptionSet);

			var newDocument = document.WithSyntaxRoot(newRoot);
			
			
			RefactorResult = await newDocument.GetTextAsync(cancellationToken);
		}
	}
}