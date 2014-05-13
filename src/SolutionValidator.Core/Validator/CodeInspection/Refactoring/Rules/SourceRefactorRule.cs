#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="SourceRefactorRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.CodeInspection.Refactoring
{
	#region using...
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Catel.Logging;
	using Common;
	using Configuration;
	using FolderStructure;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.Text;

	#endregion

	public abstract class SourceRefactorRule<T> : TransformRule where T : CSharpSyntaxChangeCollector
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
		private readonly Project _project;
		private SourceText _refactorResult;

		protected SourceRefactorRule(IncludeExcludeCollection sourceFileFilters, IFileSystemHelper fileSystemHelper, string fileNamePattern = "*.cs", bool isBackupEnabled = true)
			: base(sourceFileFilters, fileSystemHelper, fileNamePattern, isBackupEnabled)
		{
			var workspace = new CustomWorkspace();
			var solution = workspace.CurrentSolution;
			_project = solution.AddProject("dummyProjectName", "dummyAssemblyName", LanguageNames.CSharp);
		}

		protected override string Transform(string code, string fileName, Action<ValidationResult> notify)
		{
			var document = _project.AddDocument(fileName, code);
			Refactor(document, notify, CancellationToken.None).Wait();
			return _refactorResult.ToString();
		}

		protected virtual async Task Refactor(Document document, Action<ValidationResult> notify, CancellationToken cancellationToken)
		{
			var tree = await document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
			var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

			var root = await tree.GetRootAsync(cancellationToken).ConfigureAwait(false);
			var changeCollector = (T) Activator.CreateInstance(typeof (T), new object[] {semanticModel});
			changeCollector.Visit(root);

			var sourceText = await document.GetTextAsync(cancellationToken);
			var changes = CreateChanges(sourceText, changeCollector.Spans);

			_refactorResult = sourceText.WithChanges(changes);
		}

		protected abstract IEnumerable<TextChange> CreateChanges(SourceText sourceText, IEnumerable<TextSpan> spans);
	}
}