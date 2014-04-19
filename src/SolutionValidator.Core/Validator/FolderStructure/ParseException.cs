using System;

namespace SolutionValidator.Validator.FolderStructure
{
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