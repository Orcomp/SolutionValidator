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

	public class RemoveRedundantThisQualifierRule : RefactorRule<RemoveRedundantThisQualifierRewriter>
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

		public RemoveRedundantThisQualifierRule(IncludeExcludeCollection sourceFileFilters, IFileSystemHelper fileSystemHelper, string fileNamePattern = "*.cs", bool isBackupEnabled = true) : base(sourceFileFilters, fileSystemHelper, fileNamePattern, isBackupEnabled)
		{
		}

		#region Message overrides
		protected override string TransformedMessage
		{
			get { return "Removed redundant 'this' qualifiers"; }
		}

		protected override string TransformingMessage
		{
			get { return "Removing redundant 'this' qualifiers"; }
		}

		protected override string TransformerMessage
		{
			get { return "Redundant 'this' qualifier remover"; }
		}

		protected override string TransformMessage
		{
			get { return "Remove redundant 'this' qualifiers"; }
		}
		#endregion

		//protected override async Task Refactor(Document document, Action<ValidationResult> notify, CancellationToken cancellationToken)
		//{
		//	var tree = await document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
		//	var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
		//	var rewriter = new RemoveRedundantThisQualifierRewriter(semanticModel);
		//	var root = await tree.GetRootAsync(cancellationToken).ConfigureAwait(true);
		//	var newRoot = rewriter.Visit(root);
		//	var newDocument = document.WithSyntaxRoot(newRoot);
		//	RefactorResult = await newDocument.GetTextAsync(cancellationToken);
		//}
	}
}