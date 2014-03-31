using Ninject;
using SolutionValidator.Core.Infrastructure.DependencyInjection;
using SolutionValidator.Core.Infrastructure.Logging;
using SolutionValidator.Core.Infrastructure.Logging.Log4Net;
using SolutionValidator.Core.Validator.FolderStructure;
using SolutionValidator.UI.Wpf.ViewModel;

namespace SolutionValidator.UI.Wpf
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
			kernel.Bind<IViewService>().To<WpfViewService>().InSingletonScope();

			kernel.Bind<MainViewModel>().ToConstructor(x => new MainViewModel(
				x.Inject<ILogger>(),
				x.Inject<IViewService>())).InSingletonScope();

			kernel.Bind<IFileSystemHelper>().To<FileSystemHelper>().InSingletonScope();
		}

		#endregion
	}
}