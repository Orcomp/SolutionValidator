#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IncludeExcludeElement.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Configuration
{
	#region using...
	using System.Configuration;

	#endregion

	public class IncludeExcludeElement : ConfigurationElement
	{
		private const string ExcludeAttributeName = "exclude";
		private const string IncludeAttributeName = "include";

		[ConfigurationProperty(ExcludeAttributeName, DefaultValue = "", IsKey = false, IsRequired = false)]
		public string Exclude
		{
			get { return ((string) (this[ExcludeAttributeName])); }
			set { this[ExcludeAttributeName] = value; }
		}

		[ConfigurationProperty(IncludeAttributeName, DefaultValue = "", IsKey = false, IsRequired = false)]
		public string Include
		{
			get { return ((string) (this[IncludeAttributeName])); }
			set { this[IncludeAttributeName] = value; }
		}
	}
}