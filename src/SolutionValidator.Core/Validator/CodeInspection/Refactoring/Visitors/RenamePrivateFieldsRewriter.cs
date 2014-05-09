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
		private Workspace _workspace;

		public RenamePrivateFieldsRewriter(string find, string replace, SemanticModel semanticModel, Workspace workspace)
		{
			_find = find;
			_replace = replace;
			_semanticModel = semanticModel;
			_workspace = workspace;
		}

		public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax name)
		{
			var symbolInfo = _semanticModel.GetSymbolInfo(name);
			var fieldSymbol = symbolInfo.Symbol as IFieldSymbol;
			
			if (fieldSymbol != null 
				&& fieldSymbol.DeclaredAccessibility == Accessibility.Private 
				&& !fieldSymbol.IsConst)
			{
				
				//name = name.WithLeadingTrivia().WithTrailingTrivia().Update(SyntaxFactory.Identifier(GetChangedName(name.Identifier.ValueText)));
				name = name.WithIdentifier(SyntaxFactory.Identifier(GetChangedName(name.Identifier.ValueText)));
				//name.NormalizeWhitespace();
				//name = (IdentifierNameSyntax) Formatter.Format(name, _workspace);
				//name = name.WithAdditionalAnnotations(Formatter.Annotation);
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
				variables.Add(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(GetChangedName(variable.Identifier.ValueText))));
			}

			field = field.WithDeclaration(SyntaxFactory.VariableDeclaration(field.Declaration.Type, SyntaxFactory.SeparatedList(variables)));
			field = field.WithAdditionalAnnotations(Formatter.Annotation);
			return field;
		}

		private string GetChangedName(string oldName)
		{
			return Regex.Replace(oldName, _find, _replace, RegexOptions.None);
		}
	}
}