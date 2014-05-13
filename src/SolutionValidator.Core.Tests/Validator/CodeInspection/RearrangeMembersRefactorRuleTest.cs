#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="RearrangeMembersRefactorRuleTest.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Tests.Validator.CodeInspection
{
	#region using...
	using NUnit.Framework;
	using SolutionValidator.CodeInspection.Refactoring;

	#endregion  

	[TestFixture]
	public class RearrangeMembersTreeRefactorRuleTest : TransformRuleBaseTest
	{
		[Test]
		[TestCase("DoNotTouch", "DoNotTouch", true)]
		[TestCase("MovePrivateFields", "DoNotTouch", true)]
		[TestCase("MovePublicFields", "DoNotTouch", false)]
		[TestCase("MoveInternalFields", "DoNotTouch", true)]
		[TestCase("MoveProtectedInternalFields", "DoNotTouch", true)]
		[TestCase("MoveProtectedFields", "DoNotTouch", true)]
		[TestCase("MoveConstructor", "DoNotTouch", true)]
		[TestCase("MoveDestructor", "DoNotTouch", true)]
		[TestCase("MoveDelegate", "DoNotTouch", true)]
		[TestCase("MoveEvent", "DoNotTouch", true)]
		[TestCase("MoveEnum", "DoNotTouch", true)]
		[TestCase("MoveInterface", "DoNotTouch", true)]
		[TestCase("MoveProperty", "DoNotTouch", true)]
		[TestCase("MoveIndexer", "DoNotTouch", true)]
		[TestCase("MoveMethod", "DoNotTouch", true)]
		[TestCase("MoveNestedStruct", "DoNotTouch", true)]
		[TestCase("MoveNestedClass", "DoNotTouch", false)]
		public void Test(string original, string expected, bool isExactExpected)
		{
			Test<RearrangeMembersTreeRefactorRule>(@"RearrangeMembers", original, expected, isExactExpected);
		}
	}
}