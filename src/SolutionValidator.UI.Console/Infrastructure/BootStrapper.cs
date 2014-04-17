using Ninject;
using SolutionValidator.Core.Infrastructure.DependencyInjection;
using SolutionValidator.Core.Infrastructure.Logging;
using SolutionValidator.Core.Infrastructure.Logging.Log4Net;
using SolutionValidator.Core.Validator.FolderStructure;
using SolutionValidator.Core.Validator.ProjectFile;

namespace SolutionValidator.UI.Console.Infrastructure
{
	/// <summary>
	///     Dependency Injection initializer class
	/// </summary>
	public static class BootStrapper
	{
		#region Constants and Fields

		private static bool isInitialized;

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///     Creates dependency injection kernel for bindings.
		/// </summary>
		/// <returns>IKernel.</returns>
		public static IKernel CreateKernel()
		{
			if (isInitialized)
			{
				isInitialized = true;
				return null;
			}
			var kernel = new StandardKernel();
			RegisterServices(kernel);
			Dependency.Initialize(new NinjectResolver(kernel));
			return kernel;
		}

		#endregion

		#region Methods

		/// <summary>
		///     Registers the services / interface bindings.
		///     Place all DI bindings here:
		/// </summary>
		/// <param name="kernel">The kernel.</param>
		private static void RegisterServices(IKernel kernel)
		{
			kernel.Bind<ILogger>().To<Log4NetLogger>().InThreadScope();
			kernel.Bind<IFileSystemHelper>().To<FileSystemHelper>().InSingletonScope();
			kernel.Bind<IProjectFileHelper>().To<ProjectFileHelper>().InSingletonScope();
		}

		#endregion
	}
}