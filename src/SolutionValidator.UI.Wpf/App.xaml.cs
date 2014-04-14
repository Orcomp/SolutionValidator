using System;
using System.Threading;
using System.Windows;
using SolutionValidator.Core.Infrastructure.DependencyInjection;
using SolutionValidator.Core.Infrastructure.Logging;

namespace SolutionValidator.UI.Wpf
{
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		#region Methods

		/// <summary>
		///     Raises the <see cref="E:System.Windows.Application.Exit" /> event.
		///     Deactivates GlobalClickListener click hook (via Dispose())
		///     Stops the highlighter thread (via Dispose())
		/// </summary>
		/// <param name="e">
		///     An <see cref="T:System.Windows.ExitEventArgs" /> that contains the event data.
		/// </param>
		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);
			try
			{
				Dependency.Resolve<ILogger>().Info("Application terminated normally.");
			}
			catch (Exception exception)
			{
				Dependency.Resolve<ILogger>().Error(exception);
			}
		}

		/// <summary>
		///     Raises the <see cref="E:System.Windows.Application.Startup" /> event.
		///     Activates GlobalClickListener click hook
		///     Starts the highlighter thread
		/// </summary>
		/// <param name="e">
		///     A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.
		/// </param>
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			try
			{				
				BootStrapper.RegisterServices();

				// This is only for verbose logging info and debug info
				Thread.CurrentThread.Name = "UI";

				// Informational log message
				Dependency.Resolve<ILogger>().Info("Application started.");
			}
			catch (Exception exception)
			{
				Dependency.Resolve<ILogger>().Error(exception);
			}
		}

		#endregion
	}
}