#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectFileException.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.ProjectFile
{
	#region using...
	using System;

	#endregion

	public class ProjectFileException : ApplicationException
	{
		public ProjectFileException(string message)
			: base(message)
		{
		}

		public ProjectFileException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}