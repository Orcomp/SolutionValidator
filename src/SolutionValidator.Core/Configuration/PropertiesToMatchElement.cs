#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertiesToMatchElement.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Configuration
{
	#region using...
	using System.Configuration;

	#endregion

	public class PropertiesToMatchElement : ConfigurationElement
	{
		#region Constants
		private const string PropertyNameAttributeName = "propertyName";

		private const string OtherPropertyNameAttributeName = "otherPropertyName";
		#endregion

		#region Properties
		[ConfigurationProperty(PropertyNameAttributeName, DefaultValue = "", IsRequired = true)]
		public string PropertyName
		{
			get { return ((string) (this[PropertyNameAttributeName])); }
			set { this[PropertyNameAttributeName] = value; }
		}

		[ConfigurationProperty(OtherPropertyNameAttributeName, DefaultValue = "", IsRequired = true)]
		public string OtherPropertyName
		{
			get { return ((string) (this[OtherPropertyNameAttributeName])); }
			set { this[OtherPropertyNameAttributeName] = value; }
		}
		#endregion
	}
}