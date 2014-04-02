namespace SolutionValidator.Core.Validator.Common
{
	public abstract class Rule
	{
		public abstract ValidationResult Validate(RepositoryInfo repositoryInfo);
	}
}