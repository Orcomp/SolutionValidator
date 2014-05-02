#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ParseException.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.FolderStructure
{
	#region using...
	using System;

	#endregion

	public class ParseException : ApplicationException
	{
		public ParseException(string message, int lineNumber, int column)
			: base(message)
		{
			LineNumber = lineNumber;
			Column = column;
		}

		public int LineNumber { get; set; }
		public int Column { get; set; }
	}
}