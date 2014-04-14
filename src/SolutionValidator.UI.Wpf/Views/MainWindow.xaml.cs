using System;
using System.Windows;
using Catel.Windows;

namespace SolutionValidator.UI.Wpf.Views
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
	    public MainWindow()
            : base(DataWindowMode.Custom)
		{
			InitializeComponent();
            SizeToContent = SizeToContent.Manual;
		}
        
	    protected override void OnClosed(EventArgs e)
	    {
	        base.OnClosed(e);

            ViewModel.CloseViewModel(null);
	    }
	}
}