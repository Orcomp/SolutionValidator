using SolutionValidator.Core.Infrastructure.DependencyInjection;
using SolutionValidator.Core.Infrastructure.Logging;
using SolutionValidator.Core.Infrastructure.Logging.Log4Net;
using SolutionValidator.Core.Validator.FolderStructure;
using Catel.IoC;

namespace SolutionValidator.UI.Wpf
{
	/// <summary>
	///     Dependency Injection initializer class
	/// </summary>
	public static class BootStrapper
	{		
		#region Methods
		
		public  static void RegisterServices()
		{
		    var serviceLocator = Catel.IoC.ServiceLocator.Default;
            serviceLocator.RegisterType<ILogger, Log4NetLogger>();
            serviceLocator.RegisterInstance<IFileSystemHelper>(new FileSystemHelper());
            Dependency.Initialize(new CatelResolver());
		}

		#endregion
	}
}