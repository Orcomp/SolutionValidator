// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectFileException.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Validator.ProjectFile
{
    using System;

    public class ProjectFileException : ApplicationException
	{
		public ProjectFileException(string message) : base(message)
		{
		}

		public ProjectFileException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}