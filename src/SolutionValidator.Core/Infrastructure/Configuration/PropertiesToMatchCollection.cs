using System.Configuration;

namespace SolutionValidator.Core.Infrastructure.Configuration
{
	[ConfigurationCollection(typeof (ConfigurationNameElement))]
	public class PropertiesToMatchCollection : ConfigurationElementCollection
	{
		[ConfigurationProperty(SolutionValidatorConfigurationSection.CheckAttributeName, DefaultValue = "true")]
		public bool Check
		{
			get { return (bool) base[SolutionValidatorConfigurationSection.CheckAttributeName]; }
		}

		public PropertiesToMatchElement this[int idx]
		{
			get { return (PropertiesToMatchElement) BaseGet(idx); }
		}


		protected override ConfigurationElement CreateNewElement()
		{
			return new PropertiesToMatchElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((PropertiesToMatchElement) (element)).PropertyName +
			       ((PropertiesToMatchElement) (element)).OtherPropertyName;
		}

		public void Add(PropertiesToMatchElement item)
		{
			base.BaseAdd(item);
		}
	}
}