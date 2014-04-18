using System.Configuration;
using SolutionValidator.Core.Properties;

namespace SolutionValidator.Core.Infrastructure.Configuration
{
	[UsedImplicitly]
	public class OutputPathElement : ConfigurationElement
	{
		private const string ValueAttributeName = "value";

		[ConfigurationProperty(ValueAttributeName, DefaultValue = "output")]
		public string Value
		{
			get { return (string) base[ValueAttributeName]; }
		}

		[ConfigurationProperty(SolutionValidatorConfigurationSection.CheckAttributeName, DefaultValue = "true")]
		public bool Check
		{
			get { return (bool) base[SolutionValidatorConfigurationSection.CheckAttributeName]; }
		}
	}
}