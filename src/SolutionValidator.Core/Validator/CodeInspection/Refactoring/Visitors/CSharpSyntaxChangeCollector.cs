#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="CSharpSyntaxChangeCollector.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.CodeInspection.Refactoring
{
	#region using...
	using System.Collections.Generic;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CSharp;
	using Microsoft.CodeAnalysis.Text;

	#endregion

	public abstract class CSharpSyntaxChangeCollector : CSharpSyntaxRewriter
		//CSharpSyntaxVisitor<SyntaxNode>
	{
		protected readonly SemanticModel SemanticModel;
		private readonly IList<TextSpan> _spans = new List<TextSpan>();

		protected CSharpSyntaxChangeCollector(SemanticModel semanticModel)
		{
			SemanticModel = semanticModel;
		}

		public IEnumerable<TextSpan> Spans
		{
			get { return _spans; }
		}

		protected void AddSpan(TextSpan span)
		{
			_spans.Add(span);
		}
	}
}