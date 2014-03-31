namespace SolutionValidator.Core.Validator.Common
{
	public abstract class Rule
	{
		public abstract ValidationResult Validate(ProjectInfo projectInfo);
	}
}