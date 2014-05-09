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
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CSharp;
	using Microsoft.CodeAnalysis.CSharp.Syntax;
	using Microsoft.CodeAnalysis.Formatting;

	#endregion

	internal class RenamePrivateFieldsRewriter : CSharpSyntaxRewriter
	{
		private readonly SemanticModel _semanticModel;
		private string _find;
		private string _replace;

		public RenamePrivateFieldsRewriter(string find, string replace, SemanticModel semanticModel)
		{
			_find = find;
			_replace = replace;
			_semanticModel = semanticModel;
		}

		public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax name)
		{
			var symbolInfo = _semanticModel.GetSymbolInfo(name);
			var fieldSymbol = symbolInfo.Symbol as IFieldSymbol;
			
			if (fieldSymbol != null 
				&& fieldSymbol.DeclaredAccessibility == Accessibility.Private 
				&& !fieldSymbol.IsConst)
			{
				name = name
					.WithIdentifier(SyntaxFactory.Identifier(GetChangedName(name.Identifier.ValueText)))
					.WithLeadingTrivia(name.GetLeadingTrivia())
					.WithTrailingTrivia(name.GetTrailingTrivia());
			}

			return name;
		}


		public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax field)
		{
			if (!field.Modifiers.Any(SyntaxKind.PrivateKeyword))
			{
				return field;
			}

			if (field.Modifiers.Any(SyntaxKind.ConstKeyword))
			{
				return field;
			}
			
			var variables = new List<VariableDeclaratorSyntax>();
			foreach (var variable in field.Declaration.Variables)
			{
				
				//var newVariable = variable.ReplaceToken(variable.Identifier, SyntaxFactory.Identifier(GetChangedName(variable.Identifier.ValueText))
				//	.WithLeadingTrivia(variable.Identifier.LeadingTrivia)
				//	.WithTrailingTrivia(variable.Identifier.TrailingTrivia))
				//	.WithLeadingTrivia(variable.GetLeadingTrivia())
				//	.WithTrailingTrivia(variable.GetTrailingTrivia());
				
				var newVariable = variable.WithIdentifier(SyntaxFactory.Identifier(GetChangedName(variable.Identifier.ValueText))
					.WithLeadingTrivia(variable.Identifier.LeadingTrivia)
					.WithTrailingTrivia(variable.Identifier.TrailingTrivia))
					.WithLeadingTrivia(variable.GetLeadingTrivia())
					.WithTrailingTrivia(variable.GetTrailingTrivia());

				variables.Add(newVariable);
			}

			field = field
				.WithDeclaration(SyntaxFactory.VariableDeclaration(field.Declaration.Type, SyntaxFactory.SeparatedList(variables)))
				.WithLeadingTrivia(field.GetLeadingTrivia())
				.WithTrailingTrivia(field.GetTrailingTrivia());
			
			return field;
		}

		private string GetChangedName(string oldName)
		{
			return Regex.Replace(oldName, _find, _replace, RegexOptions.None);
		}
	}
}