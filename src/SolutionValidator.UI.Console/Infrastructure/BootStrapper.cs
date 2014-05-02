#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="BootStrapper.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Infrastructure
{
	#region using...
	using Catel.IoC;
	using FolderStructure;
	using ProjectFile;

	#endregion

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
			var serviceLocator = ServiceLocator.Default;

			serviceLocator.RegisterInstance<IFileSystemHelper>(new FileSystemHelper());
			serviceLocator.RegisterInstance<IProjectFileHelper>(new ProjectFileHelper());
		}
		#endregion
	}
}