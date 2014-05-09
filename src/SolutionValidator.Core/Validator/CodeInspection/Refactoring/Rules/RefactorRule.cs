#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="RefactorRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.CodeInspection.Refactoring
{
	#region using...
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Catel.Logging;
	using Common;
	using Configuration;
	using FolderStructure;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.Text;

	#endregion

	public abstract class RefactorRule : TransformRule
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
		private readonly Project _project;
		protected SourceText RefactorResult;
		protected readonly CustomWorkspace Workspace;

		protected RefactorRule(IncludeExcludeCollection sourceFileFilters, IFileSystemHelper fileSystemHelper, string fileNamePattern = "*.cs", bool isBackupEnabled = true)
			: base(sourceFileFilters, fileSystemHelper, fileNamePattern, isBackupEnabled)
		{
			Workspace = new CustomWorkspace();
			var solution = Workspace.CurrentSolution;
			_project = solution.AddProject("dummyProjectName", "dummyAssemblyName", LanguageNames.CSharp);
		}

		protected override string Transform(string code, string fileName, Action<ValidationResult> notify)
		{
			var document = _project.AddDocument(fileName, code);
			Refactor(document, notify, CancellationToken.None).Wait();
			return RefactorResult.ToString();
		}

		protected abstract Task Refactor(Document document, Action<ValidationResult> notify, CancellationToken cancellationToken);
	}
}