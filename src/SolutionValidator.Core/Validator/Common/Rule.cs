namespace SolutionValidator.Validator
{
	public abstract class Rule
	{
		public abstract ValidationResult Validate(RepositoryInfo repositoryInfo);
	}
}