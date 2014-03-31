using System.Windows;
using System.Windows.Interactivity;

namespace SolutionValidator.UI.Wpf.View.Behaviors
{
	/// <summary>
	///     Piper Class is a general solution/workaround for pushing read-only GUI properties back into ViewModel
	///     Even we are trying to use OneWayToSource Binding for a ReadOnly Dependency Property, we got
	///     error MC3065: "..." property is read-only and cannot be set from markup.
	///     (This project uses it actually for WCF Window class RestoreBounds property)
	/// </summary>
	public class Piper : Behavior<Window>
	{
		#region Constants and Fields

		/// <summary>
		///     The data pipes property
		/// </summary>
		public static readonly DependencyProperty DataPipesProperty =
			DependencyProperty.RegisterAttached(
				"DataPipes",
				typeof (DataPipeCollection),
				typeof (Piper),
				new UIPropertyMetadata(null));

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///     Gets the data pipes.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns>DataPipeCollection.</returns>
		public static DataPipeCollection GetDataPipes(DependencyObject o)
		{
			return (DataPipeCollection) o.GetValue(DataPipesProperty);
		}

		/// <summary>
		///     Sets the data pipes.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="value">The value.</param>
		public static void SetDataPipes(DependencyObject o, DataPipeCollection value)
		{
			o.SetValue(DataPipesProperty, value);
		}

		#endregion
	}
}