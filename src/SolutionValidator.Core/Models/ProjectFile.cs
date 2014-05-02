#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectFile.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Models
{
	#region using...
	using System.Collections.ObjectModel;
	using Catel.Data;

	#endregion

	public class ProjectFile : ModelBase
	{
		public ProjectFile()
		{
			RequiredConfigurations = new Collection<string>();
			IdenticalChecks = new Collection<IdenticalCheck>();
			Properties = new Collection<Property>();
		}

		public bool CheckOutPutPath { get; set; }

		public string OutputPath { get; set; }

		public bool CheckRequiredConfigurations { get; set; }

		public Collection<string> RequiredConfigurations { get; private set; }

		public bool CheckIdentical { get; set; }

		public Collection<IdenticalCheck> IdenticalChecks { get; private set; }

		public bool CheckPropertyValues { get; set; }

		public Collection<Property> Properties { get; private set; }
	}
}