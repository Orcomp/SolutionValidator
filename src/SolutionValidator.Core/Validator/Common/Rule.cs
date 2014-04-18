using System;
using SolutionValidator.Core.Infrastructure.Logging;

namespace SolutionValidator.Core.Validator.Common
{
	public abstract class Rule
	{
		protected ILogger logger;

		protected Rule(ILogger logger)
		{
			this.logger = logger;
		}

		public abstract ValidationResult Validate(RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null);
	}
}