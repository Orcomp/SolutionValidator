using System.Configuration;

namespace SolutionValidator.Core.Infrastructure.Configuration
{
	public class ConfigurationNameElement : ConfigurationElement
	{
		// MS Parser: property cannot have the name 'configurationName' because it
		// starts with the reserved prefix 'config' or 'lock'. 
		// So we will just call it: 'name'
		private const string ConfigurationNameAttributeName = "name";

		[ConfigurationProperty(ConfigurationNameAttributeName, DefaultValue = "", IsKey = true, IsRequired = true)]
		public string Name
		{
			get { return ((string) (this[ConfigurationNameAttributeName])); }
			set { this[ConfigurationNameAttributeName] = value; }
		}
	}
}