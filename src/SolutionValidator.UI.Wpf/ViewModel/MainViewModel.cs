using System.Reflection;
using System.Windows;
using GalaSoft.MvvmLight;
using SolutionValidator.Core.Infrastructure.Logging;
using SolutionValidator.UI.Wpf.Properties;

namespace SolutionValidator.UI.Wpf.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		private ILogger logger;
		private int mainWindowHeight;
		private int mainWindowLeft;
		private WindowState mainWindowState;
		private int mainWindowTop;
		private int mainWindowWidth;
		private Rect restoreBounds;
		private IViewService viewService;

		/// <summary>
		///     Initializes a new instance of the MainViewModel class.
		/// </summary>
		public MainViewModel(ILogger logger, IViewService viewService)
		{
			this.logger = logger;
			this.viewService = viewService;
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
		public int MainWindowTop
		{
			get { return mainWindowTop; }
			set
			{
				mainWindowTop = value;
				RaisePropertyChanged(() => MainWindowTop);
			}
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the main window left.
		/// </summary>
		/// <value>The main window left.</value>
		public int MainWindowLeft
		{
			get { return mainWindowLeft; }
			set
			{
				mainWindowLeft = value;
				RaisePropertyChanged(() => MainWindowLeft);
			}
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the width of the main window.
		/// </summary>
		/// <value>The width of the main window.</value>
		public int MainWindowWidth
		{
			get { return mainWindowWidth; }
			set
			{
				mainWindowWidth = value;
				RaisePropertyChanged(() => MainWindowWidth);
			}
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the height of the main window.
		/// </summary>
		/// <value>The height of the main window.</value>
		public int MainWindowHeight
		{
			get { return mainWindowHeight; }
			set
			{
				mainWindowHeight = value;
				RaisePropertyChanged(() => MainWindowHeight);
			}
		}


		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the state of the main window.
		/// </summary>
		/// <value>The state of the main window.</value>
		public WindowState MainWindowState
		{
			get { return mainWindowState; }
			set
			{
				mainWindowState = value;
				RaisePropertyChanged(() => MainWindowState);
			}
		}

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
		///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
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
			if (Settings.Default.MainWindowMaximized)
			{
				MainWindowState = WindowState.Maximized;
			}
			else
			{
				MainWindowState = WindowState.Normal;
			}
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