#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="PrivateFieldRenameRewriter.cs" company="Orcomp development team">
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
	using Microsoft.CodeAnalysis.Formatting;
	#endregion

	internal class RenamePrivateFieldsRewriter : CSharpSyntaxRewriter
	{
		private readonly SemanticModel _semanticModel;

		public RenamePrivateFieldsRewriter(SemanticModel semanticModel)
		{
			_semanticModel = semanticModel;
		}

		public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax name)
		{
			var symbolInfo = _semanticModel.GetSymbolInfo(name);
			var fieldSymbol = symbolInfo.Symbol as IFieldSymbol;
			
			if (fieldSymbol != null)
			{
				int i;
				var text = name.Parent.GetText();
			}

			return name;
		}


		public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax field)
		{
			return field;
		}
	}
}