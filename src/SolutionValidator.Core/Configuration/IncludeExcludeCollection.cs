#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IncludeExcludeCollection.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Configuration
{
	#region using...
	using System.Configuration;
	#endregion

	[ConfigurationCollection(typeof (IncludeExcludeElement))]
	
	public class IncludeExcludeCollection : ConfigurationElementCollection
	{
		public IncludeExcludeElement this[int idx]
		{
			get { return (IncludeExcludeElement) BaseGet(idx); }
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new IncludeExcludeElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((IncludeExcludeElement) (element)).Exclude + ((IncludeExcludeElement) (element)).Include;
		}

		public void Add(IncludeExcludeElement item)
		{
			base.BaseAdd(item);
		}
	}
}