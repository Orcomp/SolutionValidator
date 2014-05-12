#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveRedundantThisQualifierRewriter.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.CodeInspection.Refactoring
{
	#region using...
	using System.Linq;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CSharp;
	using Microsoft.CodeAnalysis.CSharp.Syntax;

	#endregion

	// ReSharper disable once ClassNeverInstantiated.Global
	public class RemoveRedundantThisQualifierChangeCollector : CSharpSyntaxChangeCollector
	{
		public RemoveRedundantThisQualifierChangeCollector(SemanticModel semanticModel) : base(semanticModel)
		{
		}

		public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
		{
			var thisNode = node.ChildNodes().FirstOrDefault() as ThisExpressionSyntax;
			var nameNode = node.ChildNodes().LastOrDefault() as IdentifierNameSyntax;

			if (thisNode == null || nameNode == null)
			{
				return base.VisitMemberAccessExpression(node);
			}

			// Check for field: 
			var symbolInfo = SemanticModel.GetSymbolInfo(nameNode);
			var fieldSymbol = symbolInfo.Symbol as IFieldSymbol;
			if (fieldSymbol != null)
			{
				// Checking for naming ambiguities:
				var symbols = SemanticModel.LookupSymbols(node.SpanStart)
					.Where(s => new[]
					{
						SymbolKind.Local,
						SymbolKind.Parameter,
						SymbolKind.Event,
						SymbolKind.Property
					}.Any(k => k == s.Kind));

				if (symbols.Any(s => s.Name.Equals(nameNode.Identifier.ValueText)))
				{
					// Removing the "this" qualifier may cause name resolving ambiguities. 
					// Not removing the "this" qualifier:
					return base.VisitMemberAccessExpression(node);
				}
			}

			// Not field or field be no ambiguities so we are free to remove the "this" qualifier:
			AddSpan(thisNode.Span);
			AddSpan(node.OperatorToken.FullSpan);
			return base.VisitMemberAccessExpression(node);
			
		}
	}
}