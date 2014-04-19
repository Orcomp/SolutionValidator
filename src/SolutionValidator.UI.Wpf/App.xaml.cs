// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SolutionValidator
{
    using System;
    using System.Threading;
    using System.Windows;
    using Catel.IoC;
    using Catel.Logging;

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        #region Constants
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
        #endregion

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
                // Why is this in a try/catch? Expecting an exception with logging?
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
                RegisterServices();

                // This is only for verbose logging info and debug info
                Thread.CurrentThread.Name = "UI";

                Logger.Info("Application started.");
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }
        }

        private void RegisterServices()
        {
            var serviceLocator = ServiceLocator.Default;

            // TODO: Register servics
        }
        #endregion
    }
}