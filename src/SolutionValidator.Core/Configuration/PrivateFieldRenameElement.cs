#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="PrivateFieldRenameElement.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Configuration
{
	#region using...
	using System.Configuration;
	#endregion

     // ReSharper disable once ClassNeverInstantiated.Global
	public class PrivateFieldRenameElement : ConfigurationElement
	{
		private const string FindAttributeName = "find";

		private const string ReplaceAttributeName = "replace";

		[ConfigurationProperty(FindAttributeName, DefaultValue = "^([a-zA-Z][a-zA-Z0-9_]*$)")]
		public string Find
		{
			get { return (string) base[FindAttributeName]; }
		}

		[ConfigurationProperty(ReplaceAttributeName, DefaultValue = "_$1")]
		public string Replace
		{
			get { return (string) base[ReplaceAttributeName]; }
		}

		[ConfigurationProperty(SolutionValidatorConfigurationSection.CheckAttributeName, DefaultValue = "true")]
		public bool Check
		{
			get { return (bool) base[SolutionValidatorConfigurationSection.CheckAttributeName]; }
		}
	}
}