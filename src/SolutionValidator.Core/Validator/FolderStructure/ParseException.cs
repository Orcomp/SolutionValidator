// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParseException.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Validator.FolderStructure
{
    using System;

	public class ParseException : Exception
	{
		public ParseException(string message, int lineNumber, int column) : base(message)
		{
			LineNumber = lineNumber;
			Column = column;
		}

		public int LineNumber { get; set; }
		public int Column { get; set; }
	}
}