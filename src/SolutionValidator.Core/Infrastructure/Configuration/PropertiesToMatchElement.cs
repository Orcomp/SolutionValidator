using System.Configuration;

namespace SolutionValidator.Core.Infrastructure.Configuration
{
	public class PropertiesToMatchElement : ConfigurationElement
	{
		private const string PropertyNameAttributeName = "propertyName";

		private const string OtherPropertyNameAttributeName = "otherPropertyName";

		[ConfigurationProperty(PropertyNameAttributeName, DefaultValue = "", IsRequired = true)]
		public string PropertyName
		{
			get { return ((string) (this[PropertyNameAttributeName])); }
			set { this[PropertyNameAttributeName] = value; }
		}

		[ConfigurationProperty(OtherPropertyNameAttributeName, DefaultValue = "", IsRequired = true)]
		public string OtherPropertyName
		{
			get { return ((string) (this[OtherPropertyNameAttributeName])); }
			set { this[OtherPropertyNameAttributeName] = value; }
		}
	}
}