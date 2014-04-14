using System.Reflection;
using System.Windows;
using Catel.MVVM;
using SolutionValidator.Core.Infrastructure.Logging;
using SolutionValidator.UI.Wpf.Properties;

namespace SolutionValidator.UI.Wpf.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
	    private Rect restoreBounds;

	    /// <summary>
		///     Initializes a new instance of the MainWindowViewModel class.
		/// </summary>
		public MainWindowViewModel(ILogger logger)
		{
	        RestoreMainWindow();
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets the window title.
		/// </summary>
		/// <value>The title.</value>
		public string Title
		{
			get { return string.Format("Solution Validator v{0}", Assembly.GetExecutingAssembly().GetName().Version); }
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the main window top.
		/// </summary>
		/// <value>The main window top.</value>
        public int MainWindowTop { get; set; }		

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the main window left.
		/// </summary>
		/// <value>The main window left.</value>
		public int MainWindowLeft { get; set; }		

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the width of the main window.
		/// </summary>
		/// <value>The width of the main window.</value>
		public int MainWindowWidth { get; set; }

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the height of the main window.
		/// </summary>
		/// <value>The height of the main window.</value>
		public int MainWindowHeight { get; set; }		

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the state of the main window.
		/// </summary>
		/// <value>The state of the main window.</value>
		public WindowState MainWindowState { get; set; }

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the restore bounds.
		/// </summary>
		/// <value>The restore bounds.</value>
		public Rect RestoreBounds
		{
			get { return restoreBounds; }
			set { restoreBounds = value; }
		}

        /// <summary>
        /// Called when the view model has just been closed.
        ///             
        /// <para />
        ///             This method also raises the 
        /// <see cref="E:Catel.MVVM.ViewModelBase.Closed" /> event.
        /// </summary>
        /// <param name="result">The result to pass to the view. This will, for example, be used as <c>DialogResult</c>.</param>
	    protected override void OnClosed(bool? result)
	    {
	        base.OnClosed(result);
            SaveMainWindow();
	    }

	    /// <summary>
		///     Restores the main window position an size
		/// </summary>
		private void RestoreMainWindow()
		{
			MainWindowTop = Settings.Default.MainWindowTop;
			MainWindowLeft = Settings.Default.MainWindowLeft;
			MainWindowHeight = Settings.Default.MainWindowHeight;
			MainWindowWidth = Settings.Default.MainWindowWidth;
			MainWindowState = Settings.Default.MainWindowMaximized ? WindowState.Maximized : WindowState.Normal;
		}

		/// <summary>
		///     Saves the main window position and size.
		/// </summary>
		private void SaveMainWindow()
		{
			if (MainWindowState == WindowState.Maximized)
			{
				// Using RestoreBounds as the current values will be 0, 0 and the size of the screen.
				// As RestoreBounds is a read only property getting (binding) RestoreBounds done via data piping.
				Settings.Default.MainWindowTop = (int) RestoreBounds.Top;
				Settings.Default.MainWindowLeft = (int) RestoreBounds.Left;
				Settings.Default.MainWindowHeight = (int) RestoreBounds.Height;
				Settings.Default.MainWindowWidth = (int) RestoreBounds.Width;
				Settings.Default.MainWindowMaximized = true;
			}
			else
			{
				Settings.Default.MainWindowTop = MainWindowTop;
				Settings.Default.MainWindowLeft = MainWindowLeft;
				Settings.Default.MainWindowHeight = MainWindowHeight;
				Settings.Default.MainWindowWidth = MainWindowWidth;
				Settings.Default.MainWindowMaximized = false;
			}
			Settings.Default.Save();
		}
	}
}