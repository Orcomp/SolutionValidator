#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Models
{
	#region using...
	using Catel;
	using Catel.IO;

	#endregion

	public static class ContextExtensions
	{
		public static string GetFullPath(this Context context, string relativePath)
		{
			Argument.IsNotNull(() => context);
			Argument.IsNotNull(() => relativePath);

			return Path.Combine(context.WorkingDirectory, relativePath);
		}
	}
}