namespace SolutionValidator.Core.Validator.Common
{
	public class ValidationMessage
	{
		public ResultLevel ResultLevel { get; set; }
		public string Message { get; set; }
		public bool Processed { get; set; }
	}
}
