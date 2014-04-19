// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectFileElement.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Infrastructure.Configuration
{
    using System.Configuration;
    using Core.Infrastructure.Configuration;
    using Core.Properties;

    [UsedImplicitly]
	public class ProjectFileElement : ConfigurationElement
	{
		private const string OutputPathElementName = "outputPath";

		private const string RequiredConfigurationsElementName = "requiredConfigurations";

		private const string CheckIdenticalElementName = "checkIdentical";

		private const string CheckForValueElementName = "checkForValue";

		[ConfigurationProperty(OutputPathElementName)]
		public OutputPathElement OutputPath
		{
			get { return (OutputPathElement) base[OutputPathElementName]; }
		}

		[ConfigurationProperty(RequiredConfigurationsElementName)]
		public ConfigurationNameCollection RequiredConfigurations
		{
			get { return ((ConfigurationNameCollection) (base[RequiredConfigurationsElementName])); }
		}

		[ConfigurationProperty(CheckIdenticalElementName)]
		public PropertiesToMatchCollection CheckIdentical
		{
			get { return ((PropertiesToMatchCollection) (base[CheckIdenticalElementName])); }
		}

		[ConfigurationProperty(CheckForValueElementName)]
		public PropertiesToCheckCollection CheckForValue
		{
			get { return ((PropertiesToCheckCollection) (base[CheckForValueElementName])); }
		}
	}
}