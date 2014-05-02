#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="Rule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Common
{
	#region using...
	using System;

	#endregion

	public abstract class Rule
	{
		public abstract ValidationResult Validate(RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null);
	}
}