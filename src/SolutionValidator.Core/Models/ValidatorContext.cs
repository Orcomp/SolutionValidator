#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidatorContext.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Models
{
	#region using...
	using Catel.Data;

	#endregion

	public class ValidatorContext : ModelBase
	{
		public ValidatorContext()
		{
			FolderStructure = new FolderStructure();
			ProjectFile = new ProjectFile();
		}

		public FolderStructure FolderStructure { get; private set; }

		public ProjectFile ProjectFile { get; private set; }
	}
}