// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BootStrapper.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Infrastructure
{
    using Catel.IoC;
    using Validator.FolderStructure;
    using Validator.ProjectFile;

    /// <summary>
	///     Dependency Injection initializer class
	/// </summary>
	public static class BootStrapper
	{
		#region Constants and Fields
        #endregion

		#region Public Methods and Operators		
		#endregion

		#region Methods

		/// <summary>
		///     Registers the services / interface bindings.		
		/// </summary>
		public static void RegisterServices()
		{
            ServiceLocator.Default.RegisterInstance<IFileSystemHelper>(new FileSystemHelper());
            ServiceLocator.Default.RegisterInstance<IProjectFileHelper>(new ProjectFileHelper());
		}

		#endregion
	}
}