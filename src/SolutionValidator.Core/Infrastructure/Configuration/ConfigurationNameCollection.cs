using System.Configuration;

namespace SolutionValidator.Core.Infrastructure.Configuration
{
	[ConfigurationCollection(typeof (ConfigurationNameElement))]
	public class ConfigurationNameCollection : ConfigurationElementCollection
	{
		[ConfigurationProperty(SolutionValidatorConfigurationSection.CheckAttributeName, DefaultValue = "true")]
		public bool Check
		{
			get { return (bool) base[SolutionValidatorConfigurationSection.CheckAttributeName]; }
		}

		public ConfigurationNameElement this[int idx]
		{
			get { return (ConfigurationNameElement) BaseGet(idx); }
		}


		protected override ConfigurationElement CreateNewElement()
		{
			return new ConfigurationNameElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ConfigurationNameElement) (element)).Name;
		}

		public void Add(ConfigurationNameElement item)
		{
			base.BaseAdd(item);
		}
	}
}