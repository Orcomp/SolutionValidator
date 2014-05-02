#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FolderStructure.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Models
{
	#region using...
	using Catel.Data;
	using Catel.Logging;

	#endregion

	public class FolderStructure : ModelBase
	{
		#region Constants
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
		#endregion

		#region Constructors
		public FolderStructure()
		{
			DefinitionFilePath = ".folderStructure";
			Check = true;
		}
		#endregion

		#region Properties
		public string DefinitionFilePath { get; set; }

		public bool Check { get; set; }
		#endregion
	}
}