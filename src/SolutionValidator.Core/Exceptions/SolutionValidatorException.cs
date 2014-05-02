#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="SolutionValidatorException.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator
{
	#region using...
	using System;

	#endregion

	public class SolutionValidatorException : ApplicationException
	{
		public SolutionValidatorException(string message, params object[] args)
			: base(string.Format(message, args))
		{
		}
	}
}