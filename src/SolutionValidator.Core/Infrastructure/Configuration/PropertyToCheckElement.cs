using System.Configuration;

namespace SolutionValidator.Core.Infrastructure.Configuration
{
	public class PropertyToCheckElement : ConfigurationElement
	{
		private const string PropertyNameAttributeName = "propertyName";

		private const string ValueAttributeName = "value";

		[ConfigurationProperty(PropertyNameAttributeName, DefaultValue = "", IsKey = true, IsRequired = true)]
		public string PropertyName
		{
			get { return ((string) (this[PropertyNameAttributeName])); }
			set { this[PropertyNameAttributeName] = value; }
		}

		[ConfigurationProperty(ValueAttributeName, DefaultValue = "", IsRequired = true)]
		public string Value
		{
			get { return ((string) (this[ValueAttributeName])); }
			set { this[ValueAttributeName] = value; }
		}
	}
}