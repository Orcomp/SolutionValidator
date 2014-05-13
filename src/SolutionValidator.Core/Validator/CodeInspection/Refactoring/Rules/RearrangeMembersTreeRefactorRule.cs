#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveRedundantThisQualifierRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.CodeInspection.Refactoring
{
	#region using...
	using Catel.Logging;
	using Configuration;
	using FolderStructure;

	#endregion

	public class RearrangeMembersTreeRefactorRule : TreeRefactorRule<RearrangeMembersRewriter>
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

		public RearrangeMembersTreeRefactorRule(IncludeExcludeCollection sourceFileFilters, IFileSystemHelper fileSystemHelper, string fileNamePattern = "*.cs", bool isBackupEnabled = true)
			: base(sourceFileFilters, fileSystemHelper, fileNamePattern, isBackupEnabled)
		{
		}

		#region Message overrides
		protected override string TransformedMessage
		{
			get { return "Rearranged members"; }
		}

		protected override string TransformingMessage
		{
			get { return "Rearranging members"; }
		}

		protected override string TransformerMessage
		{
			get { return "Member rearranger"; }
		}

		protected override string TransformMessage
		{
			get { return "Rearrange members"; }
		}
		#endregion
	}
}