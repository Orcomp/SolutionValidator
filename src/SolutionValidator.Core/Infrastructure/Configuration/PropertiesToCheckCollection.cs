using System.Configuration;

namespace SolutionValidator.Core.Infrastructure.Configuration
{
	[ConfigurationCollection(typeof (ConfigurationNameElement))]
	public class PropertiesToCheckCollection : ConfigurationElementCollection
	{
		[ConfigurationProperty(SolutionValidatorConfigurationSection.CheckAttributeName, DefaultValue = "true")]
		public bool Check
		{
			get { return (bool) base[SolutionValidatorConfigurationSection.CheckAttributeName]; }
		}

		public PropertyToCheckElement this[int idx]
		{
			get { return (PropertyToCheckElement) BaseGet(idx); }
		}


		protected override ConfigurationElement CreateNewElement()
		{
			return new PropertyToCheckElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((PropertyToCheckElement) (element)).PropertyName;
		}

		public void Add(PropertyToCheckElement item)
		{
			base.BaseAdd(item);
		}
	}
}