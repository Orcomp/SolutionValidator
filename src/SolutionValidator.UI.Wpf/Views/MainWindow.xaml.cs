// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SolutionValidator.Views
{
    using System;
    using System.Windows;
    using Catel.Windows;

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constructors
        public MainWindow()
            : base(DataWindowMode.Custom)
        {
            InitializeComponent();
            SizeToContent = SizeToContent.Manual;
        }
        #endregion

        #region Methods
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            ViewModel.CloseViewModel(null);
        }
        #endregion
    }
}