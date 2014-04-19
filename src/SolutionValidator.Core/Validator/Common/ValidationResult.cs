namespace SolutionValidator.Validator
{
	public class ValidationResult
	{
		public ValidationResult(bool isValid, string description)
		{
			IsValid = isValid;
			Description = description;
		}

		public bool IsValid { get; set; }
		public string Description { get; set; }
	}
}