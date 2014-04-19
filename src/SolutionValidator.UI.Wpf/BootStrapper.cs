using SolutionValidator.Core.Infrastructure.DependencyInjection;
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
            serviceLocator.RegisterInstance<IFileSystemHelper>(new FileSystemHelper());
            Dependency.Initialize(new CatelResolver());
		}

		#endregion
	}
}