using System;

namespace SolutionValidator.Core.Validator.ProjectFile
{
	public class OutputPathException : Exception
	{
		public OutputPathException(string message) : base(message)
		{
		}

		public OutputPathException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}