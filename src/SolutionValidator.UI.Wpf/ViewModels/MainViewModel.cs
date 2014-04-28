// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SolutionValidator.ViewModels
{
    using System.Reflection;
    using Catel;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;

    public class MainViewModel : ViewModelBase
    {
        private readonly ISelectDirectoryService _selectDirectoryService;

        /// <summary>
        ///     Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public MainViewModel(ISelectDirectoryService selectDirectoryService)
        {
            Argument.IsNotNull(() => selectDirectoryService);

            _selectDirectoryService = selectDirectoryService;

            var assembly = Assembly.GetExecutingAssembly();

            Title = string.Format("{0} v{1}", assembly.Title(), assembly.Version());

            OpenSolution = new Command(OnOpenSolutionExecute);
        }

        #region Commands
        /// <summary>
        /// Gets the OpenSolution command.
        /// </summary>
        public Command OpenSolution { get; private set; }

        /// <summary>
        /// Method to invoke when the OpenSolution command is executed.
        /// </summary>
        private void OnOpenSolutionExecute()
        {
            // TODO: Handle command logic here
        }
        #endregion
    }
}