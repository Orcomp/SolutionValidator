#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectFileElement.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Configuration
{
	#region using...
	using System.Configuration;
	using Properties;

	#endregion

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