#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="RearrangeMembersRewriter.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.CodeInspection.Refactoring
{
	#region using...
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CSharp;
	using Microsoft.CodeAnalysis.CSharp.Syntax;

	#endregion

	// ReSharper disable once ClassNeverInstantiated.Global
	public class RearrangeMembersRewriter : CSharpSyntaxRewriter
	{
		private readonly List<List<SyntaxKind>> _modifierOrder = new List<List<SyntaxKind>>()
		{
			new List<SyntaxKind> {SyntaxKind.PublicKeyword},
			new List<SyntaxKind> {SyntaxKind.InternalKeyword},
			new List<SyntaxKind> {SyntaxKind.InternalKeyword, SyntaxKind.ProtectedKeyword},
			new List<SyntaxKind> {SyntaxKind.ProtectedKeyword},
			new List<SyntaxKind> {SyntaxKind.PrivateKeyword}
		};

		private readonly List<SyntaxKind> _memberKindOrder = new List<SyntaxKind>
		{
			SyntaxKind.ExternAliasDirective,
			SyntaxKind.UsingDirective,
			SyntaxKind.NamespaceDeclaration,
			SyntaxKind.FieldDeclaration,
			SyntaxKind.ConstructorDeclaration,
			SyntaxKind.DestructorDeclaration,
			SyntaxKind.DelegateDeclaration,
			SyntaxKind.EventFieldDeclaration,
			SyntaxKind.EnumDeclaration,
			SyntaxKind.InterfaceDeclaration,
			SyntaxKind.PropertyDeclaration,
			SyntaxKind.IndexerDeclaration,
			SyntaxKind.MethodDeclaration,
			SyntaxKind.OperatorDeclaration,
			SyntaxKind.ConversionOperatorDeclaration,
			SyntaxKind.StructDeclaration,
			SyntaxKind.ClassDeclaration
		};

		private readonly SemanticModel _semanticModel;

		public RearrangeMembersRewriter(dynamic dummy, SemanticModel semanticModel)
		{
			_semanticModel = semanticModel;
		}

		public override SyntaxNode Visit(SyntaxNode node)
		{
			// Why are we keep called with nulls? Puzzler...
			if (node == null)
			{
				return base.Visit(null);
			}

			if (new[]
			{
				SyntaxKind.ClassDeclaration,
				SyntaxKind.StructDeclaration,
				SyntaxKind.InterfaceDeclaration,
				SyntaxKind.NamespaceDeclaration
			}.All(k => k != node.CSharpKind()))
			{
				return base.Visit(node);
			}

			var comparer = new MemberComparer(_memberKindOrder, _modifierOrder);

			if (new[]
			{
				SyntaxKind.ClassDeclaration,
				SyntaxKind.StructDeclaration,
				SyntaxKind.InterfaceDeclaration
			}.Any(k => k == node.CSharpKind()))
			{
				var typeNode = node as TypeDeclarationSyntax;
				// ReSharper disable once PossibleNullReferenceException
				var orderedMembers = typeNode.Members.OrderBy(x => x, comparer);
				switch (node.CSharpKind())
				{
						// Unfortunatelly the common ancestor TypeDeclarationSyntax has _no_ virtual WithMembers member :-(
					case SyntaxKind.ClassDeclaration:
						node = ((ClassDeclarationSyntax) typeNode).WithMembers(SyntaxFactory.List(orderedMembers));
						break;
					case SyntaxKind.StructDeclaration:
						node = ((StructDeclarationSyntax) typeNode).WithMembers(SyntaxFactory.List(orderedMembers));
						break;
					case SyntaxKind.InterfaceDeclaration:
						node = ((InterfaceDeclarationSyntax) typeNode).WithMembers(SyntaxFactory.List(orderedMembers));
						break;
				}
			}
			// TODO: Namespace
			//var orderedMembers = node.GetMembers().OrderBy(x => x, comparer);
			//node = node.WithMembers(Syntax.List<MemberDeclarationSyntax>(orderedMembers));

			return base.Visit(node);
		}

		private static SyntaxTokenList GetModifiers(SyntaxNode node)
		{
			// BaseFieldDeclarationSyntax
			// BaseMethodDeclarationSyntax
			// BasePropertyDeclarationSyntax
			// BaseTypeDeclarationSyntax
			// DelegateDeclarationSyntax
			// EnumMemberDeclarationSyntax
			// GlobalStatementSyntax
			// IncompleteMemberSyntax
			// NamespaceDeclarationSyntax

			var property = node.GetType().GetProperty("Modifiers");

			if (property != null)
			{
				var syntaxTokenList = (SyntaxTokenList) property.GetValue(node, null);
				if (syntaxTokenList.Count == 0)
				{
					syntaxTokenList = syntaxTokenList.Add(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
				}
				return syntaxTokenList;
			}
			return new SyntaxTokenList();
		}

		private class MemberComparer : IComparer<MemberDeclarationSyntax>
		{
			private readonly List<List<SyntaxKind>> _modifierOrder;
			private readonly IList<SyntaxKind> _memberKindOrder;

			public MemberComparer(IList<SyntaxKind> memberKindOrder, List<List<SyntaxKind>> modifierOrder)
			{
				_memberKindOrder = memberKindOrder;
				_modifierOrder = modifierOrder;
			}

			public int Compare(MemberDeclarationSyntax member, MemberDeclarationSyntax otherMember)
			{
				var rankMember = _memberKindOrder.IndexOf(member.CSharpKind());
				var rankOtherMember = _memberKindOrder.IndexOf(otherMember.CSharpKind());

				if (rankMember != rankOtherMember)
				{
					return rankMember - rankOtherMember;
				}

				rankMember = GetRankByModifier(_modifierOrder, GetModifiers(member).Select(st => st.CSharpKind()).OrderBy(sk => sk).ToList());
				rankOtherMember = GetRankByModifier(_modifierOrder, GetModifiers(otherMember).Select(st => st.CSharpKind()).OrderBy(sk => sk).ToList());

				return rankMember - rankOtherMember;
			}

			private int GetRankByModifier(IEnumerable<List<SyntaxKind>> modifierOrder, IList<SyntaxKind> pattern)
			{
				var result = -1;

				foreach (var syntaxKinds in modifierOrder)
				{
					result++;
					if (syntaxKinds.Count != pattern.Count)
					{
						continue;
					}

					var index = 0;
					if (syntaxKinds.OrderBy(sk => sk).All(syntaxKind => syntaxKind == pattern[index++]))
					{
						return result;
					}
				}
				return 0;
			}
		}
	}
}