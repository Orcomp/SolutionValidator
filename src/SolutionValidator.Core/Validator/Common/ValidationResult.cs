namespace SolutionValidator.Core.Validator.Common
{
	public class ValidationResult
	{
		public ValidationResult(bool isValid = true, string description = "")
		{
			IsValid = isValid;
			Description = description;
			InternalErrorCount = 0;
		}

		public bool IsValid { get; set; }
		public int InternalErrorCount { get; set; }
		public string Description { get; set; }
	}
}