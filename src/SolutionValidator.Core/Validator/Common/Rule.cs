// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Rule.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Validator.Common
{
    using System;

    public abstract class Rule
	{
		public abstract ValidationResult Validate(RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null);
	}
}