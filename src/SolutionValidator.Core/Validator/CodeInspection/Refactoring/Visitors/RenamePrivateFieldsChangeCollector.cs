#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="RenamePrivateFieldsChangeCollector.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.CodeInspection.Refactoring
{
	#region using...
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CSharp;
	using Microsoft.CodeAnalysis.CSharp.Syntax;

	#endregion

	public class RenamePrivateFieldsChangeCollector : CSharpSyntaxChangeCollector
	{
		public RenamePrivateFieldsChangeCollector(SemanticModel semanticModel)
			: base(semanticModel)
		{
		}

		public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax name)
		{
			var symbolInfo = SemanticModel.GetSymbolInfo(name);
			var fieldSymbol = symbolInfo.Symbol as IFieldSymbol;

			if (fieldSymbol != null
			    && fieldSymbol.DeclaredAccessibility == Accessibility.Private
			    && !fieldSymbol.IsConst
			    && !fieldSymbol.IsStatic)
			{
				AddSpan(name.Identifier.Span);
			}

			return base.VisitIdentifierName(name);
		}

		public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax field)
		{
			if (
				field.Modifiers.Any(SyntaxKind.PrivateKeyword)
				&&
				!field.Modifiers.Any(SyntaxKind.ConstKeyword)
				&&
				!field.Modifiers.Any(SyntaxKind.StaticKeyword))
			{
				foreach (var variable in field.Declaration.Variables)
				{
					AddSpan(variable.Identifier.Span);
				}
			}
			return base.VisitFieldDeclaration(field);
		}
	}
}