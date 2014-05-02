#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyToCheckElement.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Configuration
{
	#region using...
	using System.Configuration;

	#endregion

	public class PropertyToCheckElement : ConfigurationElement
	{
		#region Constants
		private const string PropertyNameAttributeName = "propertyName";

		private const string ValueAttributeName = "value";
		#endregion

		#region Properties
		[ConfigurationProperty(PropertyNameAttributeName, DefaultValue = "", IsKey = true, IsRequired = true)]
		public string PropertyName
		{
			get { return ((string) (this[PropertyNameAttributeName])); }
			set { this[PropertyNameAttributeName] = value; }
		}

		[ConfigurationProperty(ValueAttributeName, DefaultValue = "", IsRequired = true)]
		public string Value
		{
			get { return ((string) (this[ValueAttributeName])); }
			set { this[ValueAttributeName] = value; }
		}
		#endregion
	}
}