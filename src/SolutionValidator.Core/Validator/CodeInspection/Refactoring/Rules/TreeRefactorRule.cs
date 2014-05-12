#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeRefactorRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.CodeInspection.Refactoring
{
	#region using...
	using System;
	using System.Dynamic;
	using System.Threading;
	using System.Threading.Tasks;
	using Catel.Logging;
	using Common;
	using Configuration;
	using FolderStructure;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CSharp;
	using Microsoft.CodeAnalysis.Text;

	#endregion

	public abstract class TreeRefactorRule<T> : TransformRule where T : CSharpSyntaxRewriter
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
		protected readonly dynamic Parameter;
		private readonly Project _project;

		protected SourceText RefactorResult;

		protected TreeRefactorRule(IncludeExcludeCollection sourceFileFilters, IFileSystemHelper fileSystemHelper, string fileNamePattern = "*.cs", bool isBackupEnabled = true)
			: base(sourceFileFilters, fileSystemHelper, fileNamePattern, isBackupEnabled)
		{
			//var workspace = new CustomWorkspace();
			var workspace = new CustomWorkspace();
			var solution = workspace.CurrentSolution;
			_project = solution.AddProject("dummyProjectName", "dummyAssemblyName", LanguageNames.CSharp);
			Parameter = new ExpandoObject();
		}

		protected override string Transform(string code, string fileName, Action<ValidationResult> notify)
		{
			var document = _project.AddDocument(fileName, code);
			Refactor(document, notify, CancellationToken.None).Wait();
			return RefactorResult.ToString();
		}

		protected virtual async Task Refactor(Document document, Action<ValidationResult> notify, CancellationToken cancellationToken)
		{
			var tree = await document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
			var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

			var root = await tree.GetRootAsync(cancellationToken).ConfigureAwait(false);

			// TODO: Discover available constructors dynamically:
			var rewriter = (T) Activator.CreateInstance(typeof (T), new[] {Parameter, semanticModel});
			var newRoot = rewriter.Visit(root);

			var newDocument = document.WithSyntaxRoot(newRoot);
			RefactorResult = await newDocument.GetTextAsync(cancellationToken);
		}
	}
}