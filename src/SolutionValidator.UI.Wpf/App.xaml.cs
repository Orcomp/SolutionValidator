using System;
using System.Threading;
using System.Windows;
using Catel.Logging;
using SolutionValidator.Core.Infrastructure.DependencyInjection;

namespace SolutionValidator.UI.Wpf
{
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

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
				Logger.Info("Application terminated normally.");
			}
			catch (Exception exception)
			{
				Logger.Error(exception);
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
				Logger.Info("Application started.");
			}
			catch (Exception exception)
			{
				Logger.Error(exception);
			}
		}

		#endregion
	}
}