using System;

namespace SolutionValidator.Core.Validator.ProjectFile
{
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