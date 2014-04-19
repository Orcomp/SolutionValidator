// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SolutionValidator.ViewModels
{
    using System.Reflection;
    using Catel.MVVM;
    using Catel.Reflection;

    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        ///     Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public MainViewModel()
        {
            var assembly = Assembly.GetExecutingAssembly();

            Title = string.Format("{0} v{1}", assembly.Title(), assembly.Version());
        }
    }
}