// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationNameCollection.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Infrastructure.Configuration
{
    using System.Configuration;

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