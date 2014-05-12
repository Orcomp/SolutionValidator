#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="RenamePrivateFieldsRewriter.cs" company="Orcomp development team">
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

	#endregion

	public class RenamePrivateFieldsRewriter : CSharpSyntaxRewriter
	{
		private readonly dynamic _parameters;
		private readonly SemanticModel _semanticModel;

		public RenamePrivateFieldsRewriter(dynamic parameters, SemanticModel semanticModel)
		{
			_parameters = parameters;
			_semanticModel = semanticModel;
		}

		public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax name)
		{
			// TODO: Hanlde initializer list correctly

			var symbolInfo = _semanticModel.GetSymbolInfo(name);
			var fieldSymbol = symbolInfo.Symbol as IFieldSymbol;

			if (fieldSymbol != null
			    && fieldSymbol.DeclaredAccessibility == Accessibility.Private
			    && !fieldSymbol.IsConst
			    && !fieldSymbol.IsStatic)
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

			if (field.Modifiers.Any(SyntaxKind.StaticKeyword))
			{
				return field;
			}

			//var variables = new List<VariableDeclaratorSyntax>();
			var variables = new List<VariableDeclaratorSyntax>();
			foreach (var variable in field.Declaration.Variables)
			{
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
			return Regex.Replace(oldName, _parameters.Find, _parameters.Replace, RegexOptions.None);
		}
	}
}