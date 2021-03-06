﻿#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="OutputPathElement.cs" company="Orcomp development team">
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
	public class OutputPathElement : ConfigurationElement
	{
		#region Constants
		private const string ValueAttributeName = "value";
		#endregion

		#region Properties
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
		#endregion
	}
}